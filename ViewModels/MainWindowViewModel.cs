using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
    private ViewMode _currentViewMode = ViewMode.Grid;

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
        set => SetField(ref _isHorizontalOrientation, value);
    }

    public int ColumnsCount
    {
        get => _columnsCount;
        set => SetField(ref _columnsCount, value);
    }

    public ViewMode CurrentViewMode
    {
        get => _currentViewMode;
        set => SetField(ref _currentViewMode, value);
    }

    public string ViewModeButtonText => CurrentViewMode == ViewMode.Grid ? "Switch to List" : "Switch to Grid";

    public ICommand ToggleViewModeCommand { get; }

    private void ToggleViewMode()
    {
        CurrentViewMode = CurrentViewMode == ViewMode.Grid ? ViewMode.List : ViewMode.Grid;
        OnPropertyChanged(nameof(ViewModeButtonText));
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