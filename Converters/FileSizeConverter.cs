using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ImageViewerApp.Converters;

public class FileSizeConverter : IValueConverter
{
    public static readonly FileSizeConverter Instance = new();
    private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB" };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long size) return "0 bytes";

        int order = 0;
        double len = size;

        while (len >= 1024 && order < SizeSuffixes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {SizeSuffixes[order]}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 