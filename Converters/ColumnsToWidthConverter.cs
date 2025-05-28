using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ImageViewerApp.Converters;

public class ColumnsToWidthConverter : IValueConverter
{
    public static readonly ColumnsToWidthConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int columns)
        {
            // 200 - ширина элемента, 10 - отступы (5 + 5)
            return columns * (200 + 10);
        }
        return 420; // Минимальная ширина для 2 колонок
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 