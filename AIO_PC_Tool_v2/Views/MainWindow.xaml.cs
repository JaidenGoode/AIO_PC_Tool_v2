using System.Windows;
using System.Windows.Controls;

namespace AIO_PC_Tool_v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new DashboardPage());
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string pageName)
            {
                switch (pageName)
                {
                    case "Dashboard":
                        MainFrame.Navigate(new DashboardPage());
                        break;
                    case "Tweaks":
                        MainFrame.Navigate(new TweaksPage());
                        break;
                    case "Cleaner":
                        MainFrame.Navigate(new CleanerPage());
                        break;
                    case "Dns":
                        MainFrame.Navigate(new DnsPage());
                        break;
                    case "Restore":
                        MainFrame.Navigate(new RestorePointsPage());
                        break;
                    case "Utilities":
                        MainFrame.Navigate(new UtilitiesPage());
                        break;
                    case "Settings":
                        MainFrame.Navigate(new SettingsPage());
                        break;
                }
            }
        }
    }
}
