using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using ImageViewerApp.Models;
using ImageViewerApp.Services;
using ImageViewerApp.Commands;

namespace ImageViewerApp.ViewModels;

public enum ViewMode
{
    Grid,
    List
}

public class MainWindowViewModel : ViewModelBase
{
    private readonly ImageLoaderService _imageLoader;
    private ObservableCollection<ImageFile> _images;
    private bool _isHorizontalOrientation;
    private int _columnsCount = 4;
    private int _maxColumnsCount = 6;
    private ViewMode _currentViewMode = ViewMode.Grid;
    private double _windowWidth;
    private double _windowHeight;

    public MainWindowViewModel()
    {
        _imageLoader = new ImageLoaderService();
        _images = new ObservableCollection<ImageFile>();
        ToggleViewModeCommand = new RelayCommand(ToggleViewMode);
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
                RecalculateMaxColumns();
            }
        }
    }

    public int ColumnsCount
    {
        get => _columnsCount;
        set => SetField(ref _columnsCount, Math.Min(value, MaxColumnsCount));
    }

    public int MaxColumnsCount
    {
        get => _maxColumnsCount;
        private set
        {
            if (SetField(ref _maxColumnsCount, value))
            {
                // Если текущее количество колонок больше максимального, уменьшаем его
                if (ColumnsCount > value)
                {
                    ColumnsCount = value;
                }
            }
        }
    }

    public ViewMode CurrentViewMode
    {
        get => _currentViewMode;
        set => SetField(ref _currentViewMode, value);
    }

    public string ViewModeButtonText => CurrentViewMode == ViewMode.Grid ? "Switch to List" : "Switch to Grid";

    public double WindowWidth
    {
        get => _windowWidth;
        set
        {
            if (SetField(ref _windowWidth, value))
            {
                RecalculateMaxColumns();
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
                RecalculateMaxColumns();
            }
        }
    }

    public ICommand ToggleViewModeCommand { get; }

    private void ToggleViewMode()
    {
        CurrentViewMode = CurrentViewMode == ViewMode.Grid ? ViewMode.List : ViewMode.Grid;
        OnPropertyChanged(nameof(ViewModeButtonText));
    }

    private void RecalculateMaxColumns()
    {
        const int itemWidth = 210; // 200 + 10 отступы
        const int itemHeight = 210;
        const int minColumns = 2;

        if (IsHorizontalOrientation)
        {
            // В горизонтальном режиме ограничиваем количество строк по высоте окна
            int maxRows = Math.Max(minColumns, (int)(WindowHeight / itemHeight));
            MaxColumnsCount = maxRows;
        }
        else
        {
            // В вертикальном режиме ограничиваем количество колонок по ширине окна
            int maxColumns = Math.Max(minColumns, (int)(WindowWidth / itemWidth));
            MaxColumnsCount = maxColumns;
        }
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