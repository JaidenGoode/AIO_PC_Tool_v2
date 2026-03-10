using System.IO;
using AIO_PC_Tool_v2.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;

namespace AIO_PC_Tool_v2.Services
{
    public class DnsService
    {
        public ObservableCollection<DnsProvider> GetProviders()
        {
            return new ObservableCollection<DnsProvider>
            {
                new DnsProvider { Name = "Cloudflare", Description = "Fast and privacy-focused", PrimaryDns = "1.1.1.1", SecondaryDns = "1.0.0.1", Icon = "Cloud" },
                new DnsProvider { Name = "Google DNS", Description = "Reliable and widely used", PrimaryDns = "8.8.8.8", SecondaryDns = "8.8.4.4", Icon = "Google" },
                new DnsProvider { Name = "OpenDNS", Description = "Security and filtering options", PrimaryDns = "208.67.222.222", SecondaryDns = "208.67.220.220", Icon = "Shield" },
                new DnsProvider { Name = "Quad9", Description = "Security-focused, blocks malware", PrimaryDns = "9.9.9.9", SecondaryDns = "149.112.112.112", Icon = "ShieldLock" },
                new DnsProvider { Name = "AdGuard DNS", Description = "Blocks ads and trackers", PrimaryDns = "94.140.14.14", SecondaryDns = "94.140.15.15", Icon = "ShieldOff" }
            };
        }

        public void SetDns(DnsProvider provider)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = $"interface ip set dns name=\"Ethernet\" static {provider.PrimaryDns}",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true
                };
                Process.Start(psi);

                psi.Arguments = $"interface ip add dns name=\"Ethernet\" {provider.SecondaryDns} index=2";
                Process.Start(psi);
            }
            catch { }
        }

        public void ResetToAutomatic()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "interface ip set dns name=\"Ethernet\" dhcp",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true
                };
                Process.Start(psi);
            }
            catch { }
        }
    }

    public class RestorePointService
    {
        public ObservableCollection<RestorePoint> GetRestorePoints()
        {
            var points = new ObservableCollection<RestorePoint>();

            try
            {
                using var searcher = new ManagementObjectSearcher("root\\default", "SELECT * FROM SystemRestore");
                foreach (ManagementObject obj in searcher.Get())
                {
                    points.Add(new RestorePoint
                    {
                        Description = obj["Description"]?.ToString() ?? "Unknown",
                        CreationTime = ManagementDateTimeConverter.ToDateTime(obj["CreationTime"]?.ToString() ?? ""),
                        SequenceNumber = Convert.ToInt32(obj["SequenceNumber"])
                    });
                }
            }
            catch { }

            return points;
        }

        public void CreateRestorePoint(string description)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -Command \"Checkpoint-Computer -Description '{description}' -RestorePointType MODIFY_SETTINGS\"",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true
                };
                Process.Start(psi);
            }
            catch { }
        }
    }
}
