using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class RestorePointsPage : Page
    {
        private readonly RestorePointService _restorePointService;

        public RestorePointsPage()
        {
            InitializeComponent();
            _restorePointService = new RestorePointService();
            LoadRestorePoints();
        }

        private void LoadRestorePoints()
        {
            PointsList.Children.Clear();
            var points = _restorePointService.GetRestorePoints();

            if (points.Count == 0)
            {
                PointsList.Children.Add(new TextBlock 
                { 
                    Text = "No restore points found", 
                    Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)),
                    FontSize = 14
                });
                return;
            }

            foreach (var point in points)
            {
                var card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16),
                    Margin = new Thickness(0, 0, 0, 8)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var info = new StackPanel();
                info.Children.Add(new TextBlock { Text = point.Description, FontWeight = FontWeights.SemiBold, FontSize = 14, Foreground = Brushes.White });
                info.Children.Add(new TextBlock { Text = point.CreationTime.ToString("MMMM dd, yyyy 'at' HH:mm"), FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)), Margin = new Thickness(0, 4, 0, 0) });
                Grid.SetColumn(info, 0);
                grid.Children.Add(info);

                var seq = new TextBlock 
                { 
                    Text = $"#{point.SequenceNumber}", 
                    FontSize = 12, 
                    Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(seq, 1);
                grid.Children.Add(seq);

                card.Child = grid;
                PointsList.Children.Add(card);
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var name = PointName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name for the restore point", "Name Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _restorePointService.CreateRestorePoint(name);
            PointName.Text = "";
            MessageBox.Show("Restore point creation started. This may take a moment.", "Creating", MessageBoxButton.OK, MessageBoxImage.Information);
            
            Task.Delay(3000).ContinueWith(_ => Dispatcher.Invoke(LoadRestorePoints));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRestorePoints();
        }
    }
}
