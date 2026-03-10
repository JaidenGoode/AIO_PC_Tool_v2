using System.IO;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Services
{
    public class CleanerService
    {
        public ObservableCollection<CleanerCategory> GetCategories()
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            return new ObservableCollection<CleanerCategory>
            {
                // System Cache
                new CleanerCategory
                {
                    Name = "Windows Temp Files",
                    Description = "System and user temporary files",
                    Icon = "Temp",
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
                    Icon = "Update",
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
                    Icon = "Log",
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
                    Icon = "Prefetch",
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
                    Icon = "Thumbnail",
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
                    Description = "Compiled shader cache files",
                    Icon = "DirectX",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"D3DSCache"),
                        Path.Combine(localAppData, @"AMD\DxCache"),
                        Path.Combine(localAppData, @"AMD\DxcCache"),
                        Path.Combine(localAppData, @"AMD\GLCache"),
                        Path.Combine(localAppData, @"AMD\VkCache")
                    }
                },
                new CleanerCategory
                {
                    Name = "NVIDIA Cache",
                    Description = "NVIDIA driver and shader cache",
                    Icon = "NVIDIA",
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
                    Name = "Intel Graphics Cache",
                    Description = "Intel GPU shader cache",
                    Icon = "Intel",
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
                    Name = "Chrome Cache",
                    Description = "Google Chrome browser cache",
                    Icon = "Chrome",
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
                    Name = "Edge Cache",
                    Description = "Microsoft Edge browser cache",
                    Icon = "Edge",
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
                    Name = "Firefox Cache",
                    Description = "Mozilla Firefox browser cache",
                    Icon = "Firefox",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Mozilla\Firefox\Profiles")
                    },
                    FilePatterns = new[] { "cache2" }
                },
                new CleanerCategory
                {
                    Name = "Opera GX Cache",
                    Description = "Opera GX gaming browser cache",
                    Icon = "Opera",
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
                    Name = "Brave Cache",
                    Description = "Brave browser cache",
                    Icon = "Brave",
                    Category = "Browser",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Code Cache"),
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\ShaderCache")
                    }
                },

                // Gaming Platforms
                new CleanerCategory
                {
                    Name = "Steam Cache",
                    Description = "Steam client cache and logs",
                    Icon = "Steam",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(programData, @"Steam\htmlcache"),
                        Path.Combine(localAppData, @"Steam\htmlcache"),
                        @"C:\Program Files (x86)\Steam\logs"
                    }
                },
                new CleanerCategory
                {
                    Name = "Epic Games Cache",
                    Description = "Epic Games Launcher cache",
                    Icon = "Epic",
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
                    Name = "EA App Cache",
                    Description = "EA App cache files",
                    Icon = "EA",
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
                    Name = "Ubisoft Cache",
                    Description = "Ubisoft Connect cache",
                    Icon = "Ubisoft",
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
                    Name = "GOG Galaxy Cache",
                    Description = "GOG Galaxy client cache",
                    Icon = "GOG",
                    Category = "Gaming",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(programData, @"GOG.com\Galaxy\webcache"),
                        Path.Combine(localAppData, @"GOG.com\Galaxy\logs")
                    }
                },

                // Applications
                new CleanerCategory
                {
                    Name = "Discord Cache",
                    Description = "Discord app cache",
                    Icon = "Discord",
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
                    Name = "Spotify Cache",
                    Description = "Spotify music cache",
                    Icon = "Spotify",
                    Category = "Apps",
                    IsSafe = true,
                    Paths = new[] 
                    { 
                        Path.Combine(localAppData, @"Spotify\Storage")
                    }
                },
                new CleanerCategory
                {
                    Name = "VS Code Cache",
                    Description = "Visual Studio Code cache",
                    Icon = "VSCode",
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

                // Recycle Bin
                new CleanerCategory
                {
                    Name = "Recycle Bin",
                    Description = "Deleted files awaiting permanent removal",
                    Icon = "RecycleBin",
                    Category = "System",
                    IsSafe = true,
                    IsRecycleBin = true,
                    Paths = Array.Empty<string>()
                }
            };
        }

        public async Task<(long size, int count)> ScanCategoryAsync(CleanerCategory category)
        {
            return await Task.Run(() =>
            {
                long totalSize = 0;
                int fileCount = 0;

                if (category.IsRecycleBin)
                {
                    // Handle Recycle Bin specially
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
                                string[] files;
                                if (category.FilePatterns != null && category.FilePatterns.Length > 0)
                                {
                                    var fileList = new List<string>();
                                    foreach (var pattern in category.FilePatterns)
                                    {
                                        try
                                        {
                                            fileList.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
                                        }
                                        catch { }
                                    }
                                    files = fileList.ToArray();
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
                    // Empty Recycle Bin using Shell
                    try
                    {
                        var psi = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/c rd /s /q C:\\$Recycle.Bin",
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            Verb = "runas"
                        };
                        System.Diagnostics.Process.Start(psi)?.WaitForExit(5000);
                        cleaned = category.Size;
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
                                string[] files;
                                if (category.FilePatterns != null && category.FilePatterns.Length > 0)
                                {
                                    var fileList = new List<string>();
                                    foreach (var pattern in category.FilePatterns)
                                    {
                                        try
                                        {
                                            fileList.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
                                        }
                                        catch { }
                                    }
                                    files = fileList.ToArray();
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

        public async Task<(long totalSize, int totalFiles)> ScanAllAsync(ObservableCollection<CleanerCategory> categories)
        {
            long totalSize = 0;
            int totalFiles = 0;

            var tasks = categories.Select(async cat =>
            {
                var (size, count) = await ScanCategoryAsync(cat);
                return (size, count);
            });

            var results = await Task.WhenAll(tasks);
            
            foreach (var (size, count) in results)
            {
                totalSize += size;
                totalFiles += count;
            }

            return (totalSize, totalFiles);
        }

        public async Task<long> CleanSelectedAsync(ObservableCollection<CleanerCategory> categories)
        {
            long totalCleaned = 0;

            foreach (var category in categories.Where(c => c.IsSelected))
            {
                totalCleaned += await CleanCategoryAsync(category);
            }

            return totalCleaned;
        }
    }
}
