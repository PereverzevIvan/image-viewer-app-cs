using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Avalonia;
using ImageViewerApp.Models;
using ImageViewerApp.Services;
using ImageViewerApp.Commands;
using System.Windows.Input;

namespace ImageViewerApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ImageLoaderService _imageLoader;
    private ObservableCollection<ImageFile> _images;
    private ObservableCollection<ImageFile> _filteredImages;
    private bool _isHorizontalOrientation;
    private double _windowWidth;
    private double _windowHeight;
    private int _maxColumns;
    private int _maxRows;
    private string _searchQuery = "";
    private string _selectedFileType = "All";
    private SortType _sortType = SortType.Name;
    private bool _sortDescending;

    public MainWindowViewModel()
    {
        _imageLoader = new ImageLoaderService();
        _images = new ObservableCollection<ImageFile>();
        _filteredImages = new ObservableCollection<ImageFile>();
        
        SortCommand = new RelayCommand(ApplySort);
        ToggleSortDirectionCommand = new RelayCommand(() => SortDescending = !SortDescending);
    }

    public ObservableCollection<ImageFile> Images
    {
        get => _filteredImages;
        private set
        {
            if (SetField(ref _filteredImages, value))
            {
                Console.WriteLine($"Images property changed. New count: {value?.Count ?? 0}");
            }
        }
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetField(ref _searchQuery, value))
            {
                ApplyFilters();
            }
        }
    }

    public string SelectedFileType
    {
        get => _selectedFileType;
        set
        {
            if (SetField(ref _selectedFileType, value))
            {
                ApplyFilters();
            }
        }
    }

    public SortType SortType
    {
        get => _sortType;
        set
        {
            if (SetField(ref _sortType, value))
            {
                ApplySort();
            }
        }
    }

    public bool SortDescending
    {
        get => _sortDescending;
        set
        {
            if (SetField(ref _sortDescending, value))
            {
                ApplySort();
            }
        }
    }

    public ICommand SortCommand { get; }
    public ICommand ToggleSortDirectionCommand { get; }

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

    private void ApplyFilters()
    {
        if (_images == null)
        {
            Console.WriteLine("_images is null in ApplyFilters");
            return;
        }

        Console.WriteLine($"Applying filters. Total images: {_images.Count}");
        Console.WriteLine($"Current filter: {SelectedFileType}");
        
        var query = SearchQuery.ToLower();
        var filtered = _images.Where(img =>
        {
            var nameMatch = string.IsNullOrEmpty(query) || img.Name.ToLower().Contains(query);
            var extensionMatch = SelectedFileType == "All" || 
                               (img.Extension != null && 
                                img.Extension.Equals(SelectedFileType, StringComparison.OrdinalIgnoreCase));
            
            Console.WriteLine($"File: {img.Name}, Extension: {img.Extension}, Filter: {SelectedFileType}, Match: {extensionMatch}");
            
            return nameMatch && extensionMatch;
        }).ToList();

        Console.WriteLine($"After filtering: {filtered.Count} images");
        ApplySortToList(filtered);
        Console.WriteLine($"Creating new collection with {filtered.Count} images");
        _filteredImages = new ObservableCollection<ImageFile>(filtered);
        OnPropertyChanged(nameof(Images));
        Console.WriteLine($"Images collection updated. Count: {_filteredImages.Count}");
    }

    private void ApplySort()
    {
        var list = Images.ToList();
        ApplySortToList(list);
        Images = new ObservableCollection<ImageFile>(list);
    }

    private void ApplySortToList(List<ImageFile> list)
    {
        Console.WriteLine($"Sorting {list.Count} images by {SortType}");
        IOrderedEnumerable<ImageFile> sorted = SortType switch
        {
            SortType.Name => list.OrderBy(x => x.Name),
            SortType.DateCreated => list.OrderBy(x => x.CreationTime),
            SortType.Size => list.OrderBy(x => x.Size),
            _ => list.OrderBy(x => x.Name)
        };

        var result = SortDescending ? sorted.Reverse().ToList() : sorted.ToList();
        list.Clear();
        list.AddRange(result);
        Console.WriteLine($"After sorting: {list.Count} images");
    }

    public async Task LoadImagesAsync(string[] paths)
    {
        try
        {
            Console.WriteLine($"Loading {paths.Length} images...");
            _images = await _imageLoader.LoadImagesAsync(paths);
            Console.WriteLine($"Loaded {_images.Count} images successfully");
            ApplyFilters();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки изображений: {ex.Message}");
            _images = new ObservableCollection<ImageFile>();
            ApplyFilters();
        }
    }

    public async Task LoadImagesFromDirectoryAsync(string directory)
    {
        try
        {
            Console.WriteLine($"Loading images from directory: {directory}");
            _images = await _imageLoader.LoadImagesFromDirectoryAsync(directory);
            Console.WriteLine($"Loaded {_images.Count} images from directory");
            ApplyFilters();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки директории: {ex.Message}");
            _images = new ObservableCollection<ImageFile>();
            ApplyFilters();
        }
    }
} 