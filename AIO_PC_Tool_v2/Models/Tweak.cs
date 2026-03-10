using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Models
{
    public partial class Tweak : ObservableObject
    {
        [ObservableProperty]
        private string id = string.Empty;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string category = string.Empty;

        [ObservableProperty]
        private bool isActive;

        [ObservableProperty]
        private string? warning;

        [ObservableProperty]
        private string windowsVersion = "10/11";

        public string RegistryPath { get; set; } = string.Empty;
        public string RegistryName { get; set; } = string.Empty;
        public object? OptimizedValue { get; set; }
        public object? DefaultValue { get; set; }
    }

    public class CleanerCategory : ObservableObject
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long Size { get; set; }
        public int FileCount { get; set; }
        public string SizeHuman => FormatSize(Size);
        public string[] Paths { get; set; } = Array.Empty<string>();

        private static string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }

    public class CleanHistory
    {
        public DateTime Date { get; set; }
        public long FreedBytes { get; set; }
        public string FreedHuman => FormatSize(FreedBytes);

        private static string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }

    public class DnsProvider
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PrimaryDns { get; set; } = string.Empty;
        public string SecondaryDns { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class SystemInfo
    {
        public CpuInfo Cpu { get; set; } = new();
        public RamInfo Ram { get; set; } = new();
        public GpuInfo Gpu { get; set; } = new();
        public DiskInfo Disk { get; set; } = new();
    }

    public class CpuInfo
    {
        public string Model { get; set; } = "Unknown";
        public double Usage { get; set; }
        public double Temperature { get; set; }
        public int Cores { get; set; }
        public int Threads { get; set; }
    }

    public class RamInfo
    {
        public double UsedGb { get; set; }
        public double TotalGb { get; set; }
        public double UsagePercent => TotalGb > 0 ? (UsedGb / TotalGb) * 100 : 0;
    }

    public class GpuInfo
    {
        public string Model { get; set; } = "Unknown";
        public double Usage { get; set; }
        public double Temperature { get; set; }
        public double MemoryUsedGb { get; set; }
        public double MemoryTotalGb { get; set; }
    }

    public class DiskInfo
    {
        public double UsedGb { get; set; }
        public double TotalGb { get; set; }
        public double UsagePercent => TotalGb > 0 ? (UsedGb / TotalGb) * 100 : 0;
    }

    public class RestorePoint
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public int SequenceNumber { get; set; }
    }

    public class UtilityItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
