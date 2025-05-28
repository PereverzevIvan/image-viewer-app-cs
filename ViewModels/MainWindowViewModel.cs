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
    private int _columnsCount = 4;

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

    public int ColumnsCount
    {
        get => _columnsCount;
        set => SetField(ref _columnsCount, value);
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