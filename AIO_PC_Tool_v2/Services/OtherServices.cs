using System.Diagnostics;
using AIO_PC_Tool_v2.Models;

namespace AIO_PC_Tool_v2.Services;

public interface IDnsService
{
    List<DnsProvider> GetProviders();
    Task<string?> GetCurrentProviderAsync();
    Task<bool> SetDnsAsync(string providerId);
}

public class DnsService : IDnsService
{
    private readonly List<DnsProvider> _providers;
    private readonly ISettingsService _settings;

    public DnsService(ISettingsService settings)
    {
        _settings = settings;
        _providers = new List<DnsProvider>
        {
            new() { Id = "cloudflare", Name = "Cloudflare", Description = "Fast, privacy-focused DNS (1.1.1.1)", PrimaryDns = "1.1.1.1", SecondaryDns = "1.0.0.1" },
            new() { Id = "cloudflare-malware", Name = "Cloudflare (Malware Block)", Description = "Blocks malware domains", PrimaryDns = "1.1.1.2", SecondaryDns = "1.0.0.2" },
            new() { Id = "google", Name = "Google", Description = "Reliable, fast DNS (8.8.8.8)", PrimaryDns = "8.8.8.8", SecondaryDns = "8.8.4.4" },
            new() { Id = "quad9", Name = "Quad9", Description = "Security-focused with threat blocking", PrimaryDns = "9.9.9.9", SecondaryDns = "149.112.112.112" },
            new() { Id = "opendns", Name = "OpenDNS", Description = "Cisco's DNS with phishing protection", PrimaryDns = "208.67.222.222", SecondaryDns = "208.67.220.220" },
            new() { Id = "nextdns", Name = "NextDNS", Description = "Customizable DNS with ad blocking", PrimaryDns = "45.90.28.0", SecondaryDns = "45.90.30.0" },
            new() { Id = "adguard", Name = "AdGuard", Description = "Blocks ads and trackers", PrimaryDns = "94.140.14.14", SecondaryDns = "94.140.15.15" },
            new() { Id = "default", Name = "Automatic (DHCP)", Description = "Use your ISP's default DNS", PrimaryDns = "Auto", SecondaryDns = "Auto" }
        };
    }

    public List<DnsProvider> GetProviders() => _providers;

    public async Task<string?> GetCurrentProviderAsync()
    {
        return await Task.FromResult(_settings.GetSetting("dns_provider"));
    }

    public async Task<bool> SetDnsAsync(string providerId)
    {
        var provider = _providers.FirstOrDefault(p => p.Id == providerId);
        if (provider == null) return false;

        try
        {
            string script;
            if (providerId == "default")
            {
                script = @"
Get-NetAdapter | Where-Object {$_.Status -eq 'Up' -and $_.InterfaceDescription -notmatch 'Virtual|VMware|VirtualBox|Hyper-V'} | ForEach-Object {
    Set-DnsClientServerAddress -InterfaceAlias $_.Name -ResetServerAddresses
}
ipconfig /flushdns";
            }
            else
            {
                script = $@"
Get-NetAdapter | Where-Object {{$_.Status -eq 'Up' -and $_.InterfaceDescription -notmatch 'Virtual|VMware|VirtualBox|Hyper-V'}} | ForEach-Object {{
    Set-DnsClientServerAddress -InterfaceAlias $_.Name -ServerAddresses ('{provider.PrimaryDns}','{provider.SecondaryDns}')
}}
ipconfig /flushdns";
            }

            await RunPowerShellAsync(script);
            _settings.SetSetting("dns_provider", providerId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<string> RunPowerShellAsync(string script)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NonInteractive -NoProfile -ExecutionPolicy Bypass -Command \"{script.Replace("\"", "\\\"")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return output;
    }
}

public interface IRestorePointService
{
    Task<List<RestorePoint>> GetRestorePointsAsync();
    Task<bool> CreateRestorePointAsync(string description);
}

public class RestorePointService : IRestorePointService
{
    public async Task<List<RestorePoint>> GetRestorePointsAsync()
    {
        var points = new List<RestorePoint>();
        
        try
        {
            var script = @"
Get-ComputerRestorePoint -ErrorAction SilentlyContinue | Select-Object Description, @{N='CreationTime';E={$_.ConvertToDateTime($_.CreationTime)}}, SequenceNumber | ConvertTo-Json -Compress
";
            var output = await RunPowerShellAsync(script);
            
            if (!string.IsNullOrWhiteSpace(output) && output.Trim().StartsWith("["))
            {
                var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RestorePoint>>(output);
                if (parsed != null) points = parsed;
            }
            else if (!string.IsNullOrWhiteSpace(output) && output.Trim().StartsWith("{"))
            {
                var single = Newtonsoft.Json.JsonConvert.DeserializeObject<RestorePoint>(output);
                if (single != null) points.Add(single);
            }
        }
        catch { }
        
        return points;
    }

    public async Task<bool> CreateRestorePointAsync(string description)
    {
        try
        {
            var safeName = description.Replace("'", "''").Replace("\"", "");
            var script = $@"
Enable-ComputerRestore -Drive 'C:\' -ErrorAction SilentlyContinue
Checkpoint-Computer -Description '{safeName}' -RestorePointType 'MODIFY_SETTINGS' -ErrorAction Stop
";
            await RunPowerShellAsync(script);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<string> RunPowerShellAsync(string script)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"rp-{Guid.NewGuid():N}.ps1");
        await File.WriteAllTextAsync(tempFile, script);

        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NonInteractive -NoProfile -ExecutionPolicy Bypass -File \"{tempFile}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return output;
        }
        finally
        {
            try { File.Delete(tempFile); } catch { }
        }
    }
}

public interface ISettingsService
{
    string? GetSetting(string key);
    void SetSetting(string key, string value);
    Dictionary<string, string> GetAllSettings();
}

public class SettingsService : ISettingsService
{
    private readonly string _settingsFile;
    private Dictionary<string, string> _settings;

    public SettingsService()
    {
        var dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AIO_PC_Tool_v2");
        Directory.CreateDirectory(dataDir);
        _settingsFile = Path.Combine(dataDir, "settings.json");
        _settings = LoadSettings();
    }

    public string? GetSetting(string key)
    {
        return _settings.TryGetValue(key, out var value) ? value : null;
    }

    public void SetSetting(string key, string value)
    {
        _settings[key] = value;
        SaveSettings();
    }

    public Dictionary<string, string> GetAllSettings() => new(_settings);

    private Dictionary<string, string> LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsFile))
            {
                var json = File.ReadAllText(_settingsFile);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new();
            }
        }
        catch { }
        return new Dictionary<string, string>();
    }

    private void SaveSettings()
    {
        try
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_settingsFile, json);
        }
        catch { }
    }
}
