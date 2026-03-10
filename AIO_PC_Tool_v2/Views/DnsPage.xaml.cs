using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class DnsPage : Page
    {
        private readonly DnsService _dnsService;
        private List<DnsProvider> _providers;
        private DnsProvider? _selectedProvider;

        public DnsPage()
        {
            InitializeComponent();
            _dnsService = new DnsService();
            _providers = _dnsService.GetProviders().ToList();
            DisplayProviders();
        }

        private void DisplayProviders()
        {
            ProvidersList.Children.Clear();
            
            foreach (var provider in _providers)
            {
                var card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16),
                    Margin = new Thickness(0, 0, 0, 8),
                    BorderBrush = _selectedProvider == provider ? new SolidColorBrush(Color.FromRgb(220, 38, 38)) : Brushes.Transparent,
                    BorderThickness = new Thickness(2),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = provider
                };
                card.MouseLeftButtonUp += (s, e) => 
                {
                    _selectedProvider = provider;
                    DisplayProviders();
                };

                var content = new StackPanel();
                content.Children.Add(new TextBlock { Text = provider.Name, FontWeight = FontWeights.SemiBold, FontSize = 15, Foreground = Brushes.White });
                content.Children.Add(new TextBlock { Text = provider.Description, FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)), Margin = new Thickness(0, 2, 0, 0) });
                
                var dnsRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 8, 0, 0) };
                
                var primary = new Border { Background = new SolidColorBrush(Color.FromRgb(37, 37, 37)), CornerRadius = new CornerRadius(4), Padding = new Thickness(8, 4, 8, 4) };
                primary.Child = new TextBlock { Text = provider.PrimaryDns, FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(115, 115, 115)), FontFamily = new FontFamily("Consolas") };
                dnsRow.Children.Add(primary);
                
                var secondary = new Border { Background = new SolidColorBrush(Color.FromRgb(37, 37, 37)), CornerRadius = new CornerRadius(4), Padding = new Thickness(8, 4, 8, 4), Margin = new Thickness(8, 0, 0, 0) };
                secondary.Child = new TextBlock { Text = provider.SecondaryDns, FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(115, 115, 115)), FontFamily = new FontFamily("Consolas") };
                dnsRow.Children.Add(secondary);
                
                content.Children.Add(dnsRow);
                card.Child = content;
                ProvidersList.Children.Add(card);
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProvider != null)
            {
                _dnsService.SetDns(_selectedProvider);
                MessageBox.Show($"DNS set to {_selectedProvider.Name}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a DNS provider first", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _dnsService.ResetToAutomatic();
            _selectedProvider = null;
            DisplayProviders();
            MessageBox.Show("DNS reset to automatic (DHCP)", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
