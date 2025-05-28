using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp.Converters;

public class ViewModeToListVisibilityConverter : IValueConverter
{
    public static readonly ViewModeToListVisibilityConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ViewMode mode)
        {
            return mode == ViewMode.List;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 