using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using AIO_PC_Tool_v2.Services;
using AIO_PC_Tool_v2.ViewModels;

namespace AIO_PC_Tool_v2;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();
        base.OnStartup(e);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Core Services - Singletons (one instance shared)
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ITweakService, TweakService>();
        services.AddSingleton<IHardwareMonitorService, HardwareMonitorService>();
        services.AddSingleton<ICleanerService, CleanerService>();
        services.AddSingleton<IDnsService, DnsService>();
        services.AddSingleton<IRestorePointService, RestorePointService>();
        services.AddSingleton<IUtilitiesService, UtilitiesService>();

        // ViewModels - Transient (new instance each time)
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<TweaksViewModel>();
        services.AddTransient<CleanerViewModel>();
        services.AddTransient<DnsViewModel>();
        services.AddTransient<RestorePointsViewModel>();
        services.AddTransient<UtilitiesViewModel>();
        services.AddTransient<SettingsViewModel>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Cleanup hardware monitor
        if (Services.GetService<IHardwareMonitorService>() is IDisposable disposable)
        {
            disposable.Dispose();
        }
        base.OnExit(e);
    }
}
