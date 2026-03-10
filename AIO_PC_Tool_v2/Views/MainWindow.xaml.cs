using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AIO_PC_Tool_v2.Views
{
    public partial class MainWindow : Window
    {
        private string _currentPage = "Dashboard";
        private Dictionary<string, Button> _navButtons = new();

        public MainWindow()
        {
            InitializeComponent();
            
            // Store nav buttons
            _navButtons["Dashboard"] = NavDashboard;
            _navButtons["Cleaner"] = NavCleaner;
            _navButtons["Tweaks"] = NavTweaks;
            _navButtons["Utilities"] = NavUtilities;
            _navButtons["Dns"] = NavDns;
            _navButtons["Restore"] = NavRestore;
            _navButtons["Settings"] = NavSettings;
            
            // Navigate to Dashboard
            NavigateTo("Dashboard");
        }

        public void NavigateTo(string pageName)
        {
            _currentPage = pageName;
            UpdateNavHighlight();
            
            Page? page = pageName switch
            {
                "Dashboard" => new DashboardPage(),
                "Cleaner" => new CleanerPage(),
                "Tweaks" => new TweaksPage(),
                "Utilities" => new UtilitiesPage(),
                "Dns" => new DnsPage(),
                "Restore" => new RestorePointsPage(),
                "Settings" => new SettingsPage(),
                _ => new DashboardPage()
            };
            
            MainFrame.Navigate(page);
        }

        private void UpdateNavHighlight()
        {
            foreach (var kvp in _navButtons)
            {
                if (kvp.Key == _currentPage)
                {
                    kvp.Value.Background = new SolidColorBrush(Color.FromArgb(32, 220, 38, 38));
                    kvp.Value.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                }
                else
                {
                    kvp.Value.Background = Brushes.Transparent;
                    kvp.Value.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                }
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string pageName)
            {
                NavigateTo(pageName);
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
