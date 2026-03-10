using System.Windows;

namespace AIO_PC_Tool_v2.Helpers
{
    public static class ThemeManager
    {
        public static void SetTheme(bool isDark, string accentColor)
        {
            var app = Application.Current;
            if (app?.Resources == null) return;

            // Set background colors based on dark/light mode
            if (isDark)
            {
                app.Resources["BackgroundMainColor"] = System.Windows.Media.Color.FromRgb(13, 13, 13);
                app.Resources["BackgroundCardColor"] = System.Windows.Media.Color.FromRgb(26, 26, 26);
                app.Resources["BackgroundCardHoverColor"] = System.Windows.Media.Color.FromRgb(37, 37, 37);
                app.Resources["BackgroundInputColor"] = System.Windows.Media.Color.FromRgb(42, 42, 42);
                app.Resources["TextPrimaryColor"] = System.Windows.Media.Color.FromRgb(255, 255, 255);
                app.Resources["TextSecondaryColor"] = System.Windows.Media.Color.FromRgb(163, 163, 163);
                app.Resources["TextMutedColor"] = System.Windows.Media.Color.FromRgb(115, 115, 115);
                app.Resources["BorderDefaultColor"] = System.Windows.Media.Color.FromRgb(51, 51, 51);
            }
            else
            {
                app.Resources["BackgroundMainColor"] = System.Windows.Media.Color.FromRgb(250, 250, 250);
                app.Resources["BackgroundCardColor"] = System.Windows.Media.Color.FromRgb(255, 255, 255);
                app.Resources["BackgroundCardHoverColor"] = System.Windows.Media.Color.FromRgb(245, 245, 245);
                app.Resources["BackgroundInputColor"] = System.Windows.Media.Color.FromRgb(240, 240, 240);
                app.Resources["TextPrimaryColor"] = System.Windows.Media.Color.FromRgb(23, 23, 23);
                app.Resources["TextSecondaryColor"] = System.Windows.Media.Color.FromRgb(82, 82, 82);
                app.Resources["TextMutedColor"] = System.Windows.Media.Color.FromRgb(115, 115, 115);
                app.Resources["BorderDefaultColor"] = System.Windows.Media.Color.FromRgb(229, 229, 229);
            }

            // Set accent colors
            var (primary, secondary, tertiary) = GetAccentColors(accentColor);
            app.Resources["AccentPrimaryColor"] = primary;
            app.Resources["AccentSecondaryColor"] = secondary;
            app.Resources["AccentTertiaryColor"] = tertiary;
        }

        private static (System.Windows.Media.Color, System.Windows.Media.Color, System.Windows.Media.Color) GetAccentColors(string accent)
        {
            return accent.ToLower() switch
            {
                "red" => (
                    System.Windows.Media.Color.FromRgb(220, 38, 38),
                    System.Windows.Media.Color.FromRgb(239, 68, 68),
                    System.Windows.Media.Color.FromRgb(252, 165, 165)
                ),
                "blue" => (
                    System.Windows.Media.Color.FromRgb(59, 130, 246),
                    System.Windows.Media.Color.FromRgb(96, 165, 250),
                    System.Windows.Media.Color.FromRgb(191, 219, 254)
                ),
                "green" => (
                    System.Windows.Media.Color.FromRgb(34, 197, 94),
                    System.Windows.Media.Color.FromRgb(74, 222, 128),
                    System.Windows.Media.Color.FromRgb(187, 247, 208)
                ),
                "purple" => (
                    System.Windows.Media.Color.FromRgb(139, 92, 246),
                    System.Windows.Media.Color.FromRgb(167, 139, 250),
                    System.Windows.Media.Color.FromRgb(221, 214, 254)
                ),
                "pink" => (
                    System.Windows.Media.Color.FromRgb(236, 72, 153),
                    System.Windows.Media.Color.FromRgb(244, 114, 182),
                    System.Windows.Media.Color.FromRgb(251, 207, 232)
                ),
                "orange" => (
                    System.Windows.Media.Color.FromRgb(249, 115, 22),
                    System.Windows.Media.Color.FromRgb(251, 146, 60),
                    System.Windows.Media.Color.FromRgb(254, 215, 170)
                ),
                "teal" => (
                    System.Windows.Media.Color.FromRgb(20, 184, 166),
                    System.Windows.Media.Color.FromRgb(45, 212, 191),
                    System.Windows.Media.Color.FromRgb(153, 246, 228)
                ),
                _ => (
                    System.Windows.Media.Color.FromRgb(220, 38, 38),
                    System.Windows.Media.Color.FromRgb(239, 68, 68),
                    System.Windows.Media.Color.FromRgb(252, 165, 165)
                )
            };
        }
    }
}
