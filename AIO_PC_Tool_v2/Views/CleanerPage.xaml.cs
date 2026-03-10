using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class CleanerPage : Page
    {
        private readonly CleanerService _cleanerService;
        private List<CleanerCategory> _categories;

        public CleanerPage()
        {
            InitializeComponent();
            _cleanerService = new CleanerService();
            _categories = _cleanerService.GetCategories().ToList();
            DisplayCategories();
        }

        private void DisplayCategories()
        {
            CategoriesList.Children.Clear();
            
            foreach (var category in _categories)
            {
                var card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16),
                    Margin = new Thickness(0, 0, 0, 8)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var checkbox = new CheckBox 
                { 
                    IsChecked = category.IsSelected, 
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = category
                };
                checkbox.Checked += (s, e) => category.IsSelected = true;
                checkbox.Unchecked += (s, e) => category.IsSelected = false;
                Grid.SetColumn(checkbox, 0);
                grid.Children.Add(checkbox);

                var info = new StackPanel { Margin = new Thickness(12, 0, 0, 0) };
                info.Children.Add(new TextBlock { Text = category.Name, FontWeight = FontWeights.SemiBold, FontSize = 14, Foreground = Brushes.White });
                info.Children.Add(new TextBlock { Text = category.Description, FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)) });
                Grid.SetColumn(info, 1);
                grid.Children.Add(info);

                var sizeText = new TextBlock 
                { 
                    Text = category.SizeHuman, 
                    FontWeight = FontWeights.SemiBold, 
                    FontSize = 14, 
                    Foreground = new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(sizeText, 2);
                grid.Children.Add(sizeText);

                card.Child = grid;
                CategoriesList.Children.Add(card);
            }
        }

        private async void Scan_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Scanning...";
            long total = 0;

            foreach (var category in _categories)
            {
                var (size, count) = await _cleanerService.ScanCategoryAsync(category);
                total += size;
            }

            TotalSize.Text = FormatSize(total);
            StatusText.Text = "Scan complete";
            DisplayCategories();
        }

        private async void Clean_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Cleaning...";
            long cleaned = 0;

            foreach (var category in _categories.Where(c => c.IsSelected))
            {
                cleaned += await _cleanerService.CleanCategoryAsync(category);
            }

            StatusText.Text = $"Cleaned {FormatSize(cleaned)}";
            await Task.Delay(500);
            Scan_Click(sender, e);
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }
}
