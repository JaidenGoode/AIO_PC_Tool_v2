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
            
            try
            {
                _allTweaks = _tweakService.GetAllTweaks().ToList();
                
                foreach (var tweak in _allTweaks)
                {
                    try { _tweakService.CheckTweakStatus(tweak); } catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tweaks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            DisplayTweaks();
        }

        private void CategoryFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryFilter.SelectedItem is ComboBoxItem item)
            {
                _currentCategory = item.Content?.ToString() ?? "All";
                DisplayTweaks();
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
                Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 12)
            };

            var mainStack = new StackPanel();

            // Title row with badges
            var titleRow = new WrapPanel { Margin = new Thickness(0, 0, 0, 12) };
            
            titleRow.Children.Add(new TextBlock 
            { 
                Text = tweak.Title, 
                FontWeight = FontWeights.SemiBold, 
                FontSize = 16, 
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 12, 0)
            });
            
            // Category badge
            var categoryBadge = new Border
            {
                Background = GetCategoryColor(tweak.Category),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 4, 10, 4),
                Margin = new Thickness(0, 0, 8, 0)
            };
            categoryBadge.Child = new TextBlock 
            { 
                Text = tweak.Category.ToUpper(), 
                FontSize = 10, 
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(categoryBadge);
            
            // Status badge
            var statusBadge = new Border
            {
                Background = tweak.IsActive 
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94)) 
                    : new SolidColorBrush(Color.FromRgb(64, 64, 64)),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 4, 10, 4),
                Margin = new Thickness(0, 0, 8, 0)
            };
            statusBadge.Child = new TextBlock 
            { 
                Text = tweak.IsActive ? "OPTIMIZED" : "DEFAULT", 
                FontSize = 10, 
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(statusBadge);

            // Windows version badge
            var winBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 4, 10, 4)
            };
            winBadge.Child = new TextBlock 
            { 
                Text = $"Win {tweak.WindowsVersion}", 
                FontSize = 10, 
                Foreground = new SolidColorBrush(Color.FromRgb(140, 140, 140)),
                FontWeight = FontWeights.Medium
            };
            titleRow.Children.Add(winBadge);

            mainStack.Children.Add(titleRow);
            
            // Description
            mainStack.Children.Add(new TextBlock 
            { 
                Text = tweak.Description, 
                FontSize = 14, 
                Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            });
            
            // Warning if present
            if (!string.IsNullOrEmpty(tweak.Warning))
            {
                var warningBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(60, 40, 20)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(200, 120, 40)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(14, 10, 14, 10),
                    Margin = new Thickness(0, 0, 0, 12)
                };
                warningBorder.Child = new TextBlock
                {
                    Text = "⚠ " + tweak.Warning,
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 180, 100)),
                    TextWrapping = TextWrapping.Wrap
                };
                mainStack.Children.Add(warningBorder);
            }
            
            // Buttons
            var buttonRow = new StackPanel { Orientation = Orientation.Horizontal };
            
            if (!tweak.IsActive)
            {
                var applyBtn = new Button 
                { 
                    Content = "Apply Tweak", 
                    Background = new SolidColorBrush(Color.FromRgb(220, 38, 38)),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(20, 10, 20, 10),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = tweak
                };
                applyBtn.Click += ApplyTweak_Click;
                buttonRow.Children.Add(applyBtn);
            }
            else
            {
                var revertBtn = new Button 
                { 
                    Content = "Revert to Default", 
                    Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                    Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                    BorderThickness = new Thickness(1),
                    Padding = new Thickness(20, 10, 20, 10),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = tweak
                };
                revertBtn.Click += RevertTweak_Click;
                buttonRow.Children.Add(revertBtn);
            }
            
            mainStack.Children.Add(buttonRow);
            card.Child = mainStack;
            return card;
        }

        private SolidColorBrush GetCategoryColor(string category)
        {
            return category.ToLower() switch
            {
                "privacy" => new SolidColorBrush(Color.FromRgb(139, 92, 246)),
                "performance" => new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                "gaming" => new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                "visual" => new SolidColorBrush(Color.FromRgb(236, 72, 153)),
                "network" => new SolidColorBrush(Color.FromRgb(249, 115, 22)),
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
                try
                {
                    _tweakService.ApplyTweak(tweak);
                    DisplayTweaks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to apply tweak: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RevertTweak_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tweak tweak)
            {
                try
                {
                    _tweakService.RevertTweak(tweak);
                    DisplayTweaks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to revert tweak: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ApplyAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
                
            foreach (var tweak in filtered.Where(t => !t.IsActive))
            {
                try { _tweakService.ApplyTweak(tweak); } catch { }
            }
            DisplayTweaks();
        }

        private void RevertAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
                
            foreach (var tweak in filtered.Where(t => t.IsActive))
            {
                try { _tweakService.RevertTweak(tweak); } catch { }
            }
            DisplayTweaks();
        }
    }
}
