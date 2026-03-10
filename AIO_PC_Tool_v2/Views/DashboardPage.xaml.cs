using System.Windows.Controls;
using System.Windows.Threading;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class DashboardPage : Page
    {
        private HardwareMonitorService? _hardwareService;
        private DispatcherTimer? _timer;

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
                _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                _timer.Tick += (s, e) => UpdateStats();
                _timer.Start();
                UpdateStats();
            }
            catch
            {
                CpuUsage.Text = "N/A";
                GpuUsage.Text = "N/A";
                RamUsage.Text = "N/A";
                DiskUsage.Text = "N/A";
            }
        }

        private void UpdateStats()
        {
            try
            {
                if (_hardwareService == null) return;
                
                var info = _hardwareService.GetSystemInfo();
                
                CpuUsage.Text = $"{info.Cpu.Usage:F0}%";
                CpuTemp.Text = $"Temperature: {info.Cpu.Temperature:F0}°C";
                
                GpuUsage.Text = $"{info.Gpu.Usage:F0}%";
                GpuTemp.Text = $"Temperature: {info.Gpu.Temperature:F0}°C";
                
                RamUsage.Text = $"{info.Ram.UsagePercent:F0}%";
                RamDetails.Text = $"{info.Ram.UsedGb:F1} / {info.Ram.TotalGb:F1} GB";
                
                DiskUsage.Text = $"{info.Disk.UsagePercent:F0}%";
                DiskDetails.Text = $"{info.Disk.UsedGb:F0} / {info.Disk.TotalGb:F0} GB";
            }
            catch { }
        }
    }
}
