using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AIO_PC_Tool_v2.Helpers;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility v && v == Visibility.Visible;
    }
}

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }
}

public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null && !string.IsNullOrEmpty(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToStartStopConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b ? "Stop Monitoring" : "Start Monitoring";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToAccentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b 
            ? new SolidColorBrush(Color.FromRgb(139, 92, 246)) // AccentPrimary for active
            : new SolidColorBrush(Color.FromArgb(30, 255, 255, 255)); // Subtle border
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToAppliedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b ? "Applied" : "Apply";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToSuccessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b 
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94)) // SuccessGreen
            : new SolidColorBrush(Color.FromRgb(139, 92, 246)); // AccentPrimary
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class MultiBoolToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        foreach (var value in values)
        {
            if (value is bool b && b)
                return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToOptimizedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b ? "OPTIMIZED" : "DEFAULT";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToOptimizeRevertConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b ? "REVERT" : "OPTIMIZE";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class HideCttConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string s && s == "ctt-utility" ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class CategoryToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString()?.ToLower() switch
        {
            "privacy" => new SolidColorBrush(Color.FromRgb(16, 185, 129)),     // Green
            "performance" => new SolidColorBrush(Color.FromRgb(59, 130, 246)), // Blue
            "gaming" => new SolidColorBrush(Color.FromRgb(139, 92, 246)),      // Purple
            "system" => new SolidColorBrush(Color.FromRgb(6, 182, 212)),       // Cyan
            "network" => new SolidColorBrush(Color.FromRgb(249, 115, 22)),     // Orange
            "services" => new SolidColorBrush(Color.FromRgb(236, 72, 153)),    // Pink
            "repair" => new SolidColorBrush(Color.FromRgb(234, 179, 8)),       // Yellow
            "tools" => new SolidColorBrush(Color.FromRgb(99, 102, 241)),       // Indigo
            "power" => new SolidColorBrush(Color.FromRgb(239, 68, 68)),        // Red
            "quick" => new SolidColorBrush(Color.FromRgb(34, 197, 94)),        // Green
            "recommended" => new SolidColorBrush(Color.FromRgb(139, 92, 246)), // Purple
            _ => new SolidColorBrush(Color.FromRgb(139, 92, 246))              // Default Purple
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToStatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b 
            ? new SolidColorBrush(Color.FromArgb(40, 34, 197, 94))  // Green tint
            : new SolidColorBrush(Color.FromRgb(26, 26, 36));       // Card hover
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToStatusTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b 
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))   // Green
            : new SolidColorBrush(Color.FromRgb(156, 163, 175)); // Gray
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
