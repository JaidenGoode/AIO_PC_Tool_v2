using LibreHardwareMonitor.Hardware;
using AIO_PC_Tool_v2.Models;
using System.Management;

namespace AIO_PC_Tool_v2.Services;

public interface IHardwareMonitorService : IDisposable
{
    SystemInfo GetSystemInfo();
    Task<SystemInfo> GetLiveUsageAsync();
    void StartMonitoring();
    void StopMonitoring();
    event EventHandler<SystemInfo>? UsageUpdated;
}

public class HardwareMonitorService : IHardwareMonitorService
{
    private readonly Computer _computer;
    private System.Threading.Timer? _timer;
    private bool _isMonitoring;
    
    public event EventHandler<SystemInfo>? UsageUpdated;

    public HardwareMonitorService()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsStorageEnabled = true,
            IsMotherboardEnabled = true
        };
        _computer.Open();
    }

    public void StartMonitoring()
    {
        if (_isMonitoring) return;
        _isMonitoring = true;
        _timer = new System.Threading.Timer(async _ =>
        {
            var info = await GetLiveUsageAsync();
            UsageUpdated?.Invoke(this, info);
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    public void StopMonitoring()
    {
        _isMonitoring = false;
        _timer?.Dispose();
        _timer = null;
    }

    public SystemInfo GetSystemInfo()
    {
        var info = new SystemInfo();
        
        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();
            
            switch (hardware.HardwareType)
            {
                case HardwareType.Cpu:
                    info.Cpu.Model = hardware.Name;
                    info.Cpu.Cores = Environment.ProcessorCount;
                    break;
                    
                case HardwareType.GpuNvidia:
                case HardwareType.GpuAmd:
                case HardwareType.GpuIntel:
                    info.Gpu.Model = hardware.Name;
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.SmallData && sensor.Name.Contains("Memory Total"))
                        {
                            info.Gpu.Vram = $"{sensor.Value:F0} MB";
                        }
                    }
                    break;
                    
                case HardwareType.Memory:
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Name == "Memory Used")
                                info.Memory.Used = $"{sensor.Value:F1} GB";
                            else if (sensor.Name == "Memory Available")
                                info.Memory.Available = $"{sensor.Value:F1} GB";
                        }
                    }
                    break;
                    
                case HardwareType.Storage:
                    if (string.IsNullOrEmpty(info.Storage.PrimaryDisk) || info.Storage.PrimaryDisk == "Unknown")
                    {
                        info.Storage.PrimaryDisk = hardware.Name;
                    }
                    break;
            }
        }
        
        // Get OS info
        info.Os.Name = "Windows";
        info.Os.Version = Environment.OSVersion.Version.ToString();
        
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Caption, BuildNumber FROM Win32_OperatingSystem");
            foreach (ManagementObject mo in searcher.Get())
            {
                info.Os.Name = mo["Caption"]?.ToString() ?? "Windows";
                info.Os.Build = mo["BuildNumber"]?.ToString() ?? "Unknown";
            }
        }
        catch { }
        
        // Calculate total memory
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            foreach (ManagementObject mo in searcher.Get())
            {
                var totalBytes = Convert.ToUInt64(mo["TotalPhysicalMemory"]);
                info.Memory.Total = $"{totalBytes / (1024.0 * 1024 * 1024):F1} GB";
            }
        }
        catch { }
        
        return info;
    }

    public Task<SystemInfo> GetLiveUsageAsync()
    {
        return Task.Run(() =>
        {
            var info = new SystemInfo();
            
            foreach (var hardware in _computer.Hardware)
            {
                hardware.Update();
                
                switch (hardware.HardwareType)
                {
                    case HardwareType.Cpu:
                        info.Cpu.Model = hardware.Name;
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                                info.Cpu.Usage = sensor.Value ?? 0;
                            else if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Package"))
                                info.Cpu.Temperature = sensor.Value ?? 0;
                            else if (sensor.SensorType == SensorType.Clock && sensor.Name.Contains("Core #1"))
                                info.Cpu.Speed = (sensor.Value ?? 0) / 1000.0;
                        }
                        break;
                        
                    case HardwareType.GpuNvidia:
                    case HardwareType.GpuAmd:
                    case HardwareType.GpuIntel:
                        info.Gpu.Model = hardware.Name;
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                                info.Gpu.Usage = sensor.Value ?? 0;
                            else if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
                                info.Gpu.Temperature = sensor.Value ?? 0;
                        }
                        break;
                        
                    case HardwareType.Memory:
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "Memory")
                                info.Memory.UsagePercent = sensor.Value ?? 0;
                            else if (sensor.SensorType == SensorType.Data)
                            {
                                if (sensor.Name == "Memory Used")
                                    info.Memory.Used = $"{sensor.Value:F1} GB";
                                else if (sensor.Name == "Memory Available")
                                    info.Memory.Available = $"{sensor.Value:F1} GB";
                            }
                        }
                        break;
                        
                    case HardwareType.Storage:
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "Used Space")
                                info.Storage.UsagePercent = sensor.Value ?? 0;
                            else if (sensor.SensorType == SensorType.Throughput)
                            {
                                if (sensor.Name == "Read Rate")
                                    info.Storage.ReadSpeed = (sensor.Value ?? 0) / (1024 * 1024);
                                else if (sensor.Name == "Write Rate")
                                    info.Storage.WriteSpeed = (sensor.Value ?? 0) / (1024 * 1024);
                            }
                        }
                        break;
                }
            }
            
            return info;
        });
    }

    public void Dispose()
    {
        StopMonitoring();
        _computer.Close();
    }
}
