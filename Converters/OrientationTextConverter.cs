using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ImageViewerApp.Converters;

public class OrientationTextConverter : IValueConverter
{
    public static readonly OrientationTextConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isHorizontal)
        {
            return isHorizontal ? "Horizontal View" : "Vertical View";
        }
        return "Vertical View";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 