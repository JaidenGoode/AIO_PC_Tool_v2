using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                var emptyState = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(40, 50, 40, 50),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                
                var content = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
                content.Children.Add(new TextBlock 
                { 
                    Text = "📂", 
                    FontSize = 40, 
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 16)
                });
                content.Children.Add(new TextBlock 
                { 
                    Text = "No restore points found", 
                    FontSize = 16,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                content.Children.Add(new TextBlock 
                { 
                    Text = "Create one above to protect your system", 
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 6, 0, 0)
                });
                
                emptyState.Child = content;
                PointsList.Children.Add(emptyState);
                return;
            }

            foreach (var point in points.OrderByDescending(p => p.CreationTime))
            {
                PointsList.Children.Add(CreateRestorePointCard(point));
            }
        }

        private Border CreateRestorePointCard(RestorePoint point)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(20, 16, 20, 16),
                Margin = new Thickness(0, 0, 0, 10),
                Cursor = Cursors.Hand
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
            card.MouseLeave += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Icon
            var iconBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(30, 34, 197, 94)),
                CornerRadius = new CornerRadius(8),
                Width = 40,
                Height = 40,
                Margin = new Thickness(0, 0, 16, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            iconBorder.Child = new TextBlock 
            { 
                Text = "↺", 
                FontSize = 18, 
                Foreground = new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(iconBorder, 0);
            grid.Children.Add(iconBorder);

            // Info
            var info = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            info.Children.Add(new TextBlock 
            { 
                Text = point.Description, 
                FontWeight = FontWeights.SemiBold, 
                FontSize = 14, 
                Foreground = Brushes.White 
            });
            
            var dateRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 0) };
            dateRow.Children.Add(new TextBlock 
            { 
                Text = point.CreationTime.ToString("MMMM dd, yyyy 'at' HH:mm"), 
                FontSize = 12, 
                Foreground = new SolidColorBrush(Color.FromRgb(120, 120, 120))
            });
            info.Children.Add(dateRow);
            
            Grid.SetColumn(info, 1);
            grid.Children.Add(info);

            // Sequence number badge
            var seqBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(12, 6, 12, 6),
                VerticalAlignment = VerticalAlignment.Center
            };
            seqBadge.Child = new TextBlock 
            { 
                Text = $"#{point.SequenceNumber}", 
                FontSize = 12, 
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                FontWeight = FontWeights.SemiBold
            };
            Grid.SetColumn(seqBadge, 2);
            grid.Children.Add(seqBadge);

            card.Child = grid;
            return card;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var name = PointName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name for the restore point.", "Name Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _restorePointService.CreateRestorePoint(name);
                PointName.Text = "";
                MessageBox.Show("Restore point creation started.\n\nThis may take a moment. Click 'Refresh' to see it appear.", "Creating Restore Point", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Delayed refresh
                Task.Delay(5000).ContinueWith(_ => Dispatcher.Invoke(LoadRestorePoints));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create restore point: {ex.Message}\n\nMake sure you're running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRestorePoints();
        }
    }
}
