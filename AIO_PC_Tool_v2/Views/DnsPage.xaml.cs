using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class DnsPage : Page
    {
        private readonly DnsService _dnsService;
        private string? _selectedProvider;
        private readonly Dictionary<string, (string primary, string secondary)> _dnsServers = new()
        {
            { "Cloudflare", ("1.1.1.1", "1.0.0.1") },
            { "Google", ("8.8.8.8", "8.8.4.4") },
            { "OpenDNS", ("208.67.222.222", "208.67.220.220") },
            { "Quad9", ("9.9.9.9", "149.112.112.112") },
            { "AdGuard", ("94.140.14.14", "94.140.15.15") },
            { "Default", ("Auto", "Auto") }
        };

        public DnsPage()
        {
            InitializeComponent();
            _dnsService = new DnsService();
        }

        private void DnsCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string provider)
            {
                _selectedProvider = provider;
                ApplyButton.IsEnabled = true;
                StatusText.Text = $"{provider} selected — click Apply to change DNS";
                
                // Reset all card borders
                Card_Cloudflare.BorderBrush = Brushes.Transparent;
                Card_Cloudflare.BorderThickness = new Thickness(0);
                Card_Google.BorderBrush = Brushes.Transparent;
                Card_Google.BorderThickness = new Thickness(0);
                Card_OpenDNS.BorderBrush = Brushes.Transparent;
                Card_OpenDNS.BorderThickness = new Thickness(0);
                Card_Quad9.BorderBrush = Brushes.Transparent;
                Card_Quad9.BorderThickness = new Thickness(0);
                Card_AdGuard.BorderBrush = Brushes.Transparent;
                Card_AdGuard.BorderThickness = new Thickness(0);
                Card_Default.BorderBrush = Brushes.Transparent;
                Card_Default.BorderThickness = new Thickness(0);
                
                // Highlight selected
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                border.BorderThickness = new Thickness(2);
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProvider == null) return;

            try
            {
                if (_selectedProvider == "Default")
                {
                    _dnsService.ResetToAutomatic();
                    ActiveDns.Text = "Default";
                }
                else
                {
                    var servers = _dnsServers[_selectedProvider];
                    var provider = new Models.DnsProvider 
                    { 
                        Name = _selectedProvider,
                        PrimaryDns = servers.primary,
                        SecondaryDns = servers.secondary
                    };
                    _dnsService.SetDns(provider);
                    ActiveDns.Text = _selectedProvider;
                }
                
                StatusText.Text = $"DNS changed to {_selectedProvider}";
                MessageBox.Show($"DNS successfully changed to {_selectedProvider}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to change DNS: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
