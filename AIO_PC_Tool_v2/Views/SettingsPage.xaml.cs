using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AIO_PC_Tool_v2.Views
{
    public partial class SettingsPage : Page
    {
        private string _selectedColor = "Red";
        private Dictionary<string, Border> _colorBorders = new();

        public SettingsPage()
        {
            InitializeComponent();
            InitializeColorBorders();
        }

        private void InitializeColorBorders()
        {
            _colorBorders["Red"] = ColorRed;
            _colorBorders["Blue"] = ColorBlue;
            _colorBorders["Green"] = ColorGreen;
            _colorBorders["Purple"] = ColorPurple;
            _colorBorders["Pink"] = ColorPink;
            _colorBorders["Orange"] = ColorOrange;
            _colorBorders["Teal"] = ColorTeal;
            _colorBorders["Cyan"] = ColorCyan;

            // Add hover effects to all color borders
            foreach (var kvp in _colorBorders)
            {
                var border = kvp.Value;
                var color = kvp.Key;
                
                border.MouseEnter += (s, e) =>
                {
                    if (color != _selectedColor)
                    {
                        border.RenderTransform = new ScaleTransform(1.1, 1.1);
                        border.RenderTransformOrigin = new Point(0.5, 0.5);
                    }
                };
                
                border.MouseLeave += (s, e) =>
                {
                    border.RenderTransform = null;
                };
            }
        }

        private void AccentColor_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string color)
            {
                _selectedColor = color;
                UpdateColorSelection();
                
                // Apply the theme color to the app (this would be more elaborate in production)
                // For now, we just update the visual selection
                MessageBox.Show(
                    $"Accent color changed to {color}!\n\nNote: Full theme application requires app restart.",
                    "Theme Updated",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void UpdateColorSelection()
        {
            foreach (var kvp in _colorBorders)
            {
                var border = kvp.Value;
                var color = kvp.Key;
                
                if (color == _selectedColor)
                {
                    border.BorderBrush = border.Background;
                    border.BorderThickness = new Thickness(3);
                    
                    // Add checkmark
                    border.Child = new TextBlock 
                    { 
                        Text = "✓", 
                        Foreground = Brushes.White, 
                        HorizontalAlignment = HorizontalAlignment.Center, 
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        FontSize = 16
                    };
                }
                else
                {
                    border.BorderBrush = Brushes.Transparent;
                    border.BorderThickness = new Thickness(3);
                    border.Child = null;
                }
            }
        }
    }
}
