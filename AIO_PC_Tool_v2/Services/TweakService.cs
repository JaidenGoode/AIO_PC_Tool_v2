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
                    Description = "Windows Game Mode prioritizes game processes and reduces background activity during gameplay for better FPS and lower input lag.",
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
                    Description = "Removes the Xbox overlay that can cause FPS drops, stuttering, and increased input latency in games.",
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
                    Description = "Stops Windows from recording gameplay in the background. Frees up GPU resources and reduces CPU overhead.",
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
                    Title = "Hardware-Accelerated GPU Scheduling",
                    Description = "Lets your GPU manage its own memory scheduling directly. Reduces input lag and improves frame times significantly.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    Warning = "Requires NVIDIA 10-series+ / AMD 5000+ / Intel Gen 10+",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\GraphicsDrivers",
                    RegistryName = "HwSchMode",
                    OptimizedValue = 2,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_fullscreen_optimizations",
                    Title = "Disable Fullscreen Optimizations",
                    Description = "Prevents Windows from applying DWM composition to fullscreen games. Reduces input lag in competitive games.",
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
                    Title = "Boost Game Process Priority",
                    Description = "Increases CPU scheduling priority for games, ensuring smoother gameplay even with background tasks.",
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
                    Title = "Maximum GPU Priority for Games",
                    Description = "Allocates maximum GPU resources to games for consistent, high frame rates.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    RegistryName = "GPU Priority",
                    OptimizedValue = 8,
                    DefaultValue = 2
                },
                new Tweak
                {
                    Id = "scheduling_category",
                    Title = "High-Performance Scheduling",
                    Description = "Sets games to use high-performance CPU scheduling category for better responsiveness.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    RegistryName = "Scheduling Category",
                    OptimizedValue = "High",
                    DefaultValue = "Medium"
                },
                new Tweak
                {
                    Id = "sfio_priority",
                    Title = "High-Priority SFIO for Games",
                    Description = "Increases I/O priority for game processes, reducing load times and stuttering.",
                    Category = "Gaming",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    RegistryName = "SFIO Priority",
                    OptimizedValue = "High",
                    DefaultValue = "Normal"
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
                    Description = "Disables application prefetching. Can improve boot times and reduce disk activity on SSDs.",
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
                    Description = "Disables hibernation and removes hiberfil.sys, freeing disk space equal to your RAM. Sleep still works.",
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
                    Description = "Disables hybrid shutdown. Fixes boot issues and is recommended for dual-boot systems or when experiencing driver problems.",
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
                    Description = "Prioritizes foreground applications for snappier response times and better gaming performance.",
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
                    Description = "Disables window transparency and blur effects. Improves performance especially on integrated graphics.",
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
                    Description = "Minimizes window animations for faster UI response. Makes Windows feel significantly snappier.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"Control Panel\Desktop\WindowMetrics",
                    RegistryName = "MinAnimate",
                    OptimizedValue = "0",
                    DefaultValue = "1"
                },
                new Tweak
                {
                    Id = "system_responsiveness",
                    Title = "Maximize System Responsiveness",
                    Description = "Reduces CPU time reserved for background tasks from 20% to 10% for better foreground performance.",
                    Category = "Performance",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    RegistryName = "SystemResponsiveness",
                    OptimizedValue = 10,
                    DefaultValue = 20
                },

                // ============ PRIVACY TWEAKS ============
                new Tweak
                {
                    Id = "disable_telemetry",
                    Title = "Disable Telemetry",
                    Description = "Prevents Windows from sending diagnostic and usage data to Microsoft. Improves privacy and reduces background activity.",
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
                    Description = "Stops apps from tracking you with a unique advertising identifier for targeted ads.",
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
                    Description = "Stops Windows from collecting your activity history and syncing it to Microsoft cloud.",
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
                    Description = "Stops Windows from asking for feedback. Reduces interruptions and popups.",
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
                    Description = "Removes suggested apps and promotional content from the Start menu.",
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
                    Description = "Stops Microsoft from personalizing tips, ads and recommendations based on your data.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy",
                    RegistryName = "TailoredExperiencesWithDiagnosticDataEnabled",
                    OptimizedValue = 0,
                    DefaultValue = 1
                },
                new Tweak
                {
                    Id = "disable_location_tracking",
                    Title = "Disable Location Tracking",
                    Description = "Prevents Windows and apps from accessing your location data.",
                    Category = "Privacy",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location",
                    RegistryName = "Value",
                    OptimizedValue = "Deny",
                    DefaultValue = "Allow"
                },

                // ============ NETWORK TWEAKS ============
                new Tweak
                {
                    Id = "disable_nagle",
                    Title = "Disable Nagle's Algorithm",
                    Description = "Reduces network latency by disabling TCP packet batching. Essential for online gaming and real-time applications.",
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
                    Description = "Removes Windows bandwidth throttling for multimedia applications. Improves streaming and download speeds.",
                    Category = "Network",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    RegistryName = "NetworkThrottlingIndex",
                    OptimizedValue = -1,
                    DefaultValue = 10
                },
                new Tweak
                {
                    Id = "enable_large_mtu",
                    Title = "Enable Large MTU Packets",
                    Description = "Enables larger network packets for improved throughput on high-speed connections.",
                    Category = "Network",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                    RegistryName = "EnablePMTUDiscovery",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },

                // ============ VISUAL TWEAKS ============
                new Tweak
                {
                    Id = "show_file_extensions",
                    Title = "Show File Extensions",
                    Description = "Shows file extensions for all files. Helps identify file types and avoid malware disguised as documents.",
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
                    Description = "Shows hidden files and folders in File Explorer for better system visibility.",
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
                    Description = "Prevents USB devices from being suspended. Fixes USB disconnection issues with gaming peripherals.",
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
                    Description = "Prevents Windows from throttling background applications. Ensures full CPU performance at all times.",
                    Category = "Power",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling",
                    RegistryName = "PowerThrottlingOff",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },
                new Tweak
                {
                    Id = "disable_core_parking",
                    Title = "Disable CPU Core Parking",
                    Description = "Keeps all CPU cores active instead of parking unused cores. Improves multi-threaded performance.",
                    Category = "Power",
                    WindowsVersion = "10/11",
                    Warning = "May increase power consumption on laptops",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    RegistryName = "ValueMax",
                    OptimizedValue = 0,
                    DefaultValue = 100
                },

                // ============ SECURITY TWEAKS ============
                new Tweak
                {
                    Id = "disable_autorun",
                    Title = "Disable AutoRun",
                    Description = "Prevents automatic execution from USB drives and CDs. Important security measure against malware.",
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
                    Description = "Disables Windows Remote Assistance feature for better security against unauthorized access.",
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
                    Description = "Prevents automatic file deletion. Gives you full control over your files and temporary data.",
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
                    Title = "Optimize NTFS Performance",
                    Description = "Disables last access time updates for better disk performance. Reduces unnecessary disk writes.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\FileSystem",
                    RegistryName = "NtfsDisableLastAccessUpdate",
                    OptimizedValue = 1,
                    DefaultValue = 0
                },
                new Tweak
                {
                    Id = "disable_8dot3_names",
                    Title = "Disable 8.3 Filename Creation",
                    Description = "Disables legacy DOS filename creation for faster file operations on NTFS drives.",
                    Category = "Storage",
                    WindowsVersion = "10/11",
                    RegistryPath = @"SYSTEM\CurrentControlSet\Control\FileSystem",
                    RegistryName = "NtfsDisable8dot3NameCreation",
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
                        tweak.IsActive = CompareValues(currentValue, tweak.OptimizedValue);
                        return tweak.IsActive;
                    }
                }

                // Try HKCU
                using var keyUser = Registry.CurrentUser.OpenSubKey(tweak.RegistryPath);
                if (keyUser != null)
                {
                    var currentValue = keyUser.GetValue(tweak.RegistryName);
                    if (currentValue != null)
                    {
                        tweak.IsActive = CompareValues(currentValue, tweak.OptimizedValue);
                        return tweak.IsActive;
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

        private bool CompareValues(object? current, object? optimized)
        {
            if (current == null || optimized == null) return false;

            // Handle different types
            if (optimized is int intOpt)
            {
                if (current is int intCur) return intCur == intOpt;
                if (int.TryParse(current.ToString(), out int parsed)) return parsed == intOpt;
            }
            else if (optimized is string strOpt)
            {
                return current.ToString()?.Equals(strOpt, StringComparison.OrdinalIgnoreCase) ?? false;
            }

            return current.Equals(optimized);
        }

        public bool ApplyTweak(Tweak tweak)
        {
            try
            {
                // Try HKLM first (requires admin)
                try
                {
                    using var key = Registry.LocalMachine.CreateSubKey(tweak.RegistryPath);
                    if (key != null && tweak.OptimizedValue != null)
                    {
                        SetRegistryValue(key, tweak.RegistryName, tweak.OptimizedValue);
                        tweak.IsActive = true;
                        return true;
                    }
                }
                catch
                {
                    // Fall through to HKCU
                }

                // Try HKCU as fallback
                using var keyUser = Registry.CurrentUser.CreateSubKey(tweak.RegistryPath);
                if (keyUser != null && tweak.OptimizedValue != null)
                {
                    SetRegistryValue(keyUser, tweak.RegistryName, tweak.OptimizedValue);
                    tweak.IsActive = true;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool RevertTweak(Tweak tweak)
        {
            try
            {
                // Try HKLM first
                try
                {
                    using var key = Registry.LocalMachine.CreateSubKey(tweak.RegistryPath);
                    if (key != null)
                    {
                        if (tweak.DefaultValue == null)
                        {
                            key.DeleteValue(tweak.RegistryName, false);
                        }
                        else
                        {
                            SetRegistryValue(key, tweak.RegistryName, tweak.DefaultValue);
                        }
                        tweak.IsActive = false;
                        return true;
                    }
                }
                catch
                {
                    // Fall through to HKCU
                }

                // Try HKCU as fallback
                using var keyUser = Registry.CurrentUser.CreateSubKey(tweak.RegistryPath);
                if (keyUser != null)
                {
                    if (tweak.DefaultValue == null)
                    {
                        keyUser.DeleteValue(tweak.RegistryName, false);
                    }
                    else
                    {
                        SetRegistryValue(keyUser, tweak.RegistryName, tweak.DefaultValue);
                    }
                    tweak.IsActive = false;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        private void SetRegistryValue(RegistryKey key, string name, object value)
        {
            if (value is int intValue)
            {
                key.SetValue(name, intValue, RegistryValueKind.DWord);
            }
            else if (value is string strValue)
            {
                key.SetValue(name, strValue, RegistryValueKind.String);
            }
        }

        public int GetActiveTweakCount(List<Tweak> tweaks)
        {
            return tweaks.Count(t => t.IsActive);
        }
    }
}
