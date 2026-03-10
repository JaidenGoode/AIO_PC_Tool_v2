using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class TweaksPage : Page
    {
        private readonly TweakService _tweakService;
        private List<Tweak> _allTweaks;
        private string _currentCategory = "All";
        private Dictionary<string, Button> _categoryButtons = new();

        private static readonly string[] Categories = { "All", "Gaming", "Performance", "Privacy", "Network", "Visual", "Power", "Security", "Storage" };

        public TweaksPage()
        {
            InitializeComponent();
            _tweakService = new TweakService();
            _allTweaks = new List<Tweak>();
            
            BuildCategoryButtons();
            LoadTweaks();
        }

        private void BuildCategoryButtons()
        {
            CategoryButtons.Children.Clear();
            _categoryButtons.Clear();

            foreach (var category in Categories)
            {
                var btn = new Button
                {
                    Content = category,
                    Tag = category,
                    Background = category == "All" 
                        ? new SolidColorBrush(Color.FromRgb(220, 38, 38)) 
                        : Brushes.Transparent,
                    Foreground = category == "All" 
                        ? Brushes.White 
                        : new SolidColorBrush(Color.FromRgb(128, 128, 128)),
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(14, 8, 14, 8),
                    Cursor = Cursors.Hand,
                    FontSize = 11,
                    FontWeight = FontWeights.Medium,
                    Margin = new Thickness(2, 0, 2, 0)
                };

                // Create the template for rounded corners
                var template = new ControlTemplate(typeof(Button));
                var border = new FrameworkElementFactory(typeof(Border));
                border.SetBinding(Border.BackgroundProperty, new System.Windows.Data.Binding("Background") { RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.TemplatedParent) });
                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
                border.SetValue(Border.PaddingProperty, new Thickness(14, 8, 14, 8));
                
                var presenter = new FrameworkElementFactory(typeof(ContentPresenter));
                presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                border.AppendChild(presenter);
                
                template.VisualTree = border;
                btn.Template = template;

                btn.Click += Category_Click;
                btn.MouseEnter += (s, e) => 
                {
                    if (_currentCategory != category)
                        btn.Background = new SolidColorBrush(Color.FromRgb(37, 37, 37));
                };
                btn.MouseLeave += (s, e) => 
                {
                    if (_currentCategory != category)
                        btn.Background = Brushes.Transparent;
                };

                _categoryButtons[category] = btn;
                CategoryButtons.Children.Add(btn);
            }
        }

        private void LoadTweaks()
        {
            try
            {
                _allTweaks = _tweakService.GetAllTweaks();
                
                foreach (var tweak in _allTweaks)
                {
                    try { _tweakService.CheckTweakStatus(tweak); } catch { }
                }
                
                UpdateCounts();
                DisplayTweaks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tweaks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCounts()
        {
            var activeCount = _allTweaks.Count(t => t.IsActive);
            ActiveCount.Text = activeCount.ToString();
            TotalCount.Text = $" / {_allTweaks.Count}";
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string category)
            {
                _currentCategory = category;
                HighlightCategory(category);
                DisplayTweaks();
            }
        }

        private void HighlightCategory(string category)
        {
            foreach (var kvp in _categoryButtons)
            {
                if (kvp.Key == category)
                {
                    kvp.Value.Background = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    kvp.Value.Foreground = Brushes.White;
                }
                else
                {
                    kvp.Value.Background = Brushes.Transparent;
                    kvp.Value.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                }
            }
        }

        private void DisplayTweaks()
        {
            TweaksList.Children.Clear();
            
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);

            // Group by category if showing all
            if (_currentCategory == "All")
            {
                var groups = filtered.GroupBy(t => t.Category).OrderBy(g => GetCategoryOrder(g.Key));
                
                foreach (var group in groups)
                {
                    // Category header
                    var header = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 16, 0, 12) };
                    header.Children.Add(new Ellipse
                    {
                        Width = 8,
                        Height = 8,
                        Fill = GetCategoryColor(group.Key),
                        Margin = new Thickness(0, 0, 10, 0),
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
                    
                    var activeInCat = group.Count(t => t.IsActive);
                    header.Children.Add(new TextBlock
                    {
                        Text = $" • {activeInCat}/{group.Count()} active",
                        FontSize = 11,
                        Foreground = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    
                    TweaksList.Children.Add(header);

                    foreach (var tweak in group)
                    {
                        TweaksList.Children.Add(CreateTweakCard(tweak));
                    }
                }
            }
            else
            {
                foreach (var tweak in filtered)
                {
                    TweaksList.Children.Add(CreateTweakCard(tweak));
                }
            }
        }

        private Border CreateTweakCard(Tweak tweak)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(20, 16, 20, 16),
                Margin = new Thickness(0, 0, 0, 8)
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
            card.MouseLeave += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Left content
            var content = new StackPanel();

            // Title row with status indicator
            var titleRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 6) };
            
            // Status dot
            var statusDot = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94)) 
                    : new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Margin = new Thickness(0, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            titleRow.Children.Add(statusDot);

            // Title
            titleRow.Children.Add(new TextBlock 
            { 
                Text = tweak.Title, 
                FontWeight = FontWeights.SemiBold, 
                FontSize = 14, 
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            });
            
            // Category badge (only if showing All)
            if (_currentCategory == "All")
            {
                var categoryBadge = new Border
                {
                    Background = new SolidColorBrush(Color.FromArgb(40, GetCategoryColor(tweak.Category).Color.R, GetCategoryColor(tweak.Category).Color.G, GetCategoryColor(tweak.Category).Color.B)),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(8, 3, 8, 3),
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };
                categoryBadge.Child = new TextBlock 
                { 
                    Text = tweak.Category.ToUpper(), 
                    FontSize = 9, 
                    Foreground = GetCategoryColor(tweak.Category),
                    FontWeight = FontWeights.Bold
                };
                titleRow.Children.Add(categoryBadge);
            }
            
            // Status badge
            var statusBadge = new Border
            {
                Background = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromArgb(40, 34, 197, 94))
                    : new SolidColorBrush(Color.FromRgb(35, 35, 35)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 3, 8, 3),
                Margin = new Thickness(8, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            statusBadge.Child = new TextBlock 
            { 
                Text = tweak.IsActive ? "✓ OPTIMIZED" : "DEFAULT", 
                FontSize = 9, 
                Foreground = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
                    : new SolidColorBrush(Color.FromRgb(90, 90, 90)),
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(statusBadge);

            // Windows version badge
            var winBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 3, 8, 3),
                Margin = new Thickness(6, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            winBadge.Child = new TextBlock 
            { 
                Text = $"Win {tweak.WindowsVersion}", 
                FontSize = 9, 
                Foreground = new SolidColorBrush(Color.FromRgb(70, 70, 70)),
                FontWeight = FontWeights.Medium
            };
            titleRow.Children.Add(winBadge);

            content.Children.Add(titleRow);
            
            // Description
            content.Children.Add(new TextBlock 
            { 
                Text = tweak.Description, 
                FontSize = 12, 
                Foreground = new SolidColorBrush(Color.FromRgb(130, 130, 130)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(18, 0, 0, 0),
                MaxWidth = 700
            });
            
            // Warning if present
            if (!string.IsNullOrEmpty(tweak.Warning))
            {
                var warningPanel = new StackPanel 
                { 
                    Orientation = Orientation.Horizontal, 
                    Margin = new Thickness(18, 8, 0, 0) 
                };
                warningPanel.Children.Add(new TextBlock 
                { 
                    Text = "⚠", 
                    FontSize = 11, 
                    Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)),
                    Margin = new Thickness(0, 0, 6, 0)
                });
                warningPanel.Children.Add(new TextBlock 
                { 
                    Text = tweak.Warning, 
                    FontSize = 11, 
                    Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)),
                    TextWrapping = TextWrapping.Wrap
                });
                content.Children.Add(warningPanel);
            }
            
            Grid.SetColumn(content, 0);
            grid.Children.Add(content);

            // Right button
            var buttonPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            if (!tweak.IsActive)
            {
                var applyBtn = CreateActionButton("Apply", true, tweak);
                applyBtn.Click += ApplyTweak_Click;
                buttonPanel.Children.Add(applyBtn);
            }
            else
            {
                var revertBtn = CreateActionButton("Revert", false, tweak);
                revertBtn.Click += RevertTweak_Click;
                buttonPanel.Children.Add(revertBtn);
            }
            
            Grid.SetColumn(buttonPanel, 1);
            grid.Children.Add(buttonPanel);

            card.Child = grid;
            return card;
        }

        private Button CreateActionButton(string text, bool isPrimary, Tweak tweak)
        {
            var btn = new Button
            {
                Content = text,
                Background = isPrimary 
                    ? new SolidColorBrush(Color.FromRgb(220, 38, 38))
                    : new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                Foreground = isPrimary 
                    ? Brushes.White 
                    : new SolidColorBrush(Color.FromRgb(160, 160, 160)),
                BorderThickness = new Thickness(0),
                Padding = new Thickness(20, 10, 20, 10),
                Cursor = Cursors.Hand,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Tag = tweak
            };

            // Create template with rounded corners
            var template = new ControlTemplate(typeof(Button));
            var border = new FrameworkElementFactory(typeof(Border));
            border.SetBinding(Border.BackgroundProperty, new System.Windows.Data.Binding("Background") { RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.TemplatedParent) });
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            border.SetValue(Border.PaddingProperty, new Thickness(20, 10, 20, 10));
            
            var presenter = new FrameworkElementFactory(typeof(ContentPresenter));
            presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(presenter);
            
            template.VisualTree = border;
            btn.Template = template;

            // Hover effects
            btn.MouseEnter += (s, e) =>
            {
                btn.Background = isPrimary 
                    ? new SolidColorBrush(Color.FromRgb(239, 68, 68))
                    : new SolidColorBrush(Color.FromRgb(50, 50, 50));
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.Background = isPrimary 
                    ? new SolidColorBrush(Color.FromRgb(220, 38, 38))
                    : new SolidColorBrush(Color.FromRgb(40, 40, 40));
            };

            return btn;
        }

        private SolidColorBrush GetCategoryColor(string category)
        {
            return category.ToLower() switch
            {
                "gaming" => new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                "performance" => new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                "privacy" => new SolidColorBrush(Color.FromRgb(139, 92, 246)),
                "network" => new SolidColorBrush(Color.FromRgb(249, 115, 22)),
                "visual" => new SolidColorBrush(Color.FromRgb(236, 72, 153)),
                "power" => new SolidColorBrush(Color.FromRgb(234, 179, 8)),
                "security" => new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                "storage" => new SolidColorBrush(Color.FromRgb(6, 182, 212)),
                _ => new SolidColorBrush(Color.FromRgb(107, 114, 128))
            };
        }

        private int GetCategoryOrder(string category)
        {
            return category.ToLower() switch
            {
                "gaming" => 0,
                "performance" => 1,
                "privacy" => 2,
                "network" => 3,
                "visual" => 4,
                "power" => 5,
                "security" => 6,
                "storage" => 7,
                _ => 99
            };
        }

        private void ApplyTweak_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tweak tweak)
            {
                try
                {
                    if (_tweakService.ApplyTweak(tweak))
                    {
                        UpdateCounts();
                        DisplayTweaks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to apply tweak. Try running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RevertTweak_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tweak tweak)
            {
                try
                {
                    if (_tweakService.RevertTweak(tweak))
                    {
                        UpdateCounts();
                        DisplayTweaks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to revert tweak. Try running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ApplyAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
            
            var toApply = filtered.Where(t => !t.IsActive).ToList();
            
            if (toApply.Count == 0)
            {
                MessageBox.Show("All tweaks in this category are already applied!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Apply {toApply.Count} tweaks?\n\nThis will modify system settings. Run as Administrator for best results.",
                "Confirm Apply All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;
                
            int applied = 0;
            foreach (var tweak in toApply)
            {
                try
                {
                    if (_tweakService.ApplyTweak(tweak)) applied++;
                }
                catch { }
            }
            
            UpdateCounts();
            DisplayTweaks();
            MessageBox.Show($"Applied {applied} of {toApply.Count} tweaks successfully!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RevertAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
            
            var toRevert = filtered.Where(t => t.IsActive).ToList();
            
            if (toRevert.Count == 0)
            {
                MessageBox.Show("No tweaks in this category are currently applied!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Revert {toRevert.Count} tweaks to Windows defaults?",
                "Confirm Revert All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;
                
            int reverted = 0;
            foreach (var tweak in toRevert)
            {
                try
                {
                    if (_tweakService.RevertTweak(tweak)) reverted++;
                }
                catch { }
            }
            
            UpdateCounts();
            DisplayTweaks();
            MessageBox.Show($"Reverted {reverted} tweaks to defaults!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
