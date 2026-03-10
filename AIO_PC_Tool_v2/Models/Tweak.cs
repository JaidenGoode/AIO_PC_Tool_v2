namespace AIO_PC_Tool_v2.Models;

public class Tweak
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Warning { get; set; }
    public string? FeatureBreaks { get; set; }
    public string EnableScript { get; set; } = string.Empty;
    public string DisableScript { get; set; } = string.Empty;
    public string DetectScript { get; set; } = string.Empty;
}

public class SystemInfo
{
    public CpuInfo Cpu { get; set; } = new();
    public GpuInfo Gpu { get; set; } = new();
    public MemoryInfo Memory { get; set; } = new();
    public StorageInfo Storage { get; set; } = new();
    public OsInfo Os { get; set; } = new();
}

public class CpuInfo
{
    public string Model { get; set; } = "Unknown";
    public int Cores { get; set; }
    public int Threads { get; set; }
    public double Speed { get; set; }
    public double Usage { get; set; }
    public double Temperature { get; set; }
}

public class GpuInfo
{
    public string Model { get; set; } = "Unknown";
    public string Vram { get; set; } = "Unknown";
    public double Usage { get; set; }
    public double Temperature { get; set; }
}

public class MemoryInfo
{
    public string Total { get; set; } = "0 GB";
    public string Used { get; set; } = "0 GB";
    public string Available { get; set; } = "0 GB";
    public string Type { get; set; } = "Unknown";
    public double UsagePercent { get; set; }
}

public class StorageInfo
{
    public string PrimaryDisk { get; set; } = "Unknown";
    public string TotalSpace { get; set; } = "0 GB";
    public string FreeSpace { get; set; } = "0 GB";
    public double UsagePercent { get; set; }
    public double ReadSpeed { get; set; }
    public double WriteSpeed { get; set; }
}

public class OsInfo
{
    public string Name { get; set; } = "Windows";
    public string Version { get; set; } = "Unknown";
    public string Build { get; set; } = "Unknown";
}

public class CleanCategory
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Paths { get; set; } = new();
    public string? GlobDir { get; set; }
    public string? GlobPattern { get; set; }
    public long Size { get; set; }
    public string SizeHuman { get; set; } = "0 B";
    public int FileCount { get; set; }
    public bool IsSelected { get; set; }
    public bool Found { get; set; }
}

public class CleaningHistory
{
    public DateTime Date { get; set; }
    public long Freed { get; set; }
    public string FreedHuman { get; set; } = "0 B";
    public int Count { get; set; }
}

public class DnsProvider
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PrimaryDns { get; set; } = string.Empty;
    public string SecondaryDns { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class RestorePoint
{
    public string Description { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public int SequenceNumber { get; set; }
}
