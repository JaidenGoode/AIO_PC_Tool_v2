using AIO_PC_Tool_v2.Models;
using System.Diagnostics;

namespace AIO_PC_Tool_v2.Services;

public interface ICleanerService
{
    Task<List<CleanCategory>> ScanAsync(IProgress<string>? progress = null);
    Task<CleanResult> CleanAsync(IEnumerable<string> categoryIds, IProgress<string>? progress = null);
    Task<List<CleaningHistory>> GetHistoryAsync();
    long GetTotalCleanableSize();
    int GetTotalCleanableFiles();
}

public class CleanResult
{
    public long FreedBytes { get; set; }
    public string FreedHuman { get; set; } = "0 B";
    public int CategoriesCleaned { get; set; }
    public int FilesDeleted { get; set; }
    public List<string> CleanedCategories { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

public class CleanerService : ICleanerService
{
    private readonly string _historyFile;
    private readonly string _appDataDir;
    private List<CleanCategory> _scannedCategories = new();

    public CleanerService()
    {
        _appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AIO_PC_Tool_v2");
        Directory.CreateDirectory(_appDataDir);
        _historyFile = Path.Combine(_appDataDir, "cleaning_history.json");
    }

    public long GetTotalCleanableSize() => _scannedCategories.Sum(c => c.Size);
    public int GetTotalCleanableFiles() => _scannedCategories.Sum(c => c.FileCount);

    public async Task<List<CleanCategory>> ScanAsync(IProgress<string>? progress = null)
    {
        var categories = GetCleanCategories();
        var results = new List<CleanCategory>();

        foreach (var cat in categories)
        {
            progress?.Report($"Scanning {cat.Name}...");
            
            var scanned = new CleanCategory
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
                Paths = cat.Paths,
                GlobDir = cat.GlobDir,
                GlobPattern = cat.GlobPattern
            };

            long totalSize = 0;
            int totalCount = 0;

            try
            {
                if (cat.Id == "recycle")
                {
                    // Get Recycle Bin size via PowerShell
                    var (size, count) = await GetRecycleBinSizeAsync();
                    totalSize = size;
                    totalCount = count;
                }
                else if (!string.IsNullOrEmpty(cat.GlobDir) && !string.IsNullOrEmpty(cat.GlobPattern))
                {
                    var expanded = ExpandPath(cat.GlobDir);
                    if (Directory.Exists(expanded))
                    {
                        var (size, count) = await ScanGlobAsync(expanded, cat.GlobPattern);
                        totalSize = size;
                        totalCount = count;
                    }
                }
                else
                {
                    foreach (var path in cat.Paths)
                    {
                        var expanded = ExpandPath(path);
                        var (size, count) = await ScanDirectoryAsync(expanded);
                        totalSize += size;
                        totalCount += count;
                    }
                }
            }
            catch { }

            scanned.Size = totalSize;
            scanned.SizeHuman = FormatSize(totalSize);
            scanned.FileCount = totalCount;
            scanned.Found = totalSize > 0;
            results.Add(scanned);
        }

        _scannedCategories = results;
        progress?.Report("Scan complete");
        return results;
    }

    public async Task<CleanResult> CleanAsync(IEnumerable<string> categoryIds, IProgress<string>? progress = null)
    {
        var result = new CleanResult();
        var idList = categoryIds.ToList();

        foreach (var id in idList)
        {
            var cat = _scannedCategories.FirstOrDefault(c => c.Id == id);
            if (cat == null) continue;

            progress?.Report($"Cleaning {cat.Name}...");
            long freed = 0;
            int filesDeleted = 0;

            try
            {
                if (cat.Id == "recycle")
                {
                    await EmptyRecycleBinAsync();
                    freed = cat.Size;
                    filesDeleted = cat.FileCount;
                }
                else if (!string.IsNullOrEmpty(cat.GlobDir) && !string.IsNullOrEmpty(cat.GlobPattern))
                {
                    var expanded = ExpandPath(cat.GlobDir);
                    var (size, count) = await CleanGlobAsync(expanded, cat.GlobPattern);
                    freed = size;
                    filesDeleted = count;
                }
                else
                {
                    foreach (var path in cat.Paths)
                    {
                        var expanded = ExpandPath(path);
                        var (size, count) = await CleanDirectoryAsync(expanded);
                        freed += size;
                        filesDeleted += count;
                    }
                }

                if (freed > 0)
                {
                    result.FreedBytes += freed;
                    result.CleanedCategories.Add(cat.Name);
                    result.FilesDeleted += filesDeleted;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"{cat.Name}: {ex.Message}");
            }
        }

        result.FreedHuman = FormatSize(result.FreedBytes);
        result.CategoriesCleaned = result.CleanedCategories.Count;

        // Save to history
        if (result.FreedBytes > 0)
        {
            var history = await GetHistoryAsync();
            history.Add(new CleaningHistory
            {
                Date = DateTime.Now,
                Freed = result.FreedBytes,
                FreedHuman = result.FreedHuman,
                Count = result.CategoriesCleaned
            });
            
            // Keep only last 20 entries
            if (history.Count > 20)
                history = history.TakeLast(20).ToList();
            
            await SaveHistoryAsync(history);
        }

        progress?.Report($"Cleaned {result.FreedHuman}");
        return result;
    }

    public async Task<List<CleaningHistory>> GetHistoryAsync()
    {
        try
        {
            if (File.Exists(_historyFile))
            {
                var json = await File.ReadAllTextAsync(_historyFile);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CleaningHistory>>(json) ?? new();
            }
        }
        catch { }
        return new List<CleaningHistory>();
    }

    private async Task SaveHistoryAsync(List<CleaningHistory> history)
    {
        try
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(history, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(_historyFile, json);
        }
        catch { }
    }

    private static async Task<(long size, int count)> ScanDirectoryAsync(string path)
    {
        if (!Directory.Exists(path)) return (0, 0);

        long size = 0;
        int count = 0;

        await Task.Run(() =>
        {
            try
            {
                var dirInfo = new DirectoryInfo(path);
                foreach (var file in dirInfo.EnumerateFiles("*", new EnumerationOptions 
                { 
                    IgnoreInaccessible = true, 
                    RecurseSubdirectories = true,
                    MaxRecursionDepth = 5
                }))
                {
                    try
                    {
                        size += file.Length;
                        count++;
                    }
                    catch { }
                }
            }
            catch { }
        });

        return (size, count);
    }

    private static async Task<(long size, int count)> ScanGlobAsync(string directory, string pattern)
    {
        if (!Directory.Exists(directory)) return (0, 0);

        long size = 0;
        int count = 0;

        await Task.Run(() =>
        {
            try
            {
                foreach (var file in Directory.GetFiles(directory, pattern, new EnumerationOptions 
                { 
                    IgnoreInaccessible = true 
                }))
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        size += fi.Length;
                        count++;
                    }
                    catch { }
                }
            }
            catch { }
        });

        return (size, count);
    }

    private static async Task<(long size, int count)> CleanDirectoryAsync(string path)
    {
        if (!Directory.Exists(path)) return (0, 0);

        long freed = 0;
        int count = 0;

        await Task.Run(() =>
        {
            try
            {
                var dirInfo = new DirectoryInfo(path);
                
                // Delete files
                foreach (var file in dirInfo.EnumerateFiles("*", new EnumerationOptions 
                { 
                    IgnoreInaccessible = true, 
                    RecurseSubdirectories = true,
                    MaxRecursionDepth = 5
                }))
                {
                    try
                    {
                        freed += file.Length;
                        file.Delete();
                        count++;
                    }
                    catch { }
                }

                // Delete empty subdirectories
                foreach (var dir in dirInfo.EnumerateDirectories("*", new EnumerationOptions 
                { 
                    IgnoreInaccessible = true, 
                    RecurseSubdirectories = true 
                }))
                {
                    try
                    {
                        if (!dir.EnumerateFileSystemInfos().Any())
                            dir.Delete(false);
                    }
                    catch { }
                }
            }
            catch { }
        });

        return (freed, count);
    }

    private static async Task<(long size, int count)> CleanGlobAsync(string directory, string pattern)
    {
        if (!Directory.Exists(directory)) return (0, 0);

        long freed = 0;
        int count = 0;

        await Task.Run(() =>
        {
            try
            {
                foreach (var file in Directory.GetFiles(directory, pattern, new EnumerationOptions 
                { 
                    IgnoreInaccessible = true 
                }))
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        freed += fi.Length;
                        fi.Delete();
                        count++;
                    }
                    catch { }
                }
            }
            catch { }
        });

        return (freed, count);
    }

    private static async Task<(long size, int count)> GetRecycleBinSizeAsync()
    {
        try
        {
            var script = @"
$shell = New-Object -ComObject Shell.Application
$recycleBin = $shell.NameSpace(0x0a)
$size = 0
$count = 0
if ($recycleBin) {
    $items = $recycleBin.Items()
    $count = $items.Count
    foreach ($item in $items) {
        try { $size += $item.ExtendedProperty('Size') } catch {}
    }
}
""$size|$count""
";
            var output = await RunPowerShellAsync(script);
            var parts = output.Trim().Split('|');
            if (parts.Length == 2)
            {
                long.TryParse(parts[0], out var size);
                int.TryParse(parts[1], out var count);
                return (size, count);
            }
        }
        catch { }
        return (0, 0);
    }

    private static async Task EmptyRecycleBinAsync()
    {
        try
        {
            await RunPowerShellAsync("Clear-RecycleBin -Force -ErrorAction SilentlyContinue");
        }
        catch { }
    }

    private static async Task<string> RunPowerShellAsync(string script)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NonInteractive -NoProfile -ExecutionPolicy Bypass -Command \"{script.Replace("\"", "\\\"")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return output;
    }

    private static string ExpandPath(string path)
    {
        return Environment.ExpandEnvironmentVariables(path)
            .Replace("%TEMP%", Path.GetTempPath().TrimEnd('\\'))
            .Replace("%LOCALAPPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
            .Replace("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
            .Replace("%USERPROFILE%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }

    private static string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
        return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
    }

    private static List<CleanCategory> GetCleanCategories()
    {
        var temp = Environment.GetEnvironmentVariable("TEMP") ?? Path.GetTempPath();
        var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        return new List<CleanCategory>
        {
            // System Caches
            new() { 
                Id = "temp", 
                Name = "Temporary Files", 
                Description = "Windows and app temporary files",
                Paths = new() { temp, @"C:\Windows\Temp" } 
            },
            new() { 
                Id = "prefetch", 
                Name = "Prefetch Cache", 
                Description = "Windows application prefetch files",
                Paths = new() { @"C:\Windows\Prefetch" } 
            },
            new() { 
                Id = "wupdate", 
                Name = "Windows Update Cache", 
                Description = "Downloaded Windows Update files",
                Paths = new() { @"C:\Windows\SoftwareDistribution\Download" } 
            },
            new() { 
                Id = "thumbnails", 
                Name = "Thumbnail Cache", 
                Description = "Explorer thumbnail database files",
                GlobDir = Path.Combine(local, "Microsoft", "Windows", "Explorer"),
                GlobPattern = "thumbcache_*.db" 
            },
            new() { 
                Id = "logs", 
                Name = "System Log Files", 
                Description = "Windows CBS and DISM log files",
                Paths = new() { @"C:\Windows\Logs\CBS", @"C:\Windows\Logs\DISM" } 
            },
            
            // Browser Caches
            new() { 
                Id = "chrome", 
                Name = "Chrome Cache", 
                Description = "Google Chrome browser cache",
                Paths = new() { Path.Combine(local, "Google", "Chrome", "User Data", "Default", "Cache", "Cache_Data") } 
            },
            new() { 
                Id = "edge", 
                Name = "Edge Cache", 
                Description = "Microsoft Edge browser cache",
                Paths = new() { Path.Combine(local, "Microsoft", "Edge", "User Data", "Default", "Cache", "Cache_Data") } 
            },
            new() { 
                Id = "firefox", 
                Name = "Firefox Cache", 
                Description = "Mozilla Firefox browser cache",
                Paths = new() { Path.Combine(local, "Mozilla", "Firefox", "Profiles") }
            },
            
            // Application Caches
            new() { 
                Id = "discord", 
                Name = "Discord Cache", 
                Description = "Discord application cache",
                Paths = new() { 
                    Path.Combine(roaming, "discord", "Cache", "Cache_Data"),
                    Path.Combine(roaming, "discord", "Code Cache") 
                } 
            },
            new() { 
                Id = "spotify", 
                Name = "Spotify Cache", 
                Description = "Spotify audio and data cache",
                Paths = new() { 
                    Path.Combine(local, "Spotify", "Storage"),
                    Path.Combine(local, "Spotify", "Data") 
                } 
            },
            new() { 
                Id = "steam", 
                Name = "Steam Download Cache", 
                Description = "Steam download cache (not games)",
                Paths = new() { 
                    Path.Combine(local, "Steam", "htmlcache"),
                    @"C:\Program Files (x86)\Steam\appcache\httpcache"
                } 
            },
            
            // GPU Caches
            new() { 
                Id = "nvidia", 
                Name = "NVIDIA Shader Cache", 
                Description = "NVIDIA DirectX shader cache",
                Paths = new() { 
                    Path.Combine(local, "NVIDIA", "DXCache"),
                    Path.Combine(local, "NVIDIA", "GLCache")
                } 
            },
            new() { 
                Id = "amd", 
                Name = "AMD Shader Cache", 
                Description = "AMD DirectX shader cache",
                Paths = new() { Path.Combine(local, "AMD", "DXCache") } 
            },
            new() { 
                Id = "dx", 
                Name = "DirectX Shader Cache", 
                Description = "Windows DirectX shader cache",
                Paths = new() { Path.Combine(local, "D3DSCache") } 
            },
            
            // Recycle Bin
            new() { 
                Id = "recycle", 
                Name = "Recycle Bin", 
                Description = "Deleted files in Recycle Bin",
                Paths = new() 
            },
        };
    }
}
