using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using ImageViewerApp.Models;
using ImageViewerApp.Services;

namespace ImageViewerApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ImageLoaderService _imageLoader;
    private ObservableCollection<ImageFile> _images;
    private bool _isHorizontalOrientation;
    private double _windowWidth;
    private double _windowHeight;
    private int _maxColumns;
    private int _maxRows;

    public MainWindowViewModel()
    {
        _imageLoader = new ImageLoaderService();
        _images = new ObservableCollection<ImageFile>();
    }

    public ObservableCollection<ImageFile> Images
    {
        get => _images;
        set => SetField(ref _images, value);
    }

    public bool IsHorizontalOrientation
    {
        get => _isHorizontalOrientation;
        set
        {
            if (SetField(ref _isHorizontalOrientation, value))
            {
                RecalculateMaxDimensions();
            }
        }
    }

    public double WindowWidth
    {
        get => _windowWidth;
        set
        {
            if (SetField(ref _windowWidth, value))
            {
                RecalculateMaxDimensions();
            }
        }
    }

    public double WindowHeight
    {
        get => _windowHeight;
        set
        {
            if (SetField(ref _windowHeight, value))
            {
                RecalculateMaxDimensions();
            }
        }
    }

    public int MaxColumns
    {
        get => _maxColumns;
        private set => SetField(ref _maxColumns, value);
    }

    public int MaxRows
    {
        get => _maxRows;
        private set => SetField(ref _maxRows, value);
    }

    private void RecalculateMaxDimensions()
    {
        const int itemWidth = 210; // 200 + 10 отступы
        const int itemHeight = 210;
        const int minDimension = 2;

        MaxColumns = Math.Max(minDimension, (int)(WindowWidth / itemWidth));
        MaxRows = Math.Max(minDimension, (int)(WindowHeight / itemHeight));
    }

    public async Task LoadImagesAsync(string[] paths)
    {
        Images = await _imageLoader.LoadImagesAsync(paths);
    }

    public async Task LoadImagesFromDirectoryAsync(string directory)
    {
        Images = await _imageLoader.LoadImagesFromDirectoryAsync(directory);
    }
} 