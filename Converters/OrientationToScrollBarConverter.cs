using System;
using System.Globalization;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;

namespace ImageViewerApp.Converters;

public class OrientationToScrollBarConverter : IValueConverter
{
    public static readonly OrientationToScrollBarConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isHorizontal)
        {
            return isHorizontal ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
        }
        return ScrollBarVisibility.Disabled;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 