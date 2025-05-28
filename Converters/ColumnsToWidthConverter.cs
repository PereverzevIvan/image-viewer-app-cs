using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp.Converters;

public class ColumnsToWidthConverter : IValueConverter
{
    public static readonly ColumnsToWidthConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isVertical && isVertical && parameter is MainWindowViewModel vm)
        {
            // В вертикальном режиме ширина зависит от максимального количества колонок
            return vm.MaxColumns * (200 + 10); // ширина элемента + отступы
        }
        return double.NaN; // В горизонтальном режиме не ограничиваем ширину
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 