using System.Windows;
using System.Windows.Controls;

namespace AIO_PC_Tool_v2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new DashboardPage());
    }

    private void NavButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb && rb.Tag is string tag)
        {
            Page page = tag switch
            {
                "Dashboard" => new DashboardPage(),
                "Tweaks" => new TweaksPage(),
                "Cleaner" => new CleanerPage(),
                "DNS" => new DnsPage(),
                "Restore" => new RestorePointsPage(),
                "Utilities" => new UtilitiesPage(),
                "Settings" => new SettingsPage(),
                _ => new DashboardPage()
            };
            MainFrame.Navigate(page);
        }
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
