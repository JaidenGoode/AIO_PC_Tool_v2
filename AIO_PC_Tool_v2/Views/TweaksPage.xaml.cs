using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIO_PC_Tool_v2.Models;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class TweaksPage : Page
    {
        private readonly TweakService _tweakService;
        private List<Tweak> _allTweaks;
        private string _currentCategory = "All";

        public TweaksPage()
        {
            InitializeComponent();
            _tweakService = new TweakService();
            _allTweaks = new List<Tweak>();
            
            LoadTweaks();
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
                
                UpdateActiveCount();
                DisplayTweaks();
                HighlightCategory("All");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tweaks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateActiveCount()
        {
            var activeCount = _allTweaks.Count(t => t.IsActive);
            ActiveCount.Text = activeCount.ToString();
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
            var buttons = new[] { CatAll, CatGaming, CatPerformance, CatPrivacy, CatNetwork, CatVisual, CatPower, CatSecurity, CatStorage };
            foreach (var btn in buttons)
            {
                btn.Background = btn.Tag?.ToString() == category 
                    ? new SolidColorBrush(Color.FromRgb(220, 38, 38)) 
                    : Brushes.Transparent;
                btn.Foreground = btn.Tag?.ToString() == category 
                    ? Brushes.White 
                    : new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
        }

        private void DisplayTweaks()
        {
            TweaksList.Children.Clear();
            
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);

            foreach (var tweak in filtered)
            {
                TweaksList.Children.Add(CreateTweakCard(tweak));
            }
        }

        private Border CreateTweakCard(Tweak tweak)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Left content
            var content = new StackPanel();

            // Title row
            var titleRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 8) };
            
            // Status indicator
            var statusDot = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94)) 
                    : new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                Margin = new Thickness(0, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            titleRow.Children.Add(statusDot);

            titleRow.Children.Add(new TextBlock 
            { 
                Text = tweak.Title, 
                FontWeight = FontWeights.SemiBold, 
                FontSize = 14, 
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            });
            
            // Category badge
            var categoryBadge = new Border
            {
                Background = GetCategoryColor(tweak.Category),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 3, 8, 3),
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            categoryBadge.Child = new TextBlock 
            { 
                Text = tweak.Category.ToUpper(), 
                FontSize = 9, 
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(categoryBadge);
            
            // Status badge
            var statusBadge = new Border
            {
                Background = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromArgb(40, 34, 197, 94))
                    : new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 3, 8, 3),
                Margin = new Thickness(6, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            statusBadge.Child = new TextBlock 
            { 
                Text = tweak.IsActive ? "✓ OPTIMIZED" : "DEFAULT", 
                FontSize = 9, 
                Foreground = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
                    : new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(statusBadge);

            // Windows version
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
                Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                FontWeight = FontWeights.Medium
            };
            titleRow.Children.Add(winBadge);

            content.Children.Add(titleRow);
            
            // Description
            content.Children.Add(new TextBlock 
            { 
                Text = tweak.Description, 
                FontSize = 12, 
                Foreground = new SolidColorBrush(Color.FromRgb(140, 140, 140)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(18, 0, 0, 0),
                MaxWidth = 700
            });
            
            // Warning
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

            // Right buttons
            var buttonPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            if (!tweak.IsActive)
            {
                var applyBtn = CreateButton("Apply", true, tweak);
                applyBtn.Click += ApplyTweak_Click;
                buttonPanel.Children.Add(applyBtn);
            }
            else
            {
                var revertBtn = CreateButton("Revert", false, tweak);
                revertBtn.Click += RevertTweak_Click;
                buttonPanel.Children.Add(revertBtn);
            }
            
            Grid.SetColumn(buttonPanel, 1);
            grid.Children.Add(buttonPanel);

            card.Child = grid;
            return card;
        }

        private Button CreateButton(string text, bool isPrimary, Tweak tweak)
        {
            return new Button
            {
                Content = text,
                Background = isPrimary 
                    ? new SolidColorBrush(Color.FromRgb(220, 38, 38))
                    : new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                Foreground = isPrimary ? Brushes.White : new SolidColorBrush(Color.FromRgb(160, 160, 160)),
                BorderThickness = new Thickness(0),
                Padding = new Thickness(20, 10, 20, 10),
                Cursor = System.Windows.Input.Cursors.Hand,
                Tag = tweak
            };
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

        private void ApplyTweak_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tweak tweak)
            {
                if (_tweakService.ApplyTweak(tweak))
                {
                    UpdateActiveCount();
                    DisplayTweaks();
                }
                else
                {
                    MessageBox.Show("Failed to apply tweak. Try running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void RevertTweak_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tweak tweak)
            {
                if (_tweakService.RevertTweak(tweak))
                {
                    UpdateActiveCount();
                    DisplayTweaks();
                }
                else
                {
                    MessageBox.Show("Failed to revert tweak. Try running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ApplyAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
                
            int applied = 0;
            foreach (var tweak in filtered.Where(t => !t.IsActive))
            {
                if (_tweakService.ApplyTweak(tweak)) applied++;
            }
            
            UpdateActiveCount();
            DisplayTweaks();
            MessageBox.Show($"Applied {applied} tweaks successfully!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RevertAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
                
            int reverted = 0;
            foreach (var tweak in filtered.Where(t => t.IsActive))
            {
                if (_tweakService.RevertTweak(tweak)) reverted++;
            }
            
            UpdateActiveCount();
            DisplayTweaks();
            MessageBox.Show($"Reverted {reverted} tweaks to defaults!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Ellipse : System.Windows.Shapes.Ellipse { }
}
