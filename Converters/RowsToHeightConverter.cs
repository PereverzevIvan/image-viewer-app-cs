using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp.Converters;

public class RowsToHeightConverter : IValueConverter
{
    public static readonly RowsToHeightConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isHorizontal && isHorizontal && parameter is MainWindowViewModel vm)
        {
            // В горизонтальном режиме высота зависит от максимального количества строк
            return vm.MaxRows * (200 + 10); // высота элемента + отступы
        }
        return double.NaN; // В вертикальном режиме не ограничиваем высоту
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 