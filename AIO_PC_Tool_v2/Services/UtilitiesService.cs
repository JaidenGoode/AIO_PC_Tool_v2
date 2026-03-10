using System.Diagnostics;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Services
{
    public class UtilitiesService
    {
        public ObservableCollection<UtilityItem> GetUtilities()
        {
            return new ObservableCollection<UtilityItem>
            {
                new UtilityItem
                {
                    Name = "System File Checker",
                    Description = "Scans and repairs Windows system files (sfc /scannow)",
                    Icon = "ShieldCheck",
                    Category = "Repair",
                    Action = () => RunCommand("sfc", "/scannow")
                },
                new UtilityItem
                {
                    Name = "DISM Health Check",
                    Description = "Checks Windows image health",
                    Icon = "Magnify",
                    Category = "Repair",
                    Action = () => RunCommand("DISM", "/Online /Cleanup-Image /ScanHealth")
                },
                new UtilityItem
                {
                    Name = "DISM Restore Health",
                    Description = "Repairs Windows image using Windows Update",
                    Icon = "Wrench",
                    Category = "Repair",
                    Action = () => RunCommand("DISM", "/Online /Cleanup-Image /RestoreHealth")
                },
                new UtilityItem
                {
                    Name = "Check Disk",
                    Description = "Scans drive for errors (chkdsk)",
                    Icon = "Harddisk",
                    Category = "Repair",
                    Action = () => RunCommand("chkdsk", "C: /F /R")
                },
                new UtilityItem
                {
                    Name = "Flush DNS Cache",
                    Description = "Clears the DNS resolver cache",
                    Icon = "Dns",
                    Category = "Network",
                    Action = () => RunCommand("ipconfig", "/flushdns")
                },
                new UtilityItem
                {
                    Name = "Reset Network Stack",
                    Description = "Resets Winsock and TCP/IP stack",
                    Icon = "LanConnect",
                    Category = "Network",
                    Action = () =>
                    {
                        RunCommand("netsh", "winsock reset");
                        RunCommand("netsh", "int ip reset");
                    }
                },
                new UtilityItem
                {
                    Name = "Disk Cleanup",
                    Description = "Opens Windows Disk Cleanup utility",
                    Icon = "Broom",
                    Category = "Cleanup",
                    Action = () => Process.Start("cleanmgr.exe")
                },
                new UtilityItem
                {
                    Name = "Defragment Drive",
                    Description = "Opens Windows defragmentation tool",
                    Icon = "ChartDonut",
                    Category = "Optimization",
                    Action = () => Process.Start("dfrgui.exe")
                }
            };
        }

        public void RunChrisTitusUtility()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"iwr -useb https://christitus.com/win | iex\"",
                Verb = "runas",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void RunCommand(string fileName, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                Verb = "runas",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
