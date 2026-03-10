using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIO_PC_Tool_v2.Services;

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
            var grid = new UniformGrid { Columns = 2 };

            foreach (var utility in utilities)
            {
                var card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16),
                    Margin = new Thickness(0, 0, 8, 8)
                };

                var cardGrid = new Grid();
                cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var info = new StackPanel();
                
                var titleRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };
                titleRow.Children.Add(new TextBlock { Text = utility.Name, FontWeight = FontWeights.SemiBold, FontSize = 14, Foreground = Brushes.White });
                
                var categoryBadge = new Border 
                { 
                    Background = new SolidColorBrush(Color.FromRgb(37, 37, 37)), 
                    CornerRadius = new CornerRadius(4), 
                    Padding = new Thickness(6, 2, 6, 2),
                    Margin = new Thickness(8, 0, 0, 0)
                };
                categoryBadge.Child = new TextBlock { Text = utility.Category, FontSize = 10, Foreground = new SolidColorBrush(Color.FromRgb(115, 115, 115)) };
                titleRow.Children.Add(categoryBadge);
                
                info.Children.Add(titleRow);
                info.Children.Add(new TextBlock { Text = utility.Description, FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(163, 163, 163)), TextWrapping = TextWrapping.Wrap });
                
                Grid.SetColumn(info, 0);
                cardGrid.Children.Add(info);

                var runBtn = new Button 
                { 
                    Content = "Run", 
                    Style = (Style)Application.Current.Resources["SecondaryButtonStyle"],
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(16, 8, 16, 8),
                    Tag = utility
                };
                runBtn.Click += (s, e) => utility.Action?.Invoke();
                Grid.SetColumn(runBtn, 1);
                cardGrid.Children.Add(runBtn);

                card.Child = cardGrid;
                grid.Children.Add(card);
            }

            UtilitiesList.Children.Add(grid);
        }

        private void LaunchCTT_Click(object sender, RoutedEventArgs e)
        {
            _utilitiesService.RunChrisTitusUtility();
        }
    }
}
