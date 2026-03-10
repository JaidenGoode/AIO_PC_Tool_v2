using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using AIO_PC_Tool_v2.Services;
using AIO_PC_Tool_v2.ViewModels;

namespace AIO_PC_Tool_v2
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddSingleton<TweakService>();
            services.AddSingleton<CleanerService>();
            services.AddSingleton<HardwareMonitorService>();
            services.AddSingleton<UtilitiesService>();
            services.AddSingleton<DnsService>();
            services.AddSingleton<RestorePointService>();

            // ViewModels
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<TweaksViewModel>();
            services.AddTransient<CleanerViewModel>();
            services.AddTransient<UtilitiesViewModel>();
            services.AddTransient<DnsViewModel>();
            services.AddTransient<RestorePointsViewModel>();
            services.AddTransient<SettingsViewModel>();
        }
    }
}
