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
            _allTweaks = _tweakService.GetAllTweaks().ToList();
            
            foreach (var tweak in _allTweaks)
            {
                _tweakService.CheckTweakStatus(tweak);
            }
            
            DisplayTweaks();
        }

        private void CategoryFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryFilter.SelectedItem is ComboBoxItem item)
            {
                _currentCategory = item.Content.ToString() ?? "All";
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
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 0, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Content
            var content = new StackPanel();
            
            // Title row
            var titleRow = new StackPanel { Orientation = Orientation.Horizontal };
            titleRow.Children.Add(new TextBlock 
            { 
                Text = tweak.Title, 
                FontWeight = FontWeights.SemiBold, 
                FontSize = 15, 
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            });
            
            // Category badge
            var categoryBadge = new Border
            {
                Background = GetCategoryColor(tweak.Category),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 2, 8, 2),
                Margin = new Thickness(12, 0, 0, 0)
            };
            categoryBadge.Child = new TextBlock 
            { 
                Text = tweak.Category, 
                FontSize = 10, 
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(categoryBadge);
            
            // Status badge
            var statusBadge = new Border
            {
                Background = tweak.IsActive ? new SolidColorBrush(Color.FromRgb(34, 197, 94)) : new SolidColorBrush(Color.FromRgb(64, 64, 64)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 2, 8, 2),
                Margin = new Thickness(8, 0, 0, 0)
            };
            statusBadge.Child = new TextBlock 
            { 
                Text = tweak.IsActive ? "OPTIMIZED" : "DEFAULT", 
                FontSize = 10, 
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(statusBadge);
            
            content.Children.Add(titleRow);
            
            // Description
            content.Children.Add(new TextBlock 
            { 
                Text = tweak.Description, 
                FontSize = 13, 
                Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 8, 0, 0),
                MaxWidth = 600
            });
            
            // Warning if present
            if (!string.IsNullOrEmpty(tweak.Warning))
            {
                var warningBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(254, 243, 199)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(12, 8, 12, 8),
                    Margin = new Thickness(0, 8, 0, 0)
                };
                warningBorder.Child = new TextBlock
                {
                    Text = "⚠ " + tweak.Warning,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(146, 64, 14)),
                    TextWrapping = TextWrapping.Wrap
                };
                content.Children.Add(warningBorder);
            }
            
            Grid.SetColumn(content, 0);
            grid.Children.Add(content);

            // Buttons
            var buttons = new StackPanel 
            { 
                Orientation = Orientation.Horizontal, 
                VerticalAlignment = VerticalAlignment.Center 
            };
            
            var applyBtn = new Button 
            { 
                Content = "Apply", 
                Style = (Style)Application.Current.Resources["PrimaryButtonStyle"],
                Visibility = tweak.IsActive ? Visibility.Collapsed : Visibility.Visible,
                Tag = tweak
            };
            applyBtn.Click += ApplyTweak_Click;
            
            var revertBtn = new Button 
            { 
                Content = "Revert", 
                Style = (Style)Application.Current.Resources["SecondaryButtonStyle"],
                Visibility = tweak.IsActive ? Visibility.Visible : Visibility.Collapsed,
                Tag = tweak
            };
            revertBtn.Click += RevertTweak_Click;
            
            buttons.Children.Add(applyBtn);
            buttons.Children.Add(revertBtn);
            
            Grid.SetColumn(buttons, 1);
            grid.Children.Add(buttons);

            card.Child = grid;
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
                _tweakService.ApplyTweak(tweak);
                DisplayTweaks();
            }
        }

        private void RevertTweak_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tweak tweak)
            {
                _tweakService.RevertTweak(tweak);
                DisplayTweaks();
            }
        }

        private void ApplyAll_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _currentCategory == "All" 
                ? _allTweaks 
                : _allTweaks.Where(t => t.Category == _currentCategory);
                
            foreach (var tweak in filtered.Where(t => !t.IsActive))
            {
                _tweakService.ApplyTweak(tweak);
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
                _tweakService.RevertTweak(tweak);
            }
            DisplayTweaks();
        }
    }
}
