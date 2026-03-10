using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;
using System.Collections.ObjectModel;

namespace AIO_PC_Tool_v2.Views
{
    public partial class CleanerPage : Page
    {
        private readonly CleanerService _cleanerService;
        private ObservableCollection<CleanerCategory> _categories;
        private bool _hasScanned = false;

        public CleanerPage()
        {
            InitializeComponent();
            _cleanerService = new CleanerService();
            _categories = _cleanerService.GetCategories();
            BuildCategoryUI();
            UpdateSelectedCount();
        }

        private void BuildCategoryUI()
        {
            CategoriesContainer.Children.Clear();

            // Group categories
            var groups = _categories.GroupBy(c => c.Category).OrderBy(g => GetCategoryOrder(g.Key));

            foreach (var group in groups)
            {
                // Section Header
                var header = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 20, 0, 12) };
                header.Children.Add(new TextBlock 
                { 
                    Text = GetCategoryIcon(group.Key), 
                    FontSize = 14, 
                    Foreground = new SolidColorBrush(GetCategoryAccent(group.Key)),
                    Margin = new Thickness(0, 0, 8, 0),
                    VerticalAlignment = VerticalAlignment.Center
                });
                header.Children.Add(new TextBlock 
                { 
                    Text = group.Key.ToUpper(), 
                    FontSize = 11, 
                    Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center
                });
                CategoriesContainer.Children.Add(header);

                // Cards Grid
                var grid = new UniformGrid { Columns = 3 };

                foreach (var category in group)
                {
                    grid.Children.Add(CreateCategoryCard(category));
                }

                CategoriesContainer.Children.Add(grid);
            }
        }

        private Border CreateCategoryCard(CleanerCategory category)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 10, 10),
                Cursor = Cursors.Hand,
                Tag = category
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
            card.MouseLeave += (s, e) => card.Background = category.IsSelected 
                ? new SolidColorBrush(Color.FromArgb(30, 220, 38, 38)) 
                : new SolidColorBrush(Color.FromRgb(20, 20, 20));
            card.MouseLeftButtonUp += Card_Click;

            // Update initial background if selected
            if (category.IsSelected)
            {
                card.Background = new SolidColorBrush(Color.FromArgb(30, 220, 38, 38));
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                card.BorderThickness = new Thickness(1);
            }

            var content = new Grid();
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Row 0: Checkbox + Name
            var titleRow = new StackPanel { Orientation = Orientation.Horizontal };
            
            var checkbox = new CheckBox
            {
                IsChecked = category.IsSelected,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0),
                Tag = category
            };
            checkbox.Checked += Checkbox_Changed;
            checkbox.Unchecked += Checkbox_Changed;
            titleRow.Children.Add(checkbox);

            titleRow.Children.Add(new TextBlock
            {
                Text = category.Name,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            });

            // Safe badge
            if (category.IsSafe)
            {
                var safeBadge = new Border
                {
                    Background = new SolidColorBrush(Color.FromArgb(40, 34, 197, 94)),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(6, 2, 6, 2),
                    Margin = new Thickness(8, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };
                safeBadge.Child = new TextBlock
                {
                    Text = "SAFE",
                    FontSize = 9,
                    Foreground = new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                    FontWeight = FontWeights.Bold
                };
                titleRow.Children.Add(safeBadge);
            }

            Grid.SetRow(titleRow, 0);
            content.Children.Add(titleRow);

            // Row 1: Description
            var desc = new TextBlock
            {
                Text = category.Description,
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(96, 96, 96)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(26, 6, 0, 0)
            };
            Grid.SetRow(desc, 1);
            content.Children.Add(desc);

            // Row 3: Size info
            var sizeRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(26, 10, 0, 0) };
            
            var sizeText = new TextBlock
            {
                Text = category.Size > 0 ? category.SizeHuman : "—",
                FontSize = 14,
                Foreground = category.Size > 0 
                    ? new SolidColorBrush(Color.FromRgb(220, 38, 38)) 
                    : new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                FontWeight = FontWeights.SemiBold,
                Tag = "SizeText"
            };
            sizeRow.Children.Add(sizeText);

            if (category.FileCount > 0)
            {
                sizeRow.Children.Add(new TextBlock
                {
                    Text = $" • {category.FileCount} files",
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = "FileCountText"
                });
            }

            Grid.SetRow(sizeRow, 3);
            content.Children.Add(sizeRow);

            card.Child = content;
            return card;
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border card && card.Tag is CleanerCategory category)
            {
                category.IsSelected = !category.IsSelected;
                
                // Find and update the checkbox
                if (card.Child is Grid grid)
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is StackPanel sp)
                        {
                            foreach (var item in sp.Children)
                            {
                                if (item is CheckBox cb)
                                {
                                    cb.IsChecked = category.IsSelected;
                                    break;
                                }
                            }
                        }
                    }
                }

                UpdateCardAppearance(card, category);
                UpdateSelectedCount();
            }
        }

        private void Checkbox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.Tag is CleanerCategory category)
            {
                category.IsSelected = cb.IsChecked ?? false;
                
                // Find parent card and update appearance
                var parent = cb.Parent as StackPanel;
                while (parent != null)
                {
                    if (parent.Parent is Grid g && g.Parent is Border card)
                    {
                        UpdateCardAppearance(card, category);
                        break;
                    }
                    parent = parent.Parent as StackPanel;
                }
                
                UpdateSelectedCount();
            }
        }

        private void UpdateCardAppearance(Border card, CleanerCategory category)
        {
            if (category.IsSelected)
            {
                card.Background = new SolidColorBrush(Color.FromArgb(30, 220, 38, 38));
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                card.BorderThickness = new Thickness(1);
            }
            else
            {
                card.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
                card.BorderBrush = null;
                card.BorderThickness = new Thickness(0);
            }
        }

        private void UpdateSelectedCount()
        {
            var selected = _categories.Count(c => c.IsSelected);
            CategoriesSelected.Text = $"{selected} / {_categories.Count}";
        }

        private async void Scan_Click(object sender, RoutedEventArgs e)
        {
            ScanButton.IsEnabled = false;
            CleanButton.IsEnabled = false;
            ScanText.Text = "Scanning...";
            ScanIcon.Text = "⏳";

            try
            {
                var (totalSize, totalFiles) = await _cleanerService.ScanAllAsync(_categories);
                
                TotalSize.Text = FormatSize(totalSize);
                TotalFiles.Text = totalFiles.ToString("N0");
                
                _hasScanned = true;
                CleanButton.IsEnabled = totalSize > 0;
                
                // Rebuild UI to show sizes
                BuildCategoryUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Scan error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ScanButton.IsEnabled = true;
            ScanText.Text = "Scan System";
            ScanIcon.Text = "🔍";
        }

        private async void Clean_Click(object sender, RoutedEventArgs e)
        {
            var selectedCount = _categories.Count(c => c.IsSelected && c.Size > 0);
            if (selectedCount == 0)
            {
                MessageBox.Show("No categories with data selected for cleaning.", "Nothing to Clean", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"This will permanently delete files from {selectedCount} categories.\n\nAre you sure you want to continue?",
                "Confirm Clean",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            CleanButton.IsEnabled = false;
            ScanButton.IsEnabled = false;

            try
            {
                var cleaned = await _cleanerService.CleanSelectedAsync(_categories);
                
                LastCleanTime.Text = DateTime.Now.ToString("MMM dd, HH:mm");
                
                MessageBox.Show(
                    $"Successfully freed {FormatSize(cleaned)} of disk space!",
                    "Cleaning Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Re-scan to update sizes
                await Task.Run(async () =>
                {
                    await Task.Delay(500);
                    await Dispatcher.InvokeAsync(async () =>
                    {
                        var (totalSize, totalFiles) = await _cleanerService.ScanAllAsync(_categories);
                        TotalSize.Text = FormatSize(totalSize);
                        TotalFiles.Text = totalFiles.ToString("N0");
                        BuildCategoryUI();
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Clean error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CleanButton.IsEnabled = _hasScanned;
            ScanButton.IsEnabled = true;
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var cat in _categories)
            {
                cat.IsSelected = true;
            }
            BuildCategoryUI();
            UpdateSelectedCount();
        }

        private void DeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var cat in _categories)
            {
                cat.IsSelected = false;
            }
            BuildCategoryUI();
            UpdateSelectedCount();
        }

        private string FormatSize(long bytes)
        {
            if (bytes == 0) return "0 B";
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

        private int GetCategoryOrder(string category)
        {
            return category switch
            {
                "Gaming" => 0,
                "System" => 1,
                "Browser" => 2,
                "Apps" => 3,
                _ => 99
            };
        }

        private string GetCategoryIcon(string category)
        {
            return category switch
            {
                "Gaming" => "🎮",
                "System" => "⚙",
                "Browser" => "🌐",
                "Apps" => "📱",
                _ => "📁"
            };
        }

        private Color GetCategoryAccent(string category)
        {
            return category switch
            {
                "Gaming" => Color.FromRgb(220, 38, 38),    // Red
                "System" => Color.FromRgb(59, 130, 246),   // Blue
                "Browser" => Color.FromRgb(139, 92, 246),  // Purple
                "Apps" => Color.FromRgb(34, 197, 94),      // Green
                _ => Color.FromRgb(107, 114, 128)
            };
        }
    }
}
