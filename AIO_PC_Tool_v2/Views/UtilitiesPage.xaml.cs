using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using AIO_PC_Tool_v2.Services;
using AIO_PC_Tool_v2.Models;

namespace AIO_PC_Tool_v2.Views
{
    public partial class UtilitiesPage : Page
    {
        private readonly UtilitiesService _utilitiesService;

        public UtilitiesPage()
        {
            InitializeComponent();
            _utilitiesService = new UtilitiesService();
            DisplayUtilities();
        }

        private void DisplayUtilities()
        {
            var utilities = _utilitiesService.GetUtilities();
            var groups = utilities.GroupBy(u => u.Category).OrderBy(g => GetCategoryOrder(g.Key));

            foreach (var group in groups)
            {
                // Section Header
                var header = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 16, 0, 12) };
                header.Children.Add(new TextBlock 
                { 
                    Text = GetCategoryIcon(group.Key), 
                    FontSize = 14, 
                    Foreground = new SolidColorBrush(GetCategoryColor(group.Key)),
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
                UtilitiesList.Children.Add(header);

                // Cards Grid
                var grid = new UniformGrid { Columns = 2 };

                foreach (var utility in group)
                {
                    grid.Children.Add(CreateUtilityCard(utility));
                }

                UtilitiesList.Children.Add(grid);
            }
        }

        private Border CreateUtilityCard(UtilityItem utility)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(20, 16, 20, 16),
                Margin = new Thickness(0, 0, 10, 10),
                Cursor = Cursors.Hand
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
            card.MouseLeave += (s, e) => card.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var content = new StackPanel();
            
            // Title row
            var titleRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 6) };
            titleRow.Children.Add(new TextBlock 
            { 
                Text = utility.Name, 
                FontWeight = FontWeights.SemiBold, 
                FontSize = 14, 
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            });
            
            // Category badge
            var categoryBadge = new Border 
            { 
                Background = new SolidColorBrush(Color.FromRgb(35, 35, 35)), 
                CornerRadius = new CornerRadius(4), 
                Padding = new Thickness(8, 3, 8, 3),
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            categoryBadge.Child = new TextBlock 
            { 
                Text = utility.Category.ToUpper(), 
                FontSize = 9, 
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                FontWeight = FontWeights.Bold
            };
            titleRow.Children.Add(categoryBadge);
            
            content.Children.Add(titleRow);
            
            // Description
            content.Children.Add(new TextBlock 
            { 
                Text = utility.Description, 
                FontSize = 12, 
                Foreground = new SolidColorBrush(Color.FromRgb(130, 130, 130)), 
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400
            });
            
            Grid.SetColumn(content, 0);
            grid.Children.Add(content);

            // Run button
            var runBtn = new Button 
            { 
                Content = "Run", 
                Background = new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                BorderThickness = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 10, 20, 10),
                Cursor = Cursors.Hand,
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                Tag = utility
            };

            // Create template for rounded corners
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
            runBtn.Template = template;

            // Hover effects
            runBtn.MouseEnter += (s, e) =>
            {
                runBtn.Background = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                runBtn.Foreground = Brushes.White;
            };
            runBtn.MouseLeave += (s, e) =>
            {
                runBtn.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                runBtn.Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            };

            runBtn.Click += (s, e) =>
            {
                try
                {
                    utility.Action?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to run utility: {ex.Message}\n\nTry running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            Grid.SetColumn(runBtn, 1);
            grid.Children.Add(runBtn);

            card.Child = grid;
            return card;
        }

        private void LaunchCTT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _utilitiesService.RunChrisTitusUtility();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch Chris Titus Tech Utility: {ex.Message}\n\nTry running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private int GetCategoryOrder(string category)
        {
            return category switch
            {
                "Repair" => 0,
                "Network" => 1,
                "Cleanup" => 2,
                "Optimization" => 3,
                _ => 99
            };
        }

        private string GetCategoryIcon(string category)
        {
            return category switch
            {
                "Repair" => "🔧",
                "Network" => "🌐",
                "Cleanup" => "🧹",
                "Optimization" => "⚡",
                _ => "📁"
            };
        }

        private Color GetCategoryColor(string category)
        {
            return category switch
            {
                "Repair" => Color.FromRgb(59, 130, 246),    // Blue
                "Network" => Color.FromRgb(139, 92, 246),   // Purple
                "Cleanup" => Color.FromRgb(34, 197, 94),    // Green
                "Optimization" => Color.FromRgb(249, 115, 22), // Orange
                _ => Color.FromRgb(107, 114, 128)
            };
        }
    }
}
