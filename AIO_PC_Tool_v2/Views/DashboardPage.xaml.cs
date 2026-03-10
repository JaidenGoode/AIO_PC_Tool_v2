using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class DashboardPage : Page
    {
        private HardwareMonitorService? _hardwareService;
        private DispatcherTimer? _timer;
        private double _cpuPeak = 0;

        public DashboardPage()
        {
            InitializeComponent();
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            try
            {
                _hardwareService = new HardwareMonitorService();
                _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                _timer.Tick += (s, e) => UpdateStats();
                _timer.Start();
                UpdateStats();
            }
            catch
            {
                CpuModel.Text = "Unable to read hardware";
            }
        }

        private void UpdateStats()
        {
            try
            {
                if (_hardwareService == null) return;
                
                var info = _hardwareService.GetSystemInfo();
                
                // Hardware info
                CpuModel.Text = info.Cpu.Model;
                CpuDetails.Text = $"{info.Cpu.Cores} Cores";
                GpuModel.Text = info.Gpu.Model;
                GpuDetails.Text = $"{info.Gpu.MemoryTotalGb:F0} GB";
                RamTotal.Text = $"{info.Ram.TotalGb:F1} GB";
                RamUsed.Text = $"{info.Ram.UsedGb:F1} GB in use";
                DiskTotal.Text = $"{info.Disk.TotalGb:F0} GB";
                DiskFree.Text = $"{info.Disk.TotalGb - info.Disk.UsedGb:F0} GB free";
                
                // Usage bars (max width ~300px)
                double maxWidth = 300;
                CpuBar.Width = (info.Cpu.Usage / 100.0) * maxWidth;
                RamBar.Width = (info.Ram.UsagePercent / 100.0) * maxWidth;
                GpuBar.Width = (info.Gpu.Usage / 100.0) * maxWidth;
                DiskBar.Width = (info.Disk.UsagePercent / 100.0) * maxWidth;
                
                CpuPercent.Text = $"{info.Cpu.Usage:F0}%";
                RamPercent.Text = $"{info.Ram.UsagePercent:F0}%";
                GpuPercent.Text = $"{info.Gpu.Usage:F0}%";
                DiskPercent.Text = $"{info.Disk.UsagePercent:F0}%";
                
                // Temps
                CpuTemp.Text = $"{info.Cpu.Temperature:F0}°C";
                GpuTemp.Text = $"{info.Gpu.Temperature:F0}°C";
                
                if (info.Cpu.Temperature > _cpuPeak)
                    _cpuPeak = info.Cpu.Temperature;
                CpuPeak.Text = $"{_cpuPeak:F0}°C";
            }
            catch { }
        }
    }
}
