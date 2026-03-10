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
        private Dictionary<string, Border> _cardBorders = new();

        public DnsPage()
        {
            InitializeComponent();
            _dnsService = new DnsService();
            InitializeCards();
        }

        private void InitializeCards()
        {
            _cardBorders["Cloudflare"] = Card_Cloudflare;
            _cardBorders["Google"] = Card_Google;
            _cardBorders["OpenDNS"] = Card_OpenDNS;
            _cardBorders["Quad9"] = Card_Quad9;
            _cardBorders["AdGuard"] = Card_AdGuard;
            _cardBorders["Default"] = Card_Default;

            // Add hover effects
            foreach (var kvp in _cardBorders)
            {
                var border = kvp.Value;
                var provider = kvp.Key;
                
                border.MouseEnter += (s, e) =>
                {
                    if (provider != _selectedProvider)
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    }
                };
                
                border.MouseLeave += (s, e) =>
                {
                    if (provider != _selectedProvider)
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
                    }
                };
            }
        }

        private void DnsCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string provider)
            {
                _selectedProvider = provider;
                UpdateCardSelection();
                ApplyButton.IsEnabled = true;
                StatusText.Text = $"Ready to apply {provider} DNS";
            }
        }

        private void UpdateCardSelection()
        {
            foreach (var kvp in _cardBorders)
            {
                var border = kvp.Value;
                var provider = kvp.Key;
                
                if (provider == _selectedProvider)
                {
                    border.Background = new SolidColorBrush(Color.FromArgb(30, 220, 38, 38));
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    border.BorderThickness = new Thickness(2);
                }
                else
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
                    border.BorderBrush = null;
                    border.BorderThickness = new Thickness(0);
                }
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedProvider))
            {
                MessageBox.Show("Please select a DNS provider first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_selectedProvider == "Default")
                {
                    _dnsService.ResetToAutomatic();
                    ActiveDns.Text = "Default (ISP)";
                    StatusText.Text = "DNS reset to automatic (ISP default)";
                    MessageBox.Show("DNS has been reset to automatic.\n\nYou may need to restart your browser.", "DNS Reset", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var providers = _dnsService.GetProviders();
                    var provider = providers.FirstOrDefault(p => p.Name.StartsWith(_selectedProvider));
                    
                    if (provider != null)
                    {
                        _dnsService.SetDns(provider);
                        ActiveDns.Text = _selectedProvider;
                        StatusText.Text = $"{_selectedProvider} DNS applied successfully";
                        MessageBox.Show($"DNS changed to {_selectedProvider}!\n\nPrimary: {provider.PrimaryDns}\nSecondary: {provider.SecondaryDns}\n\nYou may need to restart your browser.", "DNS Changed", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to apply DNS: {ex.Message}\n\nMake sure you're running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
