using System.IO;
using AIO_PC_Tool_v2.Models;
using LibreHardwareMonitor.Hardware;

namespace AIO_PC_Tool_v2.Services
{
    public class HardwareMonitorService : IDisposable
    {
        private readonly Computer _computer;

        public HardwareMonitorService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsStorageEnabled = true
            };
            _computer.Open();
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
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                                info.Cpu.Usage = sensor.Value ?? 0;
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Package"))
                                info.Cpu.Temperature = sensor.Value ?? 0;
                        }
                        break;

                    case HardwareType.Memory:
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Data && sensor.Name == "Memory Used")
                                info.Ram.UsedGb = sensor.Value ?? 0;
                            if (sensor.SensorType == SensorType.Data && sensor.Name == "Memory Available")
                                info.Ram.TotalGb = (sensor.Value ?? 0) + info.Ram.UsedGb;
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
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
                                info.Gpu.Temperature = sensor.Value ?? 0;
                        }
                        break;

                    case HardwareType.Storage:
                        // UsagePercent is calculated from UsedGb/TotalGb
                        break;
                }
            }

            // Get disk info from DriveInfo
            try
            {
                var drive = new DriveInfo("C");
                info.Disk.TotalGb = drive.TotalSize / (1024.0 * 1024 * 1024);
                info.Disk.UsedGb = (drive.TotalSize - drive.AvailableFreeSpace) / (1024.0 * 1024 * 1024);
            }
            catch { }

            return info;
        }

        public void Dispose()
        {
            _computer.Close();
        }
    }
}
