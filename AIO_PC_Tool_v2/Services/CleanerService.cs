using System.IO;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Services
{
    public class CleanerService
    {
        private List<CleanerCategory> GetAllPossibleCategories()
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            return new List<CleanerCategory>
            {
                // System Cache
                new CleanerCategory
                {
                    Name = "Windows Temp Files",
                    Description = "System and user temporary files",
                    Icon = "🗂",
                    Category = "System",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.GetTempPath(),
                        @"C:\Windows\Temp",
                        Path.Combine(localAppData, "Temp")
                    }
                },
                new CleanerCategory
                {
                    Name = "Windows Update Cache",
                    Description = "Downloaded Windows update files",
                    Icon = "🔄",
                    Category = "System",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        @"C:\Windows\SoftwareDistribution\Download",
                        @"C:\Windows\SoftwareDistribution\DataStore\Logs"
                    }
                },
                new CleanerCategory
                {
                    Name = "Windows Logs",
                    Description = "CBS, DISM, Windows Error Reports",
                    Icon = "📋",
                    Category = "System",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        @"C:\Windows\Logs\CBS",
                        @"C:\Windows\Logs\DISM",
                        Path.Combine(localAppData, @"Microsoft\Windows\WER"),
                        @"C:\ProgramData\Microsoft\Windows\WER"
                    }
                },
                new CleanerCategory
                {
                    Name = "Prefetch Data",
                    Description = "Application prefetch cache",
                    Icon = "⚡",
                    Category = "System",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        @"C:\Windows\Prefetch"
                    }
                },
                new CleanerCategory
                {
                    Name = "Thumbnail Cache",
                    Description = "Explorer thumbnail database",
                    Icon = "🖼",
                    Category = "System",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Microsoft\Windows\Explorer")
                    },
                    FilePatterns = new[] { "thumbcache_*.db", "iconcache_*.db" }
                },

                // DirectX & Graphics
                new CleanerCategory
                {
                    Name = "DirectX Shader Cache",
                    Description = "Compiled DirectX shader files",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"D3DSCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "AMD GPU Cache",
                    Description = "AMD driver and shader cache",
                    Icon = "🔴",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"AMD\DxCache"),
                        Path.Combine(localAppData, @"AMD\DxcCache"),
                        Path.Combine(localAppData, @"AMD\GLCache"),
                        Path.Combine(localAppData, @"AMD\VkCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "NVIDIA GPU Cache",
                    Description = "NVIDIA driver and shader cache",
                    Icon = "🟢",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"NVIDIA\DXCache"),
                        Path.Combine(localAppData, @"NVIDIA\GLCache"),
                        Path.Combine(programData, @"NVIDIA Corporation\NV_Cache"),
                        Path.Combine(appData, @"NVIDIA\ComputeCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Intel GPU Cache",
                    Description = "Intel graphics shader cache",
                    Icon = "🔵",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Intel\ShaderCache")
                    }
                },

                // Browsers
                new CleanerCategory
                {
                    Name = "Google Chrome",
                    Description = "Chrome browser cache",
                    Icon = "🌐",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Code Cache"),
                        Path.Combine(localAppData, @"Google\Chrome\User Data\Default\GPUCache"),
                        Path.Combine(localAppData, @"Google\Chrome\User Data\ShaderCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Microsoft Edge",
                    Description = "Edge browser cache",
                    Icon = "🌐",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Microsoft\Edge\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"Microsoft\Edge\User Data\Default\Code Cache"),
                        Path.Combine(localAppData, @"Microsoft\Edge\User Data\Default\GPUCache"),
                        Path.Combine(localAppData, @"Microsoft\Edge\User Data\ShaderCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Mozilla Firefox",
                    Description = "Firefox browser cache",
                    Icon = "🦊",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Mozilla\Firefox\Profiles")
                    },
                    FilePatterns = new[] { "cache2" },
                    IsRecursivePatternSearch = true
                },
                new CleanerCategory
                {
                    Name = "Opera GX",
                    Description = "Opera GX gaming browser cache",
                    Icon = "🎮",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(appData, @"Opera Software\Opera GX Stable\Cache"),
                        Path.Combine(appData, @"Opera Software\Opera GX Stable\GPUCache"),
                        Path.Combine(appData, @"Opera Software\Opera GX Stable\ShaderCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Brave Browser",
                    Description = "Brave browser cache",
                    Icon = "🦁",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Code Cache"),
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\ShaderCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Vivaldi Browser",
                    Description = "Vivaldi browser cache",
                    Icon = "🌐",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Vivaldi\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"Vivaldi\User Data\Default\Code Cache"),
                        Path.Combine(localAppData, @"Vivaldi\User Data\ShaderCache")
                    }
                },

                // Gaming Platforms
                new CleanerCategory
                {
                    Name = "Steam",
                    Description = "Steam client cache and logs",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(programData, @"Steam\htmlcache"),
                        Path.Combine(localAppData, @"Steam\htmlcache"),
                        @"C:\Program Files (x86)\Steam\logs",
                        @"C:\Program Files\Steam\logs"
                    }
                },
                new CleanerCategory
                {
                    Name = "Epic Games",
                    Description = "Epic Games Launcher cache",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"EpicGamesLauncher\Saved\webcache"),
                        Path.Combine(localAppData, @"EpicGamesLauncher\Saved\Logs")
                    }
                },
                new CleanerCategory
                {
                    Name = "EA App",
                    Description = "EA App cache files",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Electronic Arts\EA Desktop\cache"),
                        Path.Combine(programData, @"Electronic Arts\EA Desktop\Logs")
                    }
                },
                new CleanerCategory
                {
                    Name = "Ubisoft Connect",
                    Description = "Ubisoft Connect cache",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Ubisoft Game Launcher\cache"),
                        Path.Combine(localAppData, @"Ubisoft Game Launcher\logs")
                    }
                },
                new CleanerCategory
                {
                    Name = "GOG Galaxy",
                    Description = "GOG Galaxy client cache",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(programData, @"GOG.com\Galaxy\webcache"),
                        Path.Combine(localAppData, @"GOG.com\Galaxy\logs")
                    }
                },
                new CleanerCategory
                {
                    Name = "Battle.net",
                    Description = "Blizzard Battle.net cache",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(programData, @"Blizzard Entertainment\Battle.net\Cache"),
                        Path.Combine(localAppData, @"Blizzard Entertainment\Battle.net\Cache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Riot Games",
                    Description = "Riot Client cache (LoL, Valorant)",
                    Icon = "🎮",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Riot Games\Riot Client\Cache"),
                        Path.Combine(localAppData, @"Riot Games\Riot Client\Logs")
                    }
                },

                // Applications
                new CleanerCategory
                {
                    Name = "Discord",
                    Description = "Discord app cache",
                    Icon = "💬",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(appData, @"discord\Cache"),
                        Path.Combine(appData, @"discord\Code Cache"),
                        Path.Combine(appData, @"discord\GPUCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Spotify",
                    Description = "Spotify music cache",
                    Icon = "🎵",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Spotify\Storage")
                    }
                },
                new CleanerCategory
                {
                    Name = "VS Code",
                    Description = "Visual Studio Code cache",
                    Icon = "💻",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(appData, @"Code\Cache"),
                        Path.Combine(appData, @"Code\CachedData"),
                        Path.Combine(appData, @"Code\CachedExtensions"),
                        Path.Combine(appData, @"Code\GPUCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Slack",
                    Description = "Slack app cache",
                    Icon = "💼",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(appData, @"Slack\Cache"),
                        Path.Combine(appData, @"Slack\Code Cache"),
                        Path.Combine(appData, @"Slack\GPUCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Microsoft Teams",
                    Description = "Teams app cache",
                    Icon = "💼",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(appData, @"Microsoft\Teams\Cache"),
                        Path.Combine(appData, @"Microsoft\Teams\GPUCache"),
                        Path.Combine(appData, @"Microsoft\Teams\blob_storage")
                    }
                },
                new CleanerCategory
                {
                    Name = "Zoom",
                    Description = "Zoom app cache",
                    Icon = "📹",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(appData, @"Zoom\data"),
                        Path.Combine(appData, @"Zoom\bin\cache")
                    }
                },

                // Recycle Bin
                new CleanerCategory
                {
                    Name = "Recycle Bin",
                    Description = "Deleted files awaiting permanent removal",
                    Icon = "🗑",
                    Category = "System",
                    IsSafe = true,
                    IsRecycleBin = true,
                    Paths = Array.Empty<string>()
                }
            };
        }

        /// <summary>
        /// Gets only categories that exist on the system AND have files to clean
        /// </summary>
        public async Task<ObservableCollection<CleanerCategory>> GetCategoriesWithDataAsync()
        {
            var allCategories = GetAllPossibleCategories();
            var validCategories = new ObservableCollection<CleanerCategory>();

            await Task.Run(() =>
            {
                foreach (var category in allCategories)
                {
                    if (CategoryHasData(category))
                    {
                        validCategories.Add(category);
                    }
                }
            });

            return validCategories;
        }

        /// <summary>
        /// Quick check if a category has any files (doesn't count them all, just checks existence)
        /// </summary>
        private bool CategoryHasData(CleanerCategory category)
        {
            if (category.IsRecycleBin)
            {
                return RecycleBinHasFiles();
            }

            foreach (var path in category.Paths)
            {
                if (Directory.Exists(path))
                {
                    try
                    {
                        if (category.FilePatterns != null && category.FilePatterns.Length > 0)
                        {
                            foreach (var pattern in category.FilePatterns)
                            {
                                var searchOption = category.IsRecursivePatternSearch 
                                    ? SearchOption.AllDirectories 
                                    : SearchOption.TopDirectoryOnly;
                                    
                                if (Directory.EnumerateFiles(path, pattern, searchOption).Any())
                                {
                                    return true;
                                }
                                
                                // Also check for directories matching pattern
                                if (Directory.EnumerateDirectories(path, pattern, searchOption).Any())
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            // Check if there are any files
                            if (Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any())
                            {
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        // Access denied or other error - skip
                    }
                }
            }

            return false;
        }

        private bool RecycleBinHasFiles()
        {
            try
            {
                var recyclePath = @"C:\$Recycle.Bin";
                if (Directory.Exists(recyclePath))
                {
                    foreach (var dir in Directory.GetDirectories(recyclePath))
                    {
                        try
                        {
                            if (Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories).Any())
                            {
                                return true;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
            return false;
        }

        public async Task<(long size, int count)> ScanCategoryAsync(CleanerCategory category)
        {
            return await Task.Run(() =>
            {
                long totalSize = 0;
                int fileCount = 0;

                if (category.IsRecycleBin)
                {
                    try
                    {
                        var recyclePath = @"C:\$Recycle.Bin";
                        if (Directory.Exists(recyclePath))
                        {
                            foreach (var dir in Directory.GetDirectories(recyclePath))
                            {
                                try
                                {
                                    var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                                    foreach (var file in files)
                                    {
                                        try
                                        {
                                            var info = new FileInfo(file);
                                            totalSize += info.Length;
                                            fileCount++;
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                else
                {
                    foreach (var path in category.Paths)
                    {
                        if (Directory.Exists(path))
                        {
                            try
                            {
                                IEnumerable<string> files;
                                
                                if (category.FilePatterns != null && category.FilePatterns.Length > 0)
                                {
                                    var fileList = new List<string>();
                                    var searchOption = category.IsRecursivePatternSearch 
                                        ? SearchOption.AllDirectories 
                                        : SearchOption.TopDirectoryOnly;
                                        
                                    foreach (var pattern in category.FilePatterns)
                                    {
                                        try
                                        {
                                            // Get files matching pattern
                                            fileList.AddRange(Directory.GetFiles(path, pattern, searchOption));
                                            
                                            // Also get files inside directories matching pattern
                                            foreach (var dir in Directory.GetDirectories(path, pattern, searchOption))
                                            {
                                                try
                                                {
                                                    fileList.AddRange(Directory.GetFiles(dir, "*", SearchOption.AllDirectories));
                                                }
                                                catch { }
                                            }
                                        }
                                        catch { }
                                    }
                                    files = fileList;
                                }
                                else
                                {
                                    files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                                }

                                foreach (var file in files)
                                {
                                    try
                                    {
                                        var info = new FileInfo(file);
                                        totalSize += info.Length;
                                        fileCount++;
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                        }
                    }
                }

                category.Size = totalSize;
                category.FileCount = fileCount;
                return (totalSize, fileCount);
            });
        }

        public async Task<long> CleanCategoryAsync(CleanerCategory category)
        {
            return await Task.Run(() =>
            {
                long cleaned = 0;

                if (category.IsRecycleBin)
                {
                    try
                    {
                        // Use SHEmptyRecycleBin for proper recycle bin emptying
                        cleaned = category.Size;
                        var psi = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/c PowerShell.exe -NoProfile -Command \"Clear-RecycleBin -Force -ErrorAction SilentlyContinue\"",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };
                        var proc = System.Diagnostics.Process.Start(psi);
                        proc?.WaitForExit(10000);
                    }
                    catch { }
                }
                else
                {
                    foreach (var path in category.Paths)
                    {
                        if (Directory.Exists(path))
                        {
                            try
                            {
                                IEnumerable<string> files;
                                
                                if (category.FilePatterns != null && category.FilePatterns.Length > 0)
                                {
                                    var fileList = new List<string>();
                                    var searchOption = category.IsRecursivePatternSearch 
                                        ? SearchOption.AllDirectories 
                                        : SearchOption.TopDirectoryOnly;
                                        
                                    foreach (var pattern in category.FilePatterns)
                                    {
                                        try
                                        {
                                            fileList.AddRange(Directory.GetFiles(path, pattern, searchOption));
                                            
                                            foreach (var dir in Directory.GetDirectories(path, pattern, searchOption))
                                            {
                                                try
                                                {
                                                    fileList.AddRange(Directory.GetFiles(dir, "*", SearchOption.AllDirectories));
                                                }
                                                catch { }
                                            }
                                        }
                                        catch { }
                                    }
                                    files = fileList;
                                }
                                else
                                {
                                    files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                                }

                                foreach (var file in files)
                                {
                                    try
                                    {
                                        var info = new FileInfo(file);
                                        cleaned += info.Length;
                                        File.Delete(file);
                                    }
                                    catch { }
                                }

                                // Try to remove empty directories
                                try
                                {
                                    foreach (var dir in Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Reverse())
                                    {
                                        try
                                        {
                                            if (!Directory.EnumerateFileSystemEntries(dir).Any())
                                            {
                                                Directory.Delete(dir);
                                            }
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                            catch { }
                        }
                    }
                }

                return cleaned;
            });
        }

        public async Task<(long totalSize, int totalFiles, int categoryCount)> ScanAllAsync(ObservableCollection<CleanerCategory> categories)
        {
            long totalSize = 0;
            int totalFiles = 0;

            foreach (var cat in categories)
            {
                var (size, count) = await ScanCategoryAsync(cat);
                totalSize += size;
                totalFiles += count;
            }

            return (totalSize, totalFiles, categories.Count);
        }

        public async Task<long> CleanSelectedAsync(ObservableCollection<CleanerCategory> categories)
        {
            long totalCleaned = 0;

            foreach (var category in categories.Where(c => c.IsSelected && c.Size > 0))
            {
                totalCleaned += await CleanCategoryAsync(category);
            }

            return totalCleaned;
        }
    }
}
