using System.Windows;
using System.Windows.Media;

namespace AIO_PC_Tool_v2.Helpers;

/// <summary>
/// Theme Manager - Handles runtime theme switching for the application
/// Supports Light/Dark modes and multiple accent colors
/// </summary>
public static class ThemeManager
{
    // ═══════════════════════════════════════════════════════════════════════════════
    // ACCENT COLOR DEFINITIONS
    // ═══════════════════════════════════════════════════════════════════════════════
    
    public static readonly Dictionary<string, AccentColorSet> AccentColors = new()
    {
        ["Red"] = new AccentColorSet
        {
            Primary = "#EF4444",
            PrimaryHover = "#F87171",
            PrimaryDark = "#DC2626",
            PrimaryLight = "#FCA5A5",
            PrimarySubtle = "#7F1D1D",
            Secondary = "#F97316"
        },
        ["Blue"] = new AccentColorSet
        {
            Primary = "#3B82F6",
            PrimaryHover = "#60A5FA",
            PrimaryDark = "#2563EB",
            PrimaryLight = "#93C5FD",
            PrimarySubtle = "#1E3A8A",
            Secondary = "#06B6D4"
        },
        ["Purple"] = new AccentColorSet
        {
            Primary = "#A855F7",
            PrimaryHover = "#C084FC",
            PrimaryDark = "#9333EA",
            PrimaryLight = "#D8B4FE",
            PrimarySubtle = "#581C87",
            Secondary = "#6366F1"
        },
        ["Pink"] = new AccentColorSet
        {
            Primary = "#EC4899",
            PrimaryHover = "#F472B6",
            PrimaryDark = "#DB2777",
            PrimaryLight = "#F9A8D4",
            PrimarySubtle = "#831843",
            Secondary = "#A855F7"
        },
        ["Teal"] = new AccentColorSet
        {
            Primary = "#14B8A6",
            PrimaryHover = "#2DD4BF",
            PrimaryDark = "#0D9488",
            PrimaryLight = "#5EEAD4",
            PrimarySubtle = "#134E4A",
            Secondary = "#06B6D4"
        },
        ["Green"] = new AccentColorSet
        {
            Primary = "#22C55E",
            PrimaryHover = "#4ADE80",
            PrimaryDark = "#16A34A",
            PrimaryLight = "#86EFAC",
            PrimarySubtle = "#14532D",
            Secondary = "#10B981"
        },
        ["Orange"] = new AccentColorSet
        {
            Primary = "#F97316",
            PrimaryHover = "#FB923C",
            PrimaryDark = "#EA580C",
            PrimaryLight = "#FDBA74",
            PrimarySubtle = "#7C2D12",
            Secondary = "#FBBF24"
        },
        ["Gold"] = new AccentColorSet
        {
            Primary = "#FBBF24",
            PrimaryHover = "#FCD34D",
            PrimaryDark = "#F59E0B",
            PrimaryLight = "#FDE68A",
            PrimarySubtle = "#78350F",
            Secondary = "#F97316"
        },
        ["Cyan"] = new AccentColorSet
        {
            Primary = "#06B6D4",
            PrimaryHover = "#22D3EE",
            PrimaryDark = "#0891B2",
            PrimaryLight = "#67E8F9",
            PrimarySubtle = "#164E63",
            Secondary = "#3B82F6"
        }
    };

    // ═══════════════════════════════════════════════════════════════════════════════
    // DARK MODE COLORS
    // ═══════════════════════════════════════════════════════════════════════════════
    
    private static readonly Dictionary<string, string> DarkModeColors = new()
    {
        ["BackgroundDarkColor"] = "#0A0A0C",
        ["BackgroundCardColor"] = "#121216",
        ["BackgroundCardHoverColor"] = "#1A1A1F",
        ["BackgroundElevatedColor"] = "#161619",
        ["BackgroundInputColor"] = "#0D0D10",
        ["BackgroundSidebarColor"] = "#0E0E11",
        ["TextPrimaryColor"] = "#FAFAFA",
        ["TextSecondaryColor"] = "#A1A1AA",
        ["TextMutedColor"] = "#71717A",
        ["TextDisabledColor"] = "#52525B",
        ["BorderSubtleColor"] = "#1F1F23",
        ["BorderActiveColor"] = "#2D2D33",
        ["BorderFocusColor"] = "#3F3F46"
    };

    // ═══════════════════════════════════════════════════════════════════════════════
    // LIGHT MODE COLORS
    // ═══════════════════════════════════════════════════════════════════════════════
    
    private static readonly Dictionary<string, string> LightModeColors = new()
    {
        ["BackgroundDarkColor"] = "#FAFAFA",
        ["BackgroundCardColor"] = "#FFFFFF",
        ["BackgroundCardHoverColor"] = "#F4F4F5",
        ["BackgroundElevatedColor"] = "#F9FAFB",
        ["BackgroundInputColor"] = "#FFFFFF",
        ["BackgroundSidebarColor"] = "#F4F4F5",
        ["TextPrimaryColor"] = "#18181B",
        ["TextSecondaryColor"] = "#52525B",
        ["TextMutedColor"] = "#71717A",
        ["TextDisabledColor"] = "#A1A1AA",
        ["BorderSubtleColor"] = "#E4E4E7",
        ["BorderActiveColor"] = "#D4D4D8",
        ["BorderFocusColor"] = "#A1A1AA"
    };

    // ═══════════════════════════════════════════════════════════════════════════════
    // CURRENT STATE
    // ═══════════════════════════════════════════════════════════════════════════════
    
    public static bool IsDarkMode { get; private set; } = true;
    public static string CurrentAccent { get; private set; } = "Red";

    // ═══════════════════════════════════════════════════════════════════════════════
    // PUBLIC METHODS
    // ═══════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the application theme mode (Dark or Light)
    /// </summary>
    public static void SetThemeMode(bool isDark)
    {
        IsDarkMode = isDark;
        ApplyThemeColors(isDark ? DarkModeColors : LightModeColors);
    }

    /// <summary>
    /// Sets the accent color for the application
    /// </summary>
    public static void SetAccentColor(string accentName)
    {
        if (!AccentColors.TryGetValue(accentName, out var colors))
            return;

        CurrentAccent = accentName;
        
        var resources = Application.Current.Resources;
        
        resources["AccentPrimaryColor"] = ColorFromHex(colors.Primary);
        resources["AccentPrimaryHoverColor"] = ColorFromHex(colors.PrimaryHover);
        resources["AccentPrimaryDarkColor"] = ColorFromHex(colors.PrimaryDark);
        resources["AccentPrimaryLightColor"] = ColorFromHex(colors.PrimaryLight);
        resources["AccentPrimarySubtleColor"] = ColorFromHex(colors.PrimarySubtle);
        resources["AccentSecondaryColor"] = ColorFromHex(colors.Secondary);
        
        // Update the brushes
        resources["AccentPrimary"] = new SolidColorBrush(ColorFromHex(colors.Primary));
        resources["AccentPrimaryHover"] = new SolidColorBrush(ColorFromHex(colors.PrimaryHover));
        resources["AccentPrimaryDark"] = new SolidColorBrush(ColorFromHex(colors.PrimaryDark));
        resources["AccentPrimaryLight"] = new SolidColorBrush(ColorFromHex(colors.PrimaryLight));
        resources["AccentPrimarySubtle"] = new SolidColorBrush(ColorFromHex(colors.PrimarySubtle));
        resources["AccentSecondary"] = new SolidColorBrush(ColorFromHex(colors.Secondary));
        
        // Update gradient
        var gradient = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1)
        };
        gradient.GradientStops.Add(new GradientStop(ColorFromHex(colors.Primary), 0));
        gradient.GradientStops.Add(new GradientStop(ColorFromHex(colors.Secondary), 1));
        resources["AccentGradient"] = gradient;
        
        // Update accent glow
        resources["AccentGlow"] = new System.Windows.Media.Effects.DropShadowEffect
        {
            Color = ColorFromHex(colors.Primary),
            BlurRadius = 20,
            ShadowDepth = 0,
            Opacity = 0.5
        };
    }

    /// <summary>
    /// Gets all available accent color names
    /// </summary>
    public static IEnumerable<string> GetAccentColorNames() => AccentColors.Keys;

    /// <summary>
    /// Gets the display color for a given accent name (for color picker UI)
    /// </summary>
    public static Color GetAccentPreviewColor(string accentName)
    {
        return AccentColors.TryGetValue(accentName, out var colors) 
            ? ColorFromHex(colors.Primary) 
            : Colors.Red;
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // PRIVATE HELPERS
    // ═══════════════════════════════════════════════════════════════════════════════

    private static void ApplyThemeColors(Dictionary<string, string> colors)
    {
        var resources = Application.Current.Resources;
        
        foreach (var (key, hex) in colors)
        {
            resources[key] = ColorFromHex(hex);
            
            // Also update corresponding brush
            var brushKey = key.Replace("Color", "");
            if (resources.Contains(brushKey))
            {
                resources[brushKey] = new SolidColorBrush(ColorFromHex(hex));
            }
        }
    }

    private static Color ColorFromHex(string hex)
    {
        hex = hex.TrimStart('#');
        
        byte a = 255;
        byte r, g, b;
        
        if (hex.Length == 8)
        {
            a = Convert.ToByte(hex[..2], 16);
            r = Convert.ToByte(hex[2..4], 16);
            g = Convert.ToByte(hex[4..6], 16);
            b = Convert.ToByte(hex[6..8], 16);
        }
        else if (hex.Length == 6)
        {
            r = Convert.ToByte(hex[..2], 16);
            g = Convert.ToByte(hex[2..4], 16);
            b = Convert.ToByte(hex[4..6], 16);
        }
        else
        {
            return Colors.Transparent;
        }
        
        return Color.FromArgb(a, r, g, b);
    }
}

/// <summary>
/// Holds a set of accent colors for a theme
/// </summary>
public class AccentColorSet
{
    public string Primary { get; set; } = "#EF4444";
    public string PrimaryHover { get; set; } = "#F87171";
    public string PrimaryDark { get; set; } = "#DC2626";
    public string PrimaryLight { get; set; } = "#FCA5A5";
    public string PrimarySubtle { get; set; } = "#7F1D1D";
    public string Secondary { get; set; } = "#F97316";
}
