using System.Diagnostics;
using AIO_PC_Tool_v2.Models;

namespace AIO_PC_Tool_v2.Services;

public interface IUtilitiesService
{
    List<UtilityAction> GetUtilities();
    Task<(bool success, string output)> RunUtilityAsync(string actionId);
    List<UtilityAction> GetUtilitiesByCategory(string category);
}

public class UtilitiesService : IUtilitiesService
{
    private readonly List<UtilityAction> _utilities;

    public UtilitiesService()
    {
        _utilities = InitializeUtilities();
    }

    public List<UtilityAction> GetUtilities() => _utilities;
    
    public List<UtilityAction> GetUtilitiesByCategory(string category) =>
        _utilities.Where(u => u.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

    public async Task<(bool success, string output)> RunUtilityAsync(string actionId)
    {
        var utility = _utilities.FirstOrDefault(u => u.Id == actionId);
        if (utility == null) return (false, "Utility not found");

        try
        {
            if (utility.Id == "ctt-utility")
            {
                // Chris Titus Tech Utility - Run in elevated PowerShell
                Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-ExecutionPolicy Bypass -Command \"Start-Process powershell -ArgumentList '-ExecutionPolicy Bypass -Command \\\"iwr -useb https://christitus.com/win | iex\\\"' -Verb RunAs\"",
                    UseShellExecute = true,
                    CreateNoWindow = false
                });
                return (true, "Chris Titus Tech Utility launched in admin PowerShell");
            }
            else if (utility.OpensTerminal)
            {
                // Open in new admin terminal window
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/k {utility.Command}",
                    UseShellExecute = true,
                    Verb = "runas"
                });
                return (true, "Opened in new administrator terminal");
            }
            else if (utility.IsPowerShell)
            {
                // Run PowerShell command
                var result = await RunPowerShellAsync(utility.Command);
                return (true, string.IsNullOrEmpty(result) ? "Completed successfully" : result);
            }
            else
            {
                // Run directly
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c {utility.Command}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                
                var result = string.IsNullOrEmpty(output) ? error : output;
                return (process.ExitCode == 0, string.IsNullOrEmpty(result) ? "Completed successfully" : result);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}");
        }
    }

    private static async Task<string> RunPowerShellAsync(string script)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NonInteractive -NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return output;
    }

    private static List<UtilityAction> InitializeUtilities()
    {
        return new List<UtilityAction>
        {
            // ══════════════════════════════════════════════════════════════════════════
            // RECOMMENDED TOOLS
            // ══════════════════════════════════════════════════════════════════════════
            new()
            {
                Id = "ctt-utility",
                Name = "Chris Titus Tech Utility",
                Description = "Comprehensive Windows optimization tool by Chris Titus Tech. Opens in admin PowerShell.",
                Category = "recommended",
                Command = "iwr -useb https://christitus.com/win | iex",
                Icon = "Star",
                IsPowerShell = true
            },

            // ══════════════════════════════════════════════════════════════════════════
            // SYSTEM REPAIR
            // ══════════════════════════════════════════════════════════════════════════
            new()
            {
                Id = "sfc",
                Name = "System File Checker (SFC)",
                Description = "Scans and repairs corrupted Windows system files. Takes 10-30 minutes.",
                Category = "repair",
                Command = "sfc /scannow",
                Icon = "Build",
                OpensTerminal = true
            },
            new()
            {
                Id = "dism-health",
                Name = "DISM Health Check",
                Description = "Checks Windows image health without making repairs.",
                Category = "repair",
                Command = "DISM /Online /Cleanup-Image /CheckHealth",
                Icon = "Search",
                OpensTerminal = true
            },
            new()
            {
                Id = "dism-restore",
                Name = "DISM Restore Health",
                Description = "Repairs Windows image using Windows Update. Takes 15-45 minutes.",
                Category = "repair",
                Command = "DISM /Online /Cleanup-Image /RestoreHealth",
                Icon = "Healing",
                OpensTerminal = true
            },
            new()
            {
                Id = "checkdisk",
                Name = "Check Disk (CHKDSK)",
                Description = "Scans disk for errors and bad sectors. Requires restart.",
                Category = "repair",
                Command = "echo Y | chkdsk C: /f /r",
                Icon = "Storage",
                OpensTerminal = true,
                RequiresRestart = true
            },

            // ══════════════════════════════════════════════════════════════════════════
            // NETWORK TOOLS
            // ══════════════════════════════════════════════════════════════════════════
            new()
            {
                Id = "flush-dns",
                Name = "Flush DNS Cache",
                Description = "Clears the local DNS resolver cache. Fixes some connection issues.",
                Category = "network",
                Command = "ipconfig /flushdns",
                Icon = "Dns"
            },
            new()
            {
                Id = "network-reset",
                Name = "Full Network Reset",
                Description = "Resets all network adapters, Winsock, and IP settings. Requires restart.",
                Category = "network",
                Command = "netsh winsock reset && netsh int ip reset && ipconfig /flushdns && ipconfig /release && ipconfig /renew",
                Icon = "SettingsEthernet",
                OpensTerminal = true,
                RequiresRestart = true
            },
            new()
            {
                Id = "release-ip",
                Name = "Release IP Address",
                Description = "Releases current IP address from DHCP.",
                Category = "network",
                Command = "ipconfig /release",
                Icon = "LinkOff"
            },
            new()
            {
                Id = "renew-ip",
                Name = "Renew IP Address",
                Description = "Requests a new IP address from DHCP server.",
                Category = "network",
                Command = "ipconfig /renew",
                Icon = "Link"
            },
            new()
            {
                Id = "reset-winsock",
                Name = "Reset Winsock",
                Description = "Resets Windows Sockets. Fixes network issues caused by corrupt stack.",
                Category = "network",
                Command = "netsh winsock reset",
                Icon = "Refresh",
                RequiresRestart = true
            },

            // ══════════════════════════════════════════════════════════════════════════
            // WINDOWS TOOLS
            // ══════════════════════════════════════════════════════════════════════════
            new()
            {
                Id = "disk-cleanup",
                Name = "Disk Cleanup",
                Description = "Opens Windows Disk Cleanup utility.",
                Category = "tools",
                Command = "cleanmgr",
                Icon = "CleaningServices"
            },
            new()
            {
                Id = "disk-cleanup-all",
                Name = "Disk Cleanup (All Drives)",
                Description = "Opens advanced Disk Cleanup with system files option.",
                Category = "tools",
                Command = "cleanmgr /sageset:1",
                Icon = "CleaningServices"
            },
            new()
            {
                Id = "defrag",
                Name = "Optimize Drives",
                Description = "Opens drive optimization tool (defrag for HDD, TRIM for SSD).",
                Category = "tools",
                Command = "dfrgui",
                Icon = "Speed"
            },
            new()
            {
                Id = "restart-explorer",
                Name = "Restart Explorer",
                Description = "Restarts Windows Explorer (taskbar and desktop). Quick UI refresh.",
                Category = "tools",
                Command = "Stop-Process -Name explorer -Force; Start-Sleep -Milliseconds 1500; Start-Process explorer",
                Icon = "Refresh",
                IsPowerShell = true
            },
            new()
            {
                Id = "system-restore",
                Name = "System Restore",
                Description = "Opens Windows System Restore wizard.",
                Category = "tools",
                Command = "rstrui",
                Icon = "Restore"
            },
            new()
            {
                Id = "device-manager",
                Name = "Device Manager",
                Description = "Opens Device Manager to manage hardware drivers.",
                Category = "tools",
                Command = "devmgmt.msc",
                Icon = "Memory"
            },
            new()
            {
                Id = "event-viewer",
                Name = "Event Viewer",
                Description = "Opens Windows Event Viewer for system logs.",
                Category = "tools",
                Command = "eventvwr.msc",
                Icon = "List"
            },
            new()
            {
                Id = "services",
                Name = "Services Manager",
                Description = "Opens Windows Services management console.",
                Category = "tools",
                Command = "services.msc",
                Icon = "Settings"
            },

            // ══════════════════════════════════════════════════════════════════════════
            // POWER MANAGEMENT
            // ══════════════════════════════════════════════════════════════════════════
            new()
            {
                Id = "power-options",
                Name = "Power Options",
                Description = "Opens Windows Power Options control panel.",
                Category = "power",
                Command = "powercfg.cpl",
                Icon = "BatteryFull"
            },
            new()
            {
                Id = "power-report",
                Name = "Generate Power Report",
                Description = "Creates a detailed battery/power efficiency report.",
                Category = "power",
                Command = "powercfg /energy /output \"%USERPROFILE%\\Desktop\\power_report.html\" && start %USERPROFILE%\\Desktop\\power_report.html",
                Icon = "Assessment",
                OpensTerminal = true
            },
            new()
            {
                Id = "battery-report",
                Name = "Generate Battery Report",
                Description = "Creates detailed battery health report (laptops only).",
                Category = "power",
                Command = "powercfg /batteryreport /output \"%USERPROFILE%\\Desktop\\battery_report.html\" && start %USERPROFILE%\\Desktop\\battery_report.html",
                Icon = "Battery",
                OpensTerminal = true
            },

            // ══════════════════════════════════════════════════════════════════════════
            // QUICK ACTIONS
            // ══════════════════════════════════════════════════════════════════════════
            new()
            {
                Id = "temp-folder",
                Name = "Open Temp Folder",
                Description = "Opens Windows temporary files folder.",
                Category = "quick",
                Command = "explorer %TEMP%",
                Icon = "Folder"
            },
            new()
            {
                Id = "startup-folder",
                Name = "Open Startup Folder",
                Description = "Opens user startup programs folder.",
                Category = "quick",
                Command = "explorer shell:startup",
                Icon = "Folder"
            },
            new()
            {
                Id = "apps-features",
                Name = "Apps & Features",
                Description = "Opens installed programs list for uninstallation.",
                Category = "quick",
                Command = "start ms-settings:appsfeatures",
                Icon = "Apps"
            },
            new()
            {
                Id = "windows-update",
                Name = "Windows Update",
                Description = "Opens Windows Update settings.",
                Category = "quick",
                Command = "start ms-settings:windowsupdate",
                Icon = "Update"
            },
        };
    }
}

// Extended UtilityAction model
public class UtilityAction
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public bool RequiresRestart { get; set; }
    public bool OpensTerminal { get; set; }
    public bool IsPowerShell { get; set; }
}
