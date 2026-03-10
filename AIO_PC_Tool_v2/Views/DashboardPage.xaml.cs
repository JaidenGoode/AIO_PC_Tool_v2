using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class DashboardPage : Page
    {
        private readonly HardwareMonitorService _hardwareService;
        private readonly TweakService _tweakService;
        private DispatcherTimer? _updateTimer;

        public DashboardPage()
        {
            InitializeComponent();
            _hardwareService = new HardwareMonitorService();
            _tweakService = new TweakService();
            
            UpdateDateTime();
            UpdateOptimizationCount();
            StartMonitoring();
            
            Unloaded += OnUnloaded;
        }

        private void UpdateDateTime()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) =>
            {
                CurrentTime.Text = DateTime.Now.ToString("HH:mm");
                CurrentDate.Text = DateTime.Now.ToString("dddd, MMMM dd");
            };
            timer.Start();
            
            // Initial update
            CurrentTime.Text = DateTime.Now.ToString("HH:mm");
            CurrentDate.Text = DateTime.Now.ToString("dddd, MMMM dd");
        }

        private void UpdateOptimizationCount()
        {
            try
            {
                var tweaks = _tweakService.GetAllTweaks();
                foreach (var tweak in tweaks)
                {
                    try { _tweakService.CheckTweakStatus(tweak); } catch { }
                }
                var activeCount = tweaks.Count(t => t.IsActive);
                OptimizationsActive.Text = $"{activeCount} optimization{(activeCount != 1 ? "s" : "")} active";
            }
            catch
            {
                OptimizationsActive.Text = "0 optimizations active";
            }
        }

        private void StartMonitoring()
        {
            UpdateHardwareInfo();
            
            _updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _updateTimer.Tick += (s, e) => UpdateHardwareInfo();
            _updateTimer.Start();
        }

        private void UpdateHardwareInfo()
        {
            try
            {
                var info = _hardwareService.GetSystemInfo();

                // CPU
                CpuUsage.Text = $"{info.Cpu.Usage:0}%";
                CpuName.Text = info.Cpu.Model;
                CpuTemp.Text = info.Cpu.Temperature > 0 ? $"{info.Cpu.Temperature:0}°C" : "--°C";
                CpuBar.Width = new GridLength(Math.Min(info.Cpu.Usage, 100), GridUnitType.Star).Value * 1.8; // Scale to parent width

                // GPU
                GpuUsage.Text = $"{info.Gpu.Usage:0}%";
                GpuName.Text = info.Gpu.Model;
                GpuTemp.Text = info.Gpu.Temperature > 0 ? $"{info.Gpu.Temperature:0}°C" : "--°C";
                GpuBar.Width = new GridLength(Math.Min(info.Gpu.Usage, 100), GridUnitType.Star).Value * 1.8;

                // RAM
                var ramPercent = info.Ram.UsagePercent;
                RamUsage.Text = $"{ramPercent:0}%";
                RamInfo.Text = $"{info.Ram.UsedGb:0.0} / {info.Ram.TotalGb:0.0} GB";
                RamBar.Width = new GridLength(Math.Min(ramPercent, 100), GridUnitType.Star).Value * 1.8;

                // Disk
                var diskPercent = info.Disk.UsagePercent;
                DiskUsage.Text = $"{diskPercent:0}%";
                DiskInfo.Text = $"{info.Disk.UsedGb:0} / {info.Disk.TotalGb:0} GB";
                DiskBar.Width = new GridLength(Math.Min(diskPercent, 100), GridUnitType.Star).Value * 1.8;
            }
            catch
            {
                // Silently handle errors
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _updateTimer?.Stop();
            _hardwareService.Dispose();
        }

        private void QuickAction_Cleaner(object sender, MouseButtonEventArgs e)
        {
            NavigateToPage("Cleaner");
        }

        private void QuickAction_Tweaks(object sender, MouseButtonEventArgs e)
        {
            NavigateToPage("Tweaks");
        }

        private void QuickAction_DNS(object sender, MouseButtonEventArgs e)
        {
            NavigateToPage("Dns");
        }

        private void QuickAction_Restore(object sender, MouseButtonEventArgs e)
        {
            NavigateToPage("Restore");
        }

        private void NavigateToPage(string pageName)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavigateTo(pageName);
            }
        }
    }
}
