using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ImageViewerApp.Models;
using ImageViewerApp.Services;

namespace ImageViewerApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ImageLoaderService _imageLoader;
    private ObservableCollection<ImageFile> _images;
    private bool _isHorizontalOrientation;
    private double _zoomLevel = 1.0;
    private double _hoverZoomLevel = 1.0;

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
        set => SetField(ref _isHorizontalOrientation, value);
    }

    public double ZoomLevel
    {
        get => _zoomLevel;
        set => SetField(ref _zoomLevel, value);
    }

    public double HoverZoomLevel
    {
        get => _hoverZoomLevel;
        set => SetField(ref _hoverZoomLevel, value);
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