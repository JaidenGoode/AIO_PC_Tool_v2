using System.IO;
using Microsoft.Win32;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Services
{
    public class TweakService
    {
        public List<Tweak> GetAllTweaks()
        {
            return new List<Tweak>
            {
                // ============ GAMING TWEAKS ============
                new Tweak
                {
                    Id = "game_mode",
                    Title = "Enable Game Mode",
                    Description = "Windows Game Mode prioritizes game processes and reduces background activity during gameplay for better FPS.",
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
                    Description = "Removes the Xbox overlay that can cause FPS drops and stuttering in games. Use third-party alternatives instead.",
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
                    Title = "Disable Background Recording",
                    Description = "Stops Windows from recording gameplay in the background. Frees up GPU resources for better performance.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR",
                    RegistryName = "GameDVR_Enabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "hardware_gpu_scheduling",
                    Title = "Hardware GPU Scheduling",
                    Description = "Lets your GPU manage its own memory scheduling. Reduces input lag and improves frame times. Requires NVIDIA 10-series+ or AMD 5000+.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    Warning = "Requires compatible GPU. May cause issues on older hardware.",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\GraphicsDrivers",
                    RegistryName = "HwSchMode",
                    OptimizedValue = 2,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_fullscreen_optimizations",
                    Title = "Disable Fullscreen Optimizations",
                    Description = "Prevents Windows from applying compatibility features to fullscreen games. Reduces input lag in competitive games.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameConfigStore",
                    RegistryName = "GameDVR_FSEBehavior",
                    OptimizedValue = 2,
                    DefaultValue = 0
                },
                new Tweak
                {
                    Id = "optimize_gaming_priority",
                    Title = "Game Process Priority",
                    Description = "Increases CPU priority for games, ensuring smoother gameplay during background tasks.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    RegistryName = "Priority",
                    OptimizedValue = 6,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "gpu_priority",
                    Title = "GPU Priority for Games",
                    Description = "Allocates more GPU resources to games for consistent frame rates.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    RegistryName = "GPU Priority",
                    OptimizedValue = 8,
                    DefaultValue = 2
                },

                // ============ PERFORMANCE TWEAKS ============
                new Tweak
                {
                    Id = "disable_superfetch",
                    Title = "Disable SysMain (Superfetch)",
                    Description = "Stops Windows from preloading apps into RAM. Recommended for SSD users to reduce disk writes and free memory.",
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
                    Description = "Disables app prefetching. Minimal benefit on SSDs, can improve boot times.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    RegistryName = "EnablePrefetcher",
                    OptimizedValue = 0,
                    DefaultValue = 3
                },
                new Tweak
                {
                    Id = "disable_hibernation",
                    Title = "Disable Hibernation",
                    Description = "Disables hibernation and removes hiberfil.sys, freeing up disk space equal to your RAM. Sleep still works.",
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
                    Description = "Disables hybrid shutdown. Fixes boot issues and is recommended for dual-boot systems.",
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
                    Title = "Optimize CPU Scheduling",
                    Description = "Prioritizes foreground applications for snappier response times.",
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
                    Description = "Disables window transparency and blur. Improves performance on older hardware.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    RegistryName = "EnableTransparency",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_animations",
                    Title = "Reduce Visual Animations",
                    Description = "Minimizes window animations for faster UI response. Makes Windows feel snappier.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"Control Panel\Desktop\WindowMetrics",
                    RegistryName = "MinAnimate",
                    OptimizedValue = "0",
                    DefaultValue = "1"
                },

                // ============ PRIVACY TWEAKS ============
                new Tweak
                {
                    Id = "disable_telemetry",
                    Title = "Disable Telemetry",
                    Description = "Prevents Windows from sending diagnostic and usage data to Microsoft. Improves privacy.",
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
                    Description = "Stops apps from tracking you with a unique advertising identifier.",
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
                    Description = "Stops Windows from collecting your activity history and syncing it to Microsoft.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Policies\Microsoft\Windows\System",
                    RegistryName = "EnableActivityFeed",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_feedback",
                    Title = "Disable Feedback Requests",
                    Description = "Stops Windows from asking for feedback. Reduces interruptions.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Siuf\Rules",
                    RegistryName = "NumberOfSIUFInPeriod",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_suggestions",
                    Title = "Disable App Suggestions",
                    Description = "Removes suggested apps and ads from the Start menu.",
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
                    Description = "Stops Microsoft from personalizing tips and ads based on your data.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy",
                    RegistryName = "TailoredExperiencesWithDiagnosticDataEnabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },

                // ============ NETWORK TWEAKS ============
                new Tweak
                {
                    Id = "disable_nagle",
                    Title = "Disable Nagle's Algorithm",
                    Description = "Reduces network latency by disabling TCP packet batching. Great for online gaming.",
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
                    Description = "Removes bandwidth throttling for multimedia applications. Improves streaming and downloads.",
                    Category = "Network",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    RegistryName = "NetworkThrottlingIndex",
                    OptimizedValue = -1,
                    DefaultValue = 10
                },

                // ============ VISUAL TWEAKS ============
                new Tweak
                {
                    Id = "show_file_extensions",
                    Title = "Show File Extensions",
                    Description = "Shows file extensions for all files. Helps identify file types and avoid malware.",
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
                    Description = "Shows hidden files and folders in File Explorer.",
                    Category = "Visual",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryName = "Hidden",
                    OptimizedValue = 1,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "classic_context_menu",
                    Title = "Classic Context Menu",
                    Description = "Restores the full right-click menu in Windows 11 without clicking 'Show more options'.",
                    Category = "Visual",
                    WindowsVersion = "11",
                    RegistryPath = @"SOFTWARE\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32",
                    RegistryName = "",
                    OptimizedValue = "",
                    DefaultValue = null
                },

                // ============ POWER TWEAKS ============
                new Tweak
                {
                    Id = "disable_usb_suspend",
                    Title = "Disable USB Selective Suspend",
                    Description = "Prevents USB devices from being suspended. Fixes USB disconnection issues.",
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
                    Description = "Prevents Windows from throttling background apps. Ensures full performance.",
                    Category = "Power",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling",
                    RegistryName = "PowerThrottlingOff",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },

                // ============ SECURITY TWEAKS ============
                new Tweak
                {
                    Id = "disable_autorun",
                    Title = "Disable AutoRun",
                    Description = "Prevents automatic execution from USB drives. Important security measure.",
                    Category = "Security",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                    RegistryName = "NoDriveTypeAutoRun",
                    OptimizedValue = 255,
                    DefaultValue = 145
                },
                new Tweak
                {
                    Id = "disable_remote_assistance",
                    Title = "Disable Remote Assistance",
                    Description = "Disables Windows Remote Assistance feature for better security.",
                    Category = "Security",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Remote Assistance",
                    RegistryName = "fAllowToGetHelp",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },

                // ============ STORAGE TWEAKS ============
                new Tweak
                {
                    Id = "disable_storage_sense",
                    Title = "Disable Storage Sense",
                    Description = "Prevents automatic file deletion. Gives you full control over your files.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy",
                    RegistryName = "01",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "optimize_ntfs",
                    Title = "Optimize NTFS",
                    Description = "Disables last access time updates for better disk performance.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\FileSystem",
                    RegistryName = "NtfsDisableLastAccessUpdate",
                    OptimizedValue = 1,
                    DefaultValue = 0
                }
            };
        }

        public bool CheckTweakStatus(Tweak tweak)
        {
            try
            {
                // Try HKLM first
                using var key = Registry.LocalMachine.OpenSubKey(tweak.RegistryPath);
                if (key != null)
                {
                    var currentValue = key.GetValue(tweak.RegistryName);
                    if (currentValue != null)
                    {
                        if (tweak.OptimizedValue is int intOpt && currentValue is int intCur)
                        {
                            tweak.IsActive = intCur == intOpt;
                            return tweak.IsActive;
                        }
                        else if (tweak.OptimizedValue is string strOpt)
                        {
                            tweak.IsActive = currentValue.ToString() == strOpt;
                            return tweak.IsActive;
                        }
                    }
                }

                // Try HKCU
                using var keyUser = Registry.CurrentUser.OpenSubKey(tweak.RegistryPath);
                if (keyUser != null)
                {
                    var currentValue = keyUser.GetValue(tweak.RegistryName);
                    if (currentValue != null)
                    {
                        if (tweak.OptimizedValue is int intOpt && currentValue is int intCur)
                        {
                            tweak.IsActive = intCur == intOpt;
                            return tweak.IsActive;
                        }
                        else if (tweak.OptimizedValue is string strOpt)
                        {
                            tweak.IsActive = currentValue.ToString() == strOpt;
                            return tweak.IsActive;
                        }
                    }
                }

                tweak.IsActive = false;
            }
            catch
            {
                tweak.IsActive = false;
            }
            return tweak.IsActive;
        }

        public bool ApplyTweak(Tweak tweak)
        {
            try
            {
                // Try HKLM first
                using var key = Registry.LocalMachine.CreateSubKey(tweak.RegistryPath);
                if (key != null && tweak.OptimizedValue != null)
                {
                    if (tweak.OptimizedValue is int intValue)
                        key.SetValue(tweak.RegistryName, intValue, RegistryValueKind.DWord);
                    else if (tweak.OptimizedValue is string strValue)
                        key.SetValue(tweak.RegistryName, strValue, RegistryValueKind.String);
                    
                    tweak.IsActive = true;
                    return true;
                }
            }
            catch
            {
                // Try HKCU as fallback
                try
                {
                    using var key = Registry.CurrentUser.CreateSubKey(tweak.RegistryPath);
                    if (key != null && tweak.OptimizedValue != null)
                    {
                        if (tweak.OptimizedValue is int intValue)
                            key.SetValue(tweak.RegistryName, intValue, RegistryValueKind.DWord);
                        else if (tweak.OptimizedValue is string strValue)
                            key.SetValue(tweak.RegistryName, strValue, RegistryValueKind.String);
                        
                        tweak.IsActive = true;
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public bool RevertTweak(Tweak tweak)
        {
            try
            {
                // Try HKLM first
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
                    
                    tweak.IsActive = false;
                    return true;
                }
            }
            catch
            {
                // Try HKCU as fallback
                try
                {
                    using var key = Registry.CurrentUser.CreateSubKey(tweak.RegistryPath);
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
                        
                        tweak.IsActive = false;
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public int GetActiveTweakCount(List<Tweak> tweaks)
        {
            return tweaks.Count(t => t.IsActive);
        }
    }
}
