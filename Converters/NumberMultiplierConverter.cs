using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ImageViewerApp.Converters;

public class NumberMultiplierConverter : IValueConverter
{
    public static readonly NumberMultiplierConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double number && parameter is string multiplierStr)
        {
            if (double.TryParse(multiplierStr, out double multiplier))
            {
                return number * multiplier;
            }
        }
        return 200.0; // Default size
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 