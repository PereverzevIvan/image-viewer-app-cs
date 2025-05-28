using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ImageViewerApp.Models;
using ImageViewerApp.Commands;

namespace ImageViewerApp.ViewModels;

public class FullscreenViewModel : ViewModelBase
{
    private readonly ObservableCollection<ImageFile> _images;
    private int _currentIndex;
    private ImageFile? _currentImage;
    private double _zoom = 1.0;
    private double _offsetX = 0;
    private double _offsetY = 0;
    private bool _isPanning;
    private double _lastPanX;
    private double _lastPanY;

    public event EventHandler? CloseRequested;

    public FullscreenViewModel(ObservableCollection<ImageFile> images, int startIndex)
    {
        _images = images;
        _currentIndex = startIndex;
        CurrentImage = images[startIndex];

        PreviousCommand = new RelayCommand(Previous, CanPrevious);
        NextCommand = new RelayCommand(Next, CanNext);
        CloseCommand = new RelayCommand(Close);
        ResetTransformCommand = new RelayCommand(ResetTransform);
    }

    public ImageFile? CurrentImage
    {
        get => _currentImage;
        private set => SetField(ref _currentImage, value);
    }

    public double Zoom
    {
        get => _zoom;
        set
        {
            if (value < 1.0) value = 1.0;
            if (value > 8.0) value = 8.0;
            SetField(ref _zoom, value);
        }
    }

    public double OffsetX
    {
        get => _offsetX;
        set => SetField(ref _offsetX, value);
    }

    public double OffsetY
    {
        get => _offsetY;
        set => SetField(ref _offsetY, value);
    }

    public ICommand PreviousCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand ResetTransformCommand { get; }

    public void StartPan(double x, double y)
    {
        _isPanning = true;
        _lastPanX = x;
        _lastPanY = y;
    }

    public void Pan(double x, double y)
    {
        if (_isPanning)
        {
            OffsetX += (x - _lastPanX) / Zoom;
            OffsetY += (y - _lastPanY) / Zoom;
            _lastPanX = x;
            _lastPanY = y;
        }
    }

    public void EndPan()
    {
        _isPanning = false;
    }

    public void ZoomImage(double delta, double centerX, double centerY)
    {
        var oldZoom = Zoom;
        Zoom += delta;
        // Центрируем относительно курсора
        OffsetX -= (centerX / oldZoom - centerX / Zoom);
        OffsetY -= (centerY / oldZoom - centerY / Zoom);
    }

    private void ResetTransform()
    {
        Zoom = 1.0;
        OffsetX = 0;
        OffsetY = 0;
    }

    private void Previous()
    {
        if (CanPrevious())
        {
            _currentIndex--;
            CurrentImage = _images[_currentIndex];
            ResetTransform();
        }
    }

    private bool CanPrevious() => _currentIndex > 0;

    private void Next()
    {
        if (CanNext())
        {
            _currentIndex++;
            CurrentImage = _images[_currentIndex];
            ResetTransform();
        }
    }

    private bool CanNext() => _currentIndex < _images.Count - 1;

    private void Close()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
} 