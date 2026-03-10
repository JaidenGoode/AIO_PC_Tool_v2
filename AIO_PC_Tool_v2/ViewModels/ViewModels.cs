using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AIO_PC_Tool_v2.Services;
using AIO_PC_Tool_v2.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _currentPage = "Dashboard";
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _statusMessage = "Ready";

    [RelayCommand]
    private void NavigateTo(string page)
    {
        CurrentPage = page;
    }
}

public partial class DashboardViewModel : ObservableObject
{
    private readonly IHardwareMonitorService _hardwareService;
    private readonly ITweakService _tweakService;

    [ObservableProperty] private SystemInfo _systemInfo = new();
    [ObservableProperty] private double _cpuUsage;
    [ObservableProperty] private double _ramUsage;
    [ObservableProperty] private double _gpuUsage;
    [ObservableProperty] private double _cpuTemp;
    [ObservableProperty] private double _gpuTemp;
    [ObservableProperty] private int _activeTweaks;
    [ObservableProperty] private int _totalTweaks;
    [ObservableProperty] private bool _isMonitoring;
    [ObservableProperty] private string _monitorButtonText = "Start Monitoring";

    public DashboardViewModel()
    {
        _hardwareService = App.Services.GetRequiredService<IHardwareMonitorService>();
        _tweakService = App.Services.GetRequiredService<ITweakService>();
        
        _hardwareService.UsageUpdated += OnUsageUpdated;
        
        SystemInfo = _hardwareService.GetSystemInfo();
        TotalTweaks = _tweakService.GetTotalTweaksCount();
        
        // Auto-detect tweaks on load
        _ = DetectTweaksAsync();
    }

    private void OnUsageUpdated(object? sender, SystemInfo info)
    {
        CpuUsage = info.Cpu.Usage;
        RamUsage = info.Memory.UsagePercent;
        GpuUsage = info.Gpu.Usage;
        CpuTemp = info.Cpu.Temperature;
        GpuTemp = info.Gpu.Temperature;
    }

    [RelayCommand]
    private void ToggleMonitoring()
    {
        if (IsMonitoring)
        {
            _hardwareService.StopMonitoring();
            IsMonitoring = false;
            MonitorButtonText = "Start Monitoring";
        }
        else
        {
            _hardwareService.StartMonitoring();
            IsMonitoring = true;
            MonitorButtonText = "Stop Monitoring";
        }
    }

    [RelayCommand]
    private async Task DetectTweaksAsync()
    {
        await _tweakService.DetectAllTweaksAsync();
        ActiveTweaks = _tweakService.GetActiveTweaksCount();
    }
}

public partial class TweaksViewModel : ObservableObject
{
    private readonly ITweakService _tweakService;

    [ObservableProperty] private ObservableCollection<TweakItemViewModel> _tweaks = new();
    [ObservableProperty] private ObservableCollection<TweakItemViewModel> _filteredTweaks = new();
    [ObservableProperty] private string _selectedCategory = "all";
    [ObservableProperty] private string _searchQuery = string.Empty;
    [ObservableProperty] private bool _isApplying;
    [ObservableProperty] private string _statusMessage = "Detecting current system state...";
    [ObservableProperty] private int _optimizedCount;
    [ObservableProperty] private int _totalCount;
    [ObservableProperty] private bool _isDetecting;

    public List<string> Categories { get; } = new() 
    { 
        "all", "privacy", "performance", "gaming", "system", "network", "services" 
    };

    public TweaksViewModel()
    {
        _tweakService = App.Services.GetRequiredService<ITweakService>();
        _tweakService.TweakStateChanged += OnTweakStateChanged;
        
        // Initialize and auto-detect
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        IsDetecting = true;
        StatusMessage = "Detecting current system state...";
        
        // Load tweaks
        var tweaksList = _tweakService.GetAllTweaks();
        TotalCount = tweaksList.Count;
        
        foreach (var tweak in tweaksList)
        {
            Tweaks.Add(new TweakItemViewModel(tweak, this));
        }
        
        // Auto-detect all states
        await _tweakService.DetectAllTweaksAsync();
        
        // Update UI
        UpdateFilteredTweaks();
        UpdateCounts();
        
        IsDetecting = false;
        StatusMessage = $"{OptimizedCount} of {TotalCount} optimizations active";
    }

    private void OnTweakStateChanged(object? sender, TweakStateChangedEventArgs e)
    {
        UpdateCounts();
        var status = e.NewState ? "OPTIMIZED" : "DEFAULT";
        StatusMessage = $"{e.Tweak.Title}: {status}";
    }

    private void UpdateCounts()
    {
        OptimizedCount = _tweakService.GetActiveTweaksCount();
        
        // Update individual items
        foreach (var item in Tweaks)
        {
            item.RefreshState();
        }
    }

    partial void OnSelectedCategoryChanged(string value) => UpdateFilteredTweaks();
    partial void OnSearchQueryChanged(string value) => UpdateFilteredTweaks();

    private void UpdateFilteredTweaks()
    {
        var filtered = Tweaks.AsEnumerable();
        
        if (SelectedCategory != "all")
            filtered = filtered.Where(t => t.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrEmpty(SearchQuery))
            filtered = filtered.Where(t => 
                t.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        
        FilteredTweaks = new ObservableCollection<TweakItemViewModel>(filtered);
    }

    [RelayCommand]
    private async Task ApplyPresetAsync(string preset)
    {
        IsApplying = true;
        StatusMessage = $"Applying {preset} preset...";
        
        var tweaksToApply = preset switch
        {
            "gaming" => Tweaks.Where(t => t.Category == "gaming" && !t.IsActive),
            "privacy" => Tweaks.Where(t => t.Category == "privacy" && !t.IsActive),
            "performance" => Tweaks.Where(t => t.Category == "performance" && !t.IsActive),
            _ => Enumerable.Empty<TweakItemViewModel>()
        };

        foreach (var tweak in tweaksToApply.ToList())
        {
            await tweak.ToggleAsync();
        }
        
        UpdateCounts();
        StatusMessage = $"{preset} preset applied";
        IsApplying = false;
    }

    [RelayCommand]
    private async Task DetectAllAsync()
    {
        IsDetecting = true;
        StatusMessage = "Re-detecting all tweaks...";
        
        await _tweakService.DetectAllTweaksAsync();
        
        UpdateCounts();
        UpdateFilteredTweaks();
        
        IsDetecting = false;
        StatusMessage = $"{OptimizedCount} of {TotalCount} optimizations active";
    }

    public async Task ApplyTweakAsync(Tweak tweak, bool enable)
    {
        IsApplying = true;
        var (success, message) = await _tweakService.ApplyTweakAsync(tweak, enable);
        StatusMessage = message;
        IsApplying = false;
    }
}

public partial class TweakItemViewModel : ObservableObject
{
    private readonly Tweak _tweak;
    private readonly TweaksViewModel _parent;

    public int Id => _tweak.Id;
    public string Title => _tweak.Title;
    public string Description => _tweak.Description;
    public string Category => _tweak.Category;
    public string? Warning => _tweak.Warning;
    public string? FeatureBreaks => _tweak.FeatureBreaks;

    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private string _statusText = "DEFAULT";
    [ObservableProperty] private bool _isApplying;

    public TweakItemViewModel(Tweak tweak, TweaksViewModel parent)
    {
        _tweak = tweak;
        _parent = parent;
        RefreshState();
    }

    public void RefreshState()
    {
        IsActive = _tweak.IsActive;
        StatusText = _tweak.IsActive ? "OPTIMIZED" : "DEFAULT";
    }

    [RelayCommand]
    public async Task ToggleAsync()
    {
        IsApplying = true;
        await _parent.ApplyTweakAsync(_tweak, !_tweak.IsActive);
        RefreshState();
        IsApplying = false;
    }
}

public partial class CleanerViewModel : ObservableObject
{
    private readonly ICleanerService _cleanerService;

    [ObservableProperty] private ObservableCollection<CleanCategoryViewModel> _categories = new();
    [ObservableProperty] private bool _isScanning;
    [ObservableProperty] private bool _isCleaning;
    [ObservableProperty] private string _totalSize = "0 B";
    [ObservableProperty] private int _totalFiles;
    [ObservableProperty] private string _statusMessage = "Click 'Scan' to analyze your system";
    [ObservableProperty] private ObservableCollection<CleaningHistory> _history = new();
    [ObservableProperty] private bool _hasScanned;

    public CleanerViewModel()
    {
        _cleanerService = App.Services.GetRequiredService<ICleanerService>();
        _ = LoadHistoryAsync();
    }

    private async Task LoadHistoryAsync()
    {
        var historyList = await _cleanerService.GetHistoryAsync();
        History = new ObservableCollection<CleaningHistory>(historyList.TakeLast(5).Reverse());
    }

    [RelayCommand]
    private async Task ScanAsync()
    {
        IsScanning = true;
        StatusMessage = "Scanning system...";
        Categories.Clear();
        
        var progress = new Progress<string>(msg => StatusMessage = msg);
        var results = await _cleanerService.ScanAsync(progress);
        
        foreach (var cat in results.Where(c => c.Found))
        {
            Categories.Add(new CleanCategoryViewModel(cat));
        }
        
        TotalSize = FormatSize(_cleanerService.GetTotalCleanableSize());
        TotalFiles = _cleanerService.GetTotalCleanableFiles();
        
        StatusMessage = $"Found {TotalSize} in {TotalFiles} files";
        IsScanning = false;
        HasScanned = true;
    }

    [RelayCommand]
    private async Task CleanAsync()
    {
        var selected = Categories.Where(c => c.IsSelected).Select(c => c.Id).ToList();
        if (selected.Count == 0)
        {
            StatusMessage = "Select categories to clean";
            return;
        }

        IsCleaning = true;
        StatusMessage = "Cleaning...";
        
        var progress = new Progress<string>(msg => StatusMessage = msg);
        var result = await _cleanerService.CleanAsync(selected, progress);
        
        if (result.Errors.Any())
        {
            StatusMessage = $"Cleaned {result.FreedHuman}. Some errors occurred.";
        }
        else
        {
            StatusMessage = $"Successfully freed {result.FreedHuman} from {result.CategoriesCleaned} categories";
        }
        
        IsCleaning = false;
        
        await LoadHistoryAsync();
        await ScanAsync();
    }

    [RelayCommand]
    private void SelectAll()
    {
        foreach (var cat in Categories)
            cat.IsSelected = true;
    }

    [RelayCommand]
    private void DeselectAll()
    {
        foreach (var cat in Categories)
            cat.IsSelected = false;
    }

    private static string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
        return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
    }
}

public partial class CleanCategoryViewModel : ObservableObject
{
    private readonly CleanCategory _category;

    public string Id => _category.Id;
    public string Name => _category.Name;
    public string Description => _category.Description;
    public string SizeHuman => _category.SizeHuman;
    public int FileCount => _category.FileCount;

    [ObservableProperty] private bool _isSelected = true;

    public CleanCategoryViewModel(CleanCategory category)
    {
        _category = category;
    }
}

public partial class DnsViewModel : ObservableObject
{
    private readonly IDnsService _dnsService;

    [ObservableProperty] private ObservableCollection<DnsProviderViewModel> _providers = new();
    [ObservableProperty] private string? _currentProvider;
    [ObservableProperty] private bool _isApplying;
    [ObservableProperty] private string _statusMessage = "Select a DNS provider";

    public DnsViewModel()
    {
        _dnsService = App.Services.GetRequiredService<IDnsService>();
        Initialize();
    }

    private async void Initialize()
    {
        foreach (var provider in _dnsService.GetProviders())
        {
            Providers.Add(new DnsProviderViewModel(provider, this));
        }
        
        CurrentProvider = await _dnsService.GetCurrentProviderAsync();
        UpdateActiveStates();
    }

    private void UpdateActiveStates()
    {
        foreach (var p in Providers)
        {
            p.IsActive = p.Id == CurrentProvider;
        }
    }

    public async Task SetDnsAsync(string providerId)
    {
        IsApplying = true;
        StatusMessage = "Applying DNS settings...";
        
        var success = await _dnsService.SetDnsAsync(providerId);
        
        if (success)
        {
            CurrentProvider = providerId;
            UpdateActiveStates();
            var provider = Providers.FirstOrDefault(p => p.Id == providerId);
            StatusMessage = $"DNS set to {provider?.Name ?? providerId}";
        }
        else
        {
            StatusMessage = "Failed to apply DNS settings";
        }
        
        IsApplying = false;
    }
}

public partial class DnsProviderViewModel : ObservableObject
{
    private readonly DnsProvider _provider;
    private readonly DnsViewModel _parent;

    public string Id => _provider.Id;
    public string Name => _provider.Name;
    public string Description => _provider.Description;
    public string PrimaryDns => _provider.PrimaryDns;
    public string SecondaryDns => _provider.SecondaryDns;

    [ObservableProperty] private bool _isActive;

    public DnsProviderViewModel(DnsProvider provider, DnsViewModel parent)
    {
        _provider = provider;
        _parent = parent;
    }

    [RelayCommand]
    private async Task ApplyAsync()
    {
        await _parent.SetDnsAsync(Id);
    }
}

public partial class RestorePointsViewModel : ObservableObject
{
    private readonly IRestorePointService _restoreService;

    [ObservableProperty] private ObservableCollection<RestorePoint> _restorePoints = new();
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isCreating;
    [ObservableProperty] private string _newPointName = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;

    public RestorePointsViewModel()
    {
        _restoreService = App.Services.GetRequiredService<IRestorePointService>();
        _ = LoadRestorePointsAsync();
    }

    [RelayCommand]
    private async Task LoadRestorePointsAsync()
    {
        IsLoading = true;
        StatusMessage = "Loading restore points...";
        
        var points = await _restoreService.GetRestorePointsAsync();
        RestorePoints = new ObservableCollection<RestorePoint>(points.OrderByDescending(p => p.CreationTime));
        
        StatusMessage = $"Found {points.Count} restore points";
        IsLoading = false;
    }

    [RelayCommand]
    private async Task CreateRestorePointAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPointName))
        {
            StatusMessage = "Enter a name for the restore point";
            return;
        }

        IsCreating = true;
        StatusMessage = "Creating restore point (this may take a minute)...";
        
        var success = await _restoreService.CreateRestorePointAsync(NewPointName);
        
        StatusMessage = success ? "Restore point created successfully!" : "Failed to create restore point";
        NewPointName = string.Empty;
        IsCreating = false;
        
        if (success)
            await LoadRestorePointsAsync();
    }
}

public partial class UtilitiesViewModel : ObservableObject
{
    private readonly IUtilitiesService _utilitiesService;

    [ObservableProperty] private ObservableCollection<UtilityAction> _utilities = new();
    [ObservableProperty] private ObservableCollection<UtilityAction> _filteredUtilities = new();
    [ObservableProperty] private bool _isRunning;
    [ObservableProperty] private string _statusMessage = "Select a utility to run";
    [ObservableProperty] private string _selectedCategory = "all";

    public List<string> Categories { get; } = new() 
    { 
        "all", "recommended", "repair", "network", "tools", "power", "quick" 
    };

    public UtilitiesViewModel()
    {
        _utilitiesService = App.Services.GetRequiredService<IUtilitiesService>();
        
        foreach (var utility in _utilitiesService.GetUtilities())
        {
            Utilities.Add(utility);
        }
        
        FilterUtilities();
    }

    partial void OnSelectedCategoryChanged(string value) => FilterUtilities();

    private void FilterUtilities()
    {
        var filtered = SelectedCategory == "all" 
            ? Utilities 
            : new ObservableCollection<UtilityAction>(Utilities.Where(u => u.Category == SelectedCategory));
        
        FilteredUtilities = filtered;
    }

    [RelayCommand]
    private async Task RunUtilityAsync(string actionId)
    {
        var utility = Utilities.FirstOrDefault(u => u.Id == actionId);
        if (utility == null) return;

        IsRunning = true;
        StatusMessage = $"Running {utility.Name}...";
        
        var (success, output) = await _utilitiesService.RunUtilityAsync(actionId);
        
        StatusMessage = output;
        
        if (utility.RequiresRestart)
        {
            StatusMessage += " (Restart required)";
        }
        
        IsRunning = false;
    }
}

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    // Theme Settings
    [ObservableProperty] private bool _isDarkMode = true;
    [ObservableProperty] private string _selectedAccentColor = "Red";
    [ObservableProperty] private ObservableCollection<AccentColorOption> _accentColors = new();
    
    // General Settings
    [ObservableProperty] private bool _startWithWindows;
    [ObservableProperty] private bool _minimizeToTray;
    [ObservableProperty] private bool _autoDetectOnStartup = true;
    
    // About
    [ObservableProperty] private string _appVersion = "4.0.0";
    [ObservableProperty] private string _buildDate = "December 2025";

    public SettingsViewModel()
    {
        _settingsService = App.Services.GetRequiredService<ISettingsService>();
        InitializeAccentColors();
        LoadSettings();
    }

    private void InitializeAccentColors()
    {
        // Add all available accent colors
        foreach (var colorName in Helpers.ThemeManager.GetAccentColorNames())
        {
            var previewColor = Helpers.ThemeManager.GetAccentPreviewColor(colorName);
            AccentColors.Add(new AccentColorOption
            {
                Name = colorName,
                PreviewColor = new System.Windows.Media.SolidColorBrush(previewColor),
                IsSelected = colorName == "Red" // Default
            });
        }
    }

    private void LoadSettings()
    {
        // Load theme mode (default: dark)
        IsDarkMode = _settingsService.GetSetting("theme_mode") != "light";
        
        // Load accent color (default: Red)
        var savedAccent = _settingsService.GetSetting("accent_color");
        SelectedAccentColor = string.IsNullOrEmpty(savedAccent) ? "Red" : savedAccent;
        
        // Load other settings
        StartWithWindows = _settingsService.GetSetting("start_with_windows") == "true";
        MinimizeToTray = _settingsService.GetSetting("minimize_to_tray") == "true";
        AutoDetectOnStartup = _settingsService.GetSetting("auto_detect") != "false";
        
        // Apply loaded theme immediately
        ApplyCurrentTheme();
    }

    private void ApplyCurrentTheme()
    {
        Helpers.ThemeManager.SetThemeMode(IsDarkMode);
        Helpers.ThemeManager.SetAccentColor(SelectedAccentColor);
        UpdateAccentColorSelection();
    }

    private void UpdateAccentColorSelection()
    {
        foreach (var color in AccentColors)
        {
            color.IsSelected = color.Name == SelectedAccentColor;
        }
    }

    partial void OnIsDarkModeChanged(bool value)
    {
        _settingsService.SetSetting("theme_mode", value ? "dark" : "light");
        Helpers.ThemeManager.SetThemeMode(value);
    }

    partial void OnSelectedAccentColorChanged(string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        
        _settingsService.SetSetting("accent_color", value);
        Helpers.ThemeManager.SetAccentColor(value);
        UpdateAccentColorSelection();
    }
    
    partial void OnStartWithWindowsChanged(bool value) => 
        _settingsService.SetSetting("start_with_windows", value.ToString().ToLower());
    
    partial void OnMinimizeToTrayChanged(bool value) => 
        _settingsService.SetSetting("minimize_to_tray", value.ToString().ToLower());
    
    partial void OnAutoDetectOnStartupChanged(bool value) => 
        _settingsService.SetSetting("auto_detect", value.ToString().ToLower());

    [RelayCommand]
    private void SelectAccentColor(string colorName)
    {
        SelectedAccentColor = colorName;
    }
}

/// <summary>
/// Represents an accent color option for the settings UI
/// </summary>
public partial class AccentColorOption : ObservableObject
{
    public string Name { get; set; } = string.Empty;
    public System.Windows.Media.SolidColorBrush PreviewColor { get; set; } = new();
    
    [ObservableProperty] private bool _isSelected;
}
