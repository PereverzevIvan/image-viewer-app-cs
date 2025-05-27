using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace ImageViewerApp.Converters;

public class OrientationConverter : IValueConverter
{
    public static readonly OrientationConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isHorizontal)
        {
            return isHorizontal ? Orientation.Horizontal : Orientation.Vertical;
        }
        return Orientation.Vertical;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 