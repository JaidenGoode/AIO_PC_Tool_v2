using System.IO;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Services
{
    public class CleanerService
    {
        public ObservableCollection<CleanerCategory> GetCategories()
        {
            return new ObservableCollection<CleanerCategory>
            {
                new CleanerCategory
                {
                    Name = "Windows Temp Files",
                    Description = "Temporary files created by Windows",
                    Paths = new[] { Path.GetTempPath(), @"C:\Windows\Temp" }
                },
                new CleanerCategory
                {
                    Name = "Browser Cache",
                    Description = "Cached files from web browsers",
                    Paths = new[]
                    {
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Cache"),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Edge\User Data\Default\Cache")
                    }
                },
                new CleanerCategory
                {
                    Name = "Windows Update Cache",
                    Description = "Downloaded Windows update files",
                    Paths = new[] { @"C:\Windows\SoftwareDistribution\Download" }
                },
                new CleanerCategory
                {
                    Name = "Recycle Bin",
                    Description = "Deleted files in the Recycle Bin",
                    Paths = new[] { @"C:\$Recycle.Bin" }
                },
                new CleanerCategory
                {
                    Name = "Thumbnail Cache",
                    Description = "Cached thumbnails for images and videos",
                    Paths = new[] { Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Explorer") }
                }
            };
        }

        public async Task<(long size, int count)> ScanCategoryAsync(CleanerCategory category)
        {
            return await Task.Run(() =>
            {
                long totalSize = 0;
                int fileCount = 0;

                foreach (var path in category.Paths)
                {
                    if (Directory.Exists(path))
                    {
                        try
                        {
                            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
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

                foreach (var path in category.Paths)
                {
                    if (Directory.Exists(path))
                    {
                        try
                        {
                            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
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
                        }
                        catch { }
                    }
                }

                return cleaned;
            });
        }
    }
}
