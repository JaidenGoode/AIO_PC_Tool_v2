using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
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
        private DispatcherTimer? _scanAnimTimer;

        public CleanerPage()
        {
            InitializeComponent();
            _cleanerService = new CleanerService();
            _categories = new ObservableCollection<CleanerCategory>();
            
            ShowState("initial");
        }

        private void ShowState(string state)
        {
            InitialState.Visibility = state == "initial" ? Visibility.Visible : Visibility.Collapsed;
            ScanningState.Visibility = state == "scanning" ? Visibility.Visible : Visibility.Collapsed;
            AllCleanState.Visibility = state == "clean" ? Visibility.Visible : Visibility.Collapsed;
            ResultsState.Visibility = state == "results" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BuildCategoryUI()
        {
            CategoriesContainer.Children.Clear();

            if (_categories.Count == 0)
            {
                ShowState("clean");
                return;
            }

            // Group categories
            var groups = _categories
                .Where(c => c.Size > 0) // Only show categories that have data
                .GroupBy(c => c.Category)
                .OrderBy(g => GetCategoryOrder(g.Key));

            if (!groups.Any())
            {
                ShowState("clean");
                return;
            }

            ShowState("results");

            foreach (var group in groups)
            {
                // Section Header
                var header = new Border 
                { 
                    Margin = new Thickness(0, 24, 0, 16),
                    Padding = new Thickness(0)
                };
                
                var headerContent = new StackPanel { Orientation = Orientation.Horizontal };
                headerContent.Children.Add(new Border
                {
                    Background = new SolidColorBrush(Color.FromArgb(30, GetCategoryColor(group.Key).R, GetCategoryColor(group.Key).G, GetCategoryColor(group.Key).B)),
                    CornerRadius = new CornerRadius(6),
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(0, 0, 12, 0),
                    Child = new TextBlock
                    {
                        Text = GetCategoryIcon(group.Key),
                        FontSize = 14,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                });
                
                var headerText = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                headerText.Children.Add(new TextBlock
                {
                    Text = group.Key.ToUpper(),
                    FontSize = 13,
                    Foreground = new SolidColorBrush(GetCategoryColor(group.Key)),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Segoe UI")
                });
                
                var totalSize = group.Sum(c => c.Size);
                var totalFiles = group.Sum(c => c.FileCount);
                headerText.Children.Add(new TextBlock
                {
                    Text = $"{group.Count()} items • {FormatSize(totalSize)} • {totalFiles:N0} files",
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                    FontFamily = new FontFamily("Segoe UI")
                });
                
                headerContent.Children.Add(headerText);
                header.Child = headerContent;
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
                Background = category.IsSelected 
                    ? new SolidColorBrush(Color.FromArgb(20, 220, 38, 38)) 
                    : new SolidColorBrush(Color.FromRgb(18, 18, 18)),
                BorderBrush = category.IsSelected 
                    ? new SolidColorBrush(Color.FromArgb(60, 220, 38, 38))
                    : new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(18),
                Margin = new Thickness(0, 0, 12, 12),
                Cursor = Cursors.Hand,
                Tag = category
            };

            // Hover effect
            card.MouseEnter += (s, e) => 
            {
                if (!category.IsSelected)
                    card.Background = new SolidColorBrush(Color.FromRgb(24, 24, 24));
            };
            card.MouseLeave += (s, e) => 
            {
                if (!category.IsSelected)
                    card.Background = new SolidColorBrush(Color.FromRgb(18, 18, 18));
            };
            card.MouseLeftButtonUp += Card_Click;

            var content = new Grid();
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Row 0: Icon + Name + Checkbox
            var titleRow = new Grid();
            titleRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            titleRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Icon
            var iconBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                CornerRadius = new CornerRadius(8),
                Width = 36,
                Height = 36,
                Margin = new Thickness(0, 0, 12, 0)
            };
            iconBorder.Child = new TextBlock
            {
                Text = category.Icon,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(iconBorder, 0);
            titleRow.Children.Add(iconBorder);

            // Name
            var namePanel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            namePanel.Children.Add(new TextBlock
            {
                Text = category.Name,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Segoe UI")
            });
            Grid.SetColumn(namePanel, 1);
            titleRow.Children.Add(namePanel);

            // Checkbox
            var checkbox = new CheckBox
            {
                IsChecked = category.IsSelected,
                VerticalAlignment = VerticalAlignment.Center,
                Tag = category
            };
            checkbox.Checked += Checkbox_Changed;
            checkbox.Unchecked += Checkbox_Changed;
            Grid.SetColumn(checkbox, 2);
            titleRow.Children.Add(checkbox);

            Grid.SetRow(titleRow, 0);
            content.Children.Add(titleRow);

            // Row 1: Description
            var desc = new TextBlock
            {
                Text = category.Description,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(48, 8, 0, 0),
                FontFamily = new FontFamily("Segoe UI")
            };
            Grid.SetRow(desc, 1);
            content.Children.Add(desc);

            // Row 3: Size and file count
            var statsRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(48, 14, 0, 0) };
            
            // Size badge
            var sizeBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(30, 220, 38, 38)),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(0, 0, 8, 0)
            };
            sizeBadge.Child = new TextBlock
            {
                Text = category.SizeHuman,
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)),
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI")
            };
            statsRow.Children.Add(sizeBadge);

            // File count
            statsRow.Children.Add(new TextBlock
            {
                Text = $"{category.FileCount:N0} files",
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Segoe UI")
            });

            Grid.SetRow(statsRow, 3);
            content.Children.Add(statsRow);

            card.Child = content;
            return card;
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border card && card.Tag is CleanerCategory category)
            {
                category.IsSelected = !category.IsSelected;
                UpdateCardAppearance(card, category);
                
                // Update checkbox
                if (card.Child is Grid grid)
                {
                    FindAndUpdateCheckbox(grid, category.IsSelected);
                }
                
                UpdateSelectedCount();
            }
        }

        private void FindAndUpdateCheckbox(DependencyObject parent, bool isChecked)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is CheckBox cb)
                {
                    cb.IsChecked = isChecked;
                    return;
                }
                FindAndUpdateCheckbox(child, isChecked);
            }
        }

        private void Checkbox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.Tag is CleanerCategory category)
            {
                category.IsSelected = cb.IsChecked ?? false;
                
                // Find parent card
                DependencyObject? parent = cb;
                while (parent != null && !(parent is Border border && border.Tag is CleanerCategory))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                
                if (parent is Border card)
                {
                    UpdateCardAppearance(card, category);
                }
                
                UpdateSelectedCount();
            }
        }

        private void UpdateCardAppearance(Border card, CleanerCategory category)
        {
            if (category.IsSelected)
            {
                card.Background = new SolidColorBrush(Color.FromArgb(20, 220, 38, 38));
                card.BorderBrush = new SolidColorBrush(Color.FromArgb(60, 220, 38, 38));
            }
            else
            {
                card.Background = new SolidColorBrush(Color.FromRgb(18, 18, 18));
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
        }

        private void UpdateSelectedCount()
        {
            var selected = _categories.Count(c => c.IsSelected && c.Size > 0);
            var total = _categories.Count(c => c.Size > 0);
            CategoriesCount.Text = $"{selected}/{total}";
            CleanButton.IsEnabled = selected > 0;
        }

        private async void Scan_Click(object sender, RoutedEventArgs e)
        {
            ScanButton.IsEnabled = false;
            CleanButton.IsEnabled = false;
            ScanText.Text = "Scanning...";
            ScanIcon.Text = "⏳";

            ShowState("scanning");
            StartScanAnimation();

            try
            {
                // First, get only categories that exist on this system
                ScanningStatus.Text = "Detecting installed applications...";
                _categories = await _cleanerService.GetCategoriesWithDataAsync();

                if (_categories.Count == 0)
                {
                    StopScanAnimation();
                    ShowState("clean");
                    TotalSize.Text = "0 B";
                    TotalFiles.Text = "0";
                    CategoriesCount.Text = "0";
                    ScanButton.IsEnabled = true;
                    ScanText.Text = "Scan Now";
                    ScanIcon.Text = "🔍";
                    return;
                }

                // Now scan each category for actual file sizes
                ScanningStatus.Text = "Calculating file sizes...";
                var (totalSize, totalFiles, categoryCount) = await _cleanerService.ScanAllAsync(_categories);

                // Filter out categories with 0 bytes after scanning
                var categoriesWithData = _categories.Where(c => c.Size > 0).ToList();
                _categories = new ObservableCollection<CleanerCategory>(categoriesWithData);

                StopScanAnimation();

                if (_categories.Count == 0 || totalSize == 0)
                {
                    ShowState("clean");
                    TotalSize.Text = "0 B";
                    TotalFiles.Text = "0";
                    CategoriesCount.Text = "0";
                }
                else
                {
                    TotalSize.Text = FormatSize(totalSize);
                    TotalFiles.Text = totalFiles.ToString("N0");
                    CategoriesCount.Text = _categories.Count.ToString();
                    
                    _hasScanned = true;
                    CleanButton.IsEnabled = true;
                    
                    BuildCategoryUI();
                }
            }
            catch (Exception ex)
            {
                StopScanAnimation();
                MessageBox.Show($"Scan error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ShowState("initial");
            }

            ScanButton.IsEnabled = true;
            ScanText.Text = "Scan Now";
            ScanIcon.Text = "🔍";
        }

        private void StartScanAnimation()
        {
            _scanAnimTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            int frame = 0;
            string[] frames = { "⏳", "⌛" };
            _scanAnimTimer.Tick += (s, e) =>
            {
                ScanningIcon.Text = frames[frame % frames.Length];
                frame++;
            };
            _scanAnimTimer.Start();
        }

        private void StopScanAnimation()
        {
            _scanAnimTimer?.Stop();
            _scanAnimTimer = null;
        }

        private async void Clean_Click(object sender, RoutedEventArgs e)
        {
            var selectedWithData = _categories.Where(c => c.IsSelected && c.Size > 0).ToList();
            if (selectedWithData.Count == 0)
            {
                MessageBox.Show("No categories with data selected for cleaning.", "Nothing to Clean", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var totalToClean = selectedWithData.Sum(c => c.Size);
            var result = MessageBox.Show(
                $"This will permanently delete {FormatSize(totalToClean)} from {selectedWithData.Count} categories.\n\nThis action cannot be undone. Continue?",
                "Confirm Clean",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            CleanButton.IsEnabled = false;
            ScanButton.IsEnabled = false;

            try
            {
                var selectedCategories = new ObservableCollection<CleanerCategory>(selectedWithData);
                var cleaned = await _cleanerService.CleanSelectedAsync(selectedCategories);
                
                LastCleanTime.Text = DateTime.Now.ToString("HH:mm");
                
                MessageBox.Show(
                    $"Successfully freed {FormatSize(cleaned)} of disk space!",
                    "Cleaning Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Re-scan
                _categories = await _cleanerService.GetCategoriesWithDataAsync();
                var (totalSize, totalFiles, _) = await _cleanerService.ScanAllAsync(_categories);
                
                // Filter out empty categories
                var categoriesWithData = _categories.Where(c => c.Size > 0).ToList();
                _categories = new ObservableCollection<CleanerCategory>(categoriesWithData);

                if (_categories.Count == 0 || totalSize == 0)
                {
                    ShowState("clean");
                    TotalSize.Text = "0 B";
                    TotalFiles.Text = "0";
                    CategoriesCount.Text = "0";
                    CleanButton.IsEnabled = false;
                }
                else
                {
                    TotalSize.Text = FormatSize(totalSize);
                    TotalFiles.Text = totalFiles.ToString("N0");
                    CategoriesCount.Text = _categories.Count.ToString();
                    BuildCategoryUI();
                    CleanButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Clean error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        private Color GetCategoryColor(string category)
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
