using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;
using AIO_PC_Tool_v2.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace AIO_PC_Tool_v2.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private HardwareMonitorService? _hardwareService;
        private readonly DispatcherTimer _timer;

        [ObservableProperty]
        private SystemInfo systemInfo = new();

        public DashboardViewModel()
        {
            try
            {
                _hardwareService = new HardwareMonitorService();
            }
            catch
            {
                _hardwareService = null;
            }
            
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _timer.Tick += (s, e) => UpdateSystemInfo();
            _timer.Start();
            UpdateSystemInfo();
        }

        private void UpdateSystemInfo()
        {
            try
            {
                if (_hardwareService != null)
                {
                    SystemInfo = _hardwareService.GetSystemInfo();
                }
            }
            catch { }
        }
    }

    public partial class TweaksViewModel : ObservableObject
    {
        private readonly TweakService _tweakService;

        [ObservableProperty]
        private ObservableCollection<Tweak> tweaks = new();

        [ObservableProperty]
        private ObservableCollection<Tweak> filteredTweaks = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private string selectedCategory = "All";

        [ObservableProperty]
        private Tweak? selectedTweak;

        public ObservableCollection<string> Categories { get; } = new()
        {
            "All", "Privacy", "Performance", "Gaming", "Visual", "Network", "Power", "Security", "Storage"
        };

        public TweaksViewModel()
        {
            _tweakService = new TweakService();
            LoadTweaks();
        }

        private void LoadTweaks()
        {
            Tweaks = _tweakService.GetAllTweaks();
            foreach (var tweak in Tweaks)
            {
                _tweakService.CheckTweakStatus(tweak);
            }
            FilterTweaks();
        }

        partial void OnSearchTextChanged(string value) => FilterTweaks();
        partial void OnSelectedCategoryChanged(string value) => FilterTweaks();

        private void FilterTweaks()
        {
            var filtered = Tweaks.Where(t =>
                (SelectedCategory == "All" || t.Category == SelectedCategory) &&
                (string.IsNullOrEmpty(SearchText) ||
                 t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                 t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

            FilteredTweaks = new ObservableCollection<Tweak>(filtered);
        }

        [RelayCommand]
        private void ApplyTweak(Tweak tweak)
        {
            _tweakService.ApplyTweak(tweak);
        }

        [RelayCommand]
        private void RevertTweak(Tweak tweak)
        {
            _tweakService.RevertTweak(tweak);
        }

        [RelayCommand]
        private void ApplyAll()
        {
            foreach (var tweak in FilteredTweaks)
            {
                _tweakService.ApplyTweak(tweak);
            }
        }

        [RelayCommand]
        private void RevertAll()
        {
            foreach (var tweak in FilteredTweaks)
            {
                _tweakService.RevertTweak(tweak);
            }
        }
    }

    public partial class CleanerViewModel : ObservableObject
    {
        private readonly CleanerService _cleanerService;

        [ObservableProperty]
        private ObservableCollection<CleanerCategory> categories = new();

        [ObservableProperty]
        private ObservableCollection<CleanHistory> history = new();

        [ObservableProperty]
        private string totalSize = "0 B";

        [ObservableProperty]
        private int totalFiles = 0;

        [ObservableProperty]
        private string statusMessage = "Ready to scan";

        public CleanerViewModel()
        {
            _cleanerService = new CleanerService();
            Categories = _cleanerService.GetCategories();
        }

        [RelayCommand]
        private async Task Scan()
        {
            StatusMessage = "Scanning...";
            long total = 0;
            int files = 0;

            foreach (var category in Categories)
            {
                var (size, count) = await _cleanerService.ScanCategoryAsync(category);
                total += size;
                files += count;
            }

            TotalSize = FormatSize(total);
            TotalFiles = files;
            StatusMessage = $"Scan complete. Found {files} files.";
        }

        [RelayCommand]
        private void SelectAll()
        {
            foreach (var category in Categories)
            {
                category.IsSelected = true;
            }
        }

        [RelayCommand]
        private async Task Clean()
        {
            StatusMessage = "Cleaning...";
            long cleaned = 0;

            foreach (var category in Categories.Where(c => c.IsSelected))
            {
                cleaned += await _cleanerService.CleanCategoryAsync(category);
            }

            History.Insert(0, new CleanHistory { Date = DateTime.Now, FreedBytes = cleaned });
            StatusMessage = $"Cleaned {FormatSize(cleaned)}";
            await Scan();
        }

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

    public partial class UtilitiesViewModel : ObservableObject
    {
        private readonly UtilitiesService _utilitiesService;

        [ObservableProperty]
        private ObservableCollection<UtilityItem> utilities = new();

        public UtilitiesViewModel()
        {
            _utilitiesService = new UtilitiesService();
            Utilities = _utilitiesService.GetUtilities();
        }

        [RelayCommand]
        private void RunUtility(UtilityItem utility)
        {
            utility.Action?.Invoke();
        }

        [RelayCommand]
        private void RunChrisTitus()
        {
            _utilitiesService.RunChrisTitusUtility();
        }
    }

    public partial class DnsViewModel : ObservableObject
    {
        private readonly DnsService _dnsService;

        [ObservableProperty]
        private ObservableCollection<DnsProvider> providers = new();

        [ObservableProperty]
        private DnsProvider? selectedProvider;

        public DnsViewModel()
        {
            _dnsService = new DnsService();
            Providers = _dnsService.GetProviders();
        }

        [RelayCommand]
        private void ApplyDns()
        {
            if (SelectedProvider != null)
            {
                _dnsService.SetDns(SelectedProvider);
            }
        }

        [RelayCommand]
        private void ResetDns()
        {
            _dnsService.ResetToAutomatic();
        }
    }

    public partial class RestorePointsViewModel : ObservableObject
    {
        private readonly RestorePointService _restorePointService;

        [ObservableProperty]
        private ObservableCollection<RestorePoint> restorePoints = new();

        [ObservableProperty]
        private string newPointName = string.Empty;

        public RestorePointsViewModel()
        {
            _restorePointService = new RestorePointService();
            LoadRestorePoints();
        }

        [RelayCommand]
        private void LoadRestorePoints()
        {
            RestorePoints = _restorePointService.GetRestorePoints();
        }

        [RelayCommand]
        private void CreateRestorePoint()
        {
            if (!string.IsNullOrWhiteSpace(NewPointName))
            {
                _restorePointService.CreateRestorePoint(NewPointName);
                NewPointName = string.Empty;
                LoadRestorePoints();
            }
        }
    }

    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isDarkMode = true;

        [ObservableProperty]
        private string selectedAccent = "Red";

        public ObservableCollection<string> AccentColors { get; } = new()
        {
            "Red", "Blue", "Green", "Purple", "Pink", "Orange", "Teal"
        };

        partial void OnIsDarkModeChanged(bool value)
        {
            ThemeManager.SetTheme(value, SelectedAccent);
        }

        partial void OnSelectedAccentChanged(string value)
        {
            ThemeManager.SetTheme(IsDarkMode, value);
        }
    }
}
