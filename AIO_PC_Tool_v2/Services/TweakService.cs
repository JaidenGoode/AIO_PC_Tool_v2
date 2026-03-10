using Microsoft.Win32;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Services
{
    public class TweakService
    {
        public ObservableCollection<Tweak> GetAllTweaks()
        {
            return new ObservableCollection<Tweak>
            {
                // === PRIVACY TWEAKS ===
                new Tweak
                {
                    Id = "disable_telemetry",
                    Title = "Disable Windows Telemetry",
                    Description = "Prevents Windows from collecting and sending diagnostic data to Microsoft. This improves privacy by stopping usage statistics, crash reports, and browsing data from being transmitted. Safe for all users.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    RegistryName = "AllowTelemetry",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_advertising_id",
                    Title = "Disable Advertising ID",
                    Description = "Prevents apps from using your unique advertising identifier to track you across applications and show targeted ads. Improves privacy without affecting functionality.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
                    RegistryName = "Enabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_activity_history",
                    Title = "Disable Activity History",
                    Description = "Stops Windows from collecting your activity history (apps used, files opened, websites visited). This data is normally synced to your Microsoft account.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Policies\Microsoft\Windows\System",
                    RegistryName = "EnableActivityFeed",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_location_tracking",
                    Title = "Disable Location Tracking",
                    Description = "Prevents Windows and apps from accessing your physical location. Some apps like Maps or Weather may have reduced functionality.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location",
                    RegistryName = "Value",
                    OptimizedValue = "Deny",
                    DefaultValue = "Allow"
                },
                new Tweak
                {
                    Id = "disable_feedback_requests",
                    Title = "Disable Feedback Requests",
                    Description = "Stops Windows from asking you for feedback about your experience. Reduces interruptions without any negative impact.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Siuf\Rules",
                    RegistryName = "NumberOfSIUFInPeriod",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_app_suggestions",
                    Title = "Disable App Suggestions",
                    Description = "Prevents Windows from suggesting apps to install and showing advertisements in the Start menu. Clean Start menu experience.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    RegistryName = "SubscribedContent-338388Enabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_tailored_experiences",
                    Title = "Disable Tailored Experiences",
                    Description = "Stops Microsoft from using your diagnostic data to personalize tips, ads, and recommendations. Pure privacy improvement.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy",
                    RegistryName = "TailoredExperiencesWithDiagnosticDataEnabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },

                // === PERFORMANCE TWEAKS ===
                new Tweak
                {
                    Id = "disable_superfetch",
                    Title = "Disable Superfetch/SysMain",
                    Description = "Disables the SysMain service which preloads frequently used apps into RAM. Can improve performance on systems with SSD drives or limited RAM. HDD users may want to keep this enabled.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Services\SysMain",
                    RegistryName = "Start",
                    OptimizedValue = 4,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "disable_prefetch",
                    Title = "Disable Prefetch",
                    Description = "Disables application prefetching which caches app data for faster startup. Recommended only for SSD users as it provides minimal benefit on solid-state drives.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    RegistryName = "EnablePrefetcher",
                    OptimizedValue = 0,
                    DefaultValue = 3
                },
                new Tweak
                {
                    Id = "disable_search_indexing",
                    Title = "Disable Windows Search Indexing",
                    Description = "Stops the Windows Search indexer from running in the background. Reduces CPU and disk usage but makes file searches slower. Best for users who rarely use Windows Search.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Services\WSearch",
                    RegistryName = "Start",
                    OptimizedValue = 4,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "disable_hibernation",
                    Title = "Disable Hibernation",
                    Description = "Disables Windows hibernation feature and deletes the hiberfil.sys file, freeing up disk space equal to your RAM size. Sleep mode still works normally.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Power",
                    RegistryName = "HibernateEnabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_fast_startup",
                    Title = "Disable Fast Startup",
                    Description = "Disables Windows hybrid shutdown/fast boot. Can fix various boot issues and is recommended for dual-boot systems. Clean shutdown ensures all drivers reload properly.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Session Manager\Power",
                    RegistryName = "HiberbootEnabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "optimize_processor_scheduling",
                    Title = "Optimize Processor Scheduling",
                    Description = "Prioritizes foreground applications over background services for more responsive app performance. Standard optimization safe for all systems.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\PriorityControl",
                    RegistryName = "Win32PrioritySeparation",
                    OptimizedValue = 38,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "disable_transparency",
                    Title = "Disable Transparency Effects",
                    Description = "Disables window transparency and blur effects throughout Windows. Can improve performance on older hardware or integrated graphics.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    RegistryName = "EnableTransparency",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },

                // === GAMING TWEAKS ===
                new Tweak
                {
                    Id = "enable_game_mode",
                    Title = "Enable Game Mode",
                    Description = "Enables Windows Game Mode which optimizes your PC for gaming by prioritizing game processes and reducing background activity during gameplay.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\GameBar",
                    RegistryName = "AllowAutoGameMode",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },
                new Tweak
                {
                    Id = "disable_game_bar",
                    Title = "Disable Xbox Game Bar",
                    Description = "Disables the Xbox Game Bar overlay which can cause performance issues in some games. You can still use third-party alternatives like NVIDIA GeForce Experience.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR",
                    RegistryName = "AppCaptureEnabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_game_dvr",
                    Title = "Disable Game DVR/Recording",
                    Description = "Disables background game recording and capture features. Frees up system resources during gaming for better performance.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\GraphicsDrivers",
                    RegistryName = "HwSchMode",
                    OptimizedValue = 1,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "enable_hardware_gpu_scheduling",
                    Title = "Enable Hardware GPU Scheduling",
                    Description = "Allows your GPU to manage its own video memory scheduling, reducing CPU overhead and potentially improving gaming performance. Requires compatible GPU (NVIDIA 10-series+, AMD 5000+).",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    Warning = "Requires a compatible GPU (NVIDIA GTX 10-series or newer, AMD RX 5000 series or newer). May cause issues on older hardware.",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\GraphicsDrivers",
                    RegistryName = "HwSchMode",
                    OptimizedValue = 2,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_fullscreen_optimizations",
                    Title = "Disable Fullscreen Optimizations",
                    Description = "Disables Windows fullscreen optimizations which can cause input lag and stuttering in some games. Recommended for competitive gaming.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameConfigStore",
                    RegistryName = "GameDVR_FSEBehavior",
                    OptimizedValue = 2,
                    DefaultValue = 0
                },

                // === VISUAL TWEAKS ===
                new Tweak
                {
                    Id = "disable_animations",
                    Title = "Disable Window Animations",
                    Description = "Disables minimize/maximize animations and other visual effects. Makes Windows feel snappier, especially on older hardware.",
                    Category = "Visual",
                    WindowsVersion = "10/11",
                    RegistryPath = @"Control Panel\Desktop\WindowMetrics",
                    RegistryName = "MinAnimate",
                    OptimizedValue = "0",
                    DefaultValue = "1"
                },
                new Tweak
                {
                    Id = "show_file_extensions",
                    Title = "Show File Extensions",
                    Description = "Shows file extensions for all known file types. Helps identify file types and can help avoid malware disguised with fake extensions. Security best practice.",
                    Category = "Visual",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryName = "HideFileExt",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "show_hidden_files",
                    Title = "Show Hidden Files",
                    Description = "Shows hidden files and folders in File Explorer. Useful for accessing system files and troubleshooting. Does not show protected operating system files.",
                    Category = "Visual",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryName = "Hidden",
                    OptimizedValue = 1,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "disable_snap_assist",
                    Title = "Disable Snap Assist",
                    Description = "Disables the window snap suggestions when dragging windows to screen edges. Windows still snaps, but won't show suggestions for filling remaining space.",
                    Category = "Visual",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryName = "SnapAssist",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "classic_context_menu",
                    Title = "Use Classic Context Menu",
                    Description = "Restores the classic Windows 10 right-click context menu in Windows 11. Shows all options immediately without clicking 'Show more options'.",
                    Category = "Visual",
                    WindowsVersion = "11",
                    RegistryPath = @"SOFTWARE\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32",
                    RegistryName = "",
                    OptimizedValue = "",
                    DefaultValue = null
                },

                // === NETWORK TWEAKS ===
                new Tweak
                {
                    Id = "disable_nagle",
                    Title = "Disable Nagle's Algorithm",
                    Description = "Disables TCP packet batching (Nagle's algorithm). Can reduce network latency in games and real-time applications. Safe for all users.",
                    Category = "Network",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\MSMQ\Parameters",
                    RegistryName = "TcpNoDelay",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },
                new Tweak
                {
                    Id = "disable_network_throttling",
                    Title = "Disable Network Throttling",
                    Description = "Removes the network throttling that Windows applies to multimedia applications. Can improve streaming and gaming network performance.",
                    Category = "Network",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    RegistryName = "NetworkThrottlingIndex",
                    OptimizedValue = unchecked((int)0xFFFFFFFF),
                    DefaultValue = 10
                },
                new Tweak
                {
                    Id = "optimize_network_adapter",
                    Title = "Optimize Network Adapter Priority",
                    Description = "Prioritizes network traffic for games and real-time applications by adjusting system priority settings. Standard network optimization.",
                    Category = "Network",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    RegistryName = "Priority",
                    OptimizedValue = 6,
                    DefaultValue = 2
                },

                // === POWER TWEAKS ===
                new Tweak
                {
                    Id = "high_performance_power",
                    Title = "Set High Performance Power Plan",
                    Description = "Configures Windows to use maximum performance settings. CPU will run at higher clocks more often. May increase power consumption and heat.",
                    Category = "Power",
                    WindowsVersion = "10/11",
                    Warning = "Increases power consumption and may generate more heat. Desktop users and gaming laptops benefit most.",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes",
                    RegistryName = "ActivePowerScheme",
                    OptimizedValue = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",
                    DefaultValue = "381b4222-f694-41f0-9685-ff5bb260df2e"
                },
                new Tweak
                {
                    Id = "disable_usb_suspend",
                    Title = "Disable USB Selective Suspend",
                    Description = "Prevents Windows from suspending USB devices to save power. Can fix issues with USB devices disconnecting or not waking properly.",
                    Category = "Power",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Services\USB",
                    RegistryName = "DisableSelectiveSuspend",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },
                new Tweak
                {
                    Id = "disable_power_throttling",
                    Title = "Disable Power Throttling",
                    Description = "Prevents Windows from throttling background applications to save power. Ensures all applications run at full performance.",
                    Category = "Power",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling",
                    RegistryName = "PowerThrottlingOff",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },

                // === SECURITY TWEAKS ===
                new Tweak
                {
                    Id = "enable_uac",
                    Title = "Keep UAC Enabled",
                    Description = "Ensures User Account Control remains enabled. UAC is a critical security feature that prevents unauthorized changes. This verifies it's not disabled.",
                    Category = "Security",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    RegistryName = "EnableLUA",
                    OptimizedValue = 1,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_remote_assistance",
                    Title = "Disable Remote Assistance",
                    Description = "Disables Windows Remote Assistance feature which allows others to connect to your PC. Improves security if you don't use this feature.",
                    Category = "Security",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Remote Assistance",
                    RegistryName = "fAllowToGetHelp",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_autorun",
                    Title = "Disable AutoRun/AutoPlay",
                    Description = "Disables automatic execution of programs from removable media. Important security measure against USB-based malware. Strongly recommended.",
                    Category = "Security",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                    RegistryName = "NoDriveTypeAutoRun",
                    OptimizedValue = 255,
                    DefaultValue = 145
                },

                // === STORAGE TWEAKS ===
                new Tweak
                {
                    Id = "disable_storage_sense",
                    Title = "Disable Storage Sense Auto-Cleanup",
                    Description = "Prevents Windows from automatically deleting temporary files and emptying the recycle bin. Gives you full control over what gets deleted.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy",
                    RegistryName = "01",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_reserved_storage",
                    Title = "Disable Reserved Storage",
                    Description = "Disables the 7GB+ of reserved storage Windows keeps for updates. Frees up disk space but updates may require manual space management.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    Warning = "May require manual disk space management for large Windows updates. Ensure you have at least 10GB free before major updates.",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager",
                    RegistryName = "ShippedWithReserves",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "optimize_ntfs",
                    Title = "Optimize NTFS Performance",
                    Description = "Disables last access time updates on files which can improve disk performance, especially on HDDs. Safe optimization used by many power users.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\FileSystem",
                    RegistryName = "NtfsDisableLastAccessUpdate",
                    OptimizedValue = 1,
                    DefaultValue = 0
                }
            };
        }

        public void ApplyTweak(Tweak tweak)
        {
            try
            {
                using var key = Registry.LocalMachine.CreateSubKey(tweak.RegistryPath);
                if (key != null && tweak.OptimizedValue != null)
                {
                    if (tweak.OptimizedValue is int intValue)
                        key.SetValue(tweak.RegistryName, intValue, RegistryValueKind.DWord);
                    else if (tweak.OptimizedValue is string strValue)
                        key.SetValue(tweak.RegistryName, strValue, RegistryValueKind.String);
                }
                tweak.IsActive = true;
            }
            catch (Exception)
            {
                // Handle or log error
            }
        }

        public void RevertTweak(Tweak tweak)
        {
            try
            {
                using var key = Registry.LocalMachine.CreateSubKey(tweak.RegistryPath);
                if (key != null)
                {
                    if (tweak.DefaultValue == null)
                    {
                        key.DeleteValue(tweak.RegistryName, false);
                    }
                    else if (tweak.DefaultValue is int intValue)
                    {
                        key.SetValue(tweak.RegistryName, intValue, RegistryValueKind.DWord);
                    }
                    else if (tweak.DefaultValue is string strValue)
                    {
                        key.SetValue(tweak.RegistryName, strValue, RegistryValueKind.String);
                    }
                }
                tweak.IsActive = false;
            }
            catch (Exception)
            {
                // Handle or log error
            }
        }

        public void CheckTweakStatus(Tweak tweak)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(tweak.RegistryPath);
                if (key != null)
                {
                    var currentValue = key.GetValue(tweak.RegistryName);
                    tweak.IsActive = currentValue?.Equals(tweak.OptimizedValue) ?? false;
                }
            }
            catch
            {
                tweak.IsActive = false;
            }
        }
    }
}
