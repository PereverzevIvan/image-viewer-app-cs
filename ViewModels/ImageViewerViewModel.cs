using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia;
using ImageViewerApp.Commands;
using ImageViewerApp.Models;

namespace ImageViewerApp.ViewModels;

public class ImageViewerViewModel : ViewModelBase
{
    private readonly IList<ImageFile>? _images;
    private int _currentIndex;
    private ImageFile? _currentImage;
    private bool _isPropertiesVisible;
    private double _scale = 1.0;
    private double _translateX;
    private double _translateY;
    private double _lastPanX;
    private double _lastPanY;

    private const double MinScale = 0.1;
    private const double MaxScale = 10.0;
    private const double ScaleStep = 0.2;

    public ImageViewerViewModel(IList<ImageFile> images, int initialIndex)
    {
        _images = images;
        _currentIndex = initialIndex;
        _currentImage = images[initialIndex];

        CloseCommand = new RelayCommand(() => RequestClose?.Invoke(this, EventArgs.Empty));
        NextCommand = new RelayCommand(Next, CanNext);
        PreviousCommand = new RelayCommand(Previous, CanPrevious);
        TogglePropertiesCommand = new RelayCommand(() => IsPropertiesVisible = !IsPropertiesVisible);
        ResetTransformCommand = new RelayCommand(ResetTransform);
    }

    public event EventHandler? RequestClose;

    public ImageFile? CurrentImage
    {
        get => _currentImage;
        private set => SetField(ref _currentImage, value);
    }

    public bool IsPropertiesVisible
    {
        get => _isPropertiesVisible;
        set => SetField(ref _isPropertiesVisible, value);
    }

    public double Scale
    {
        get => _scale;
        set
        {
            var newValue = Math.Clamp(value, MinScale, MaxScale);
            if (Math.Abs(_scale - newValue) > 0.001)
            {
                SetField(ref _scale, newValue);
            }
        }
    }

    public double TranslateX
    {
        get => _translateX;
        set => SetField(ref _translateX, value);
    }

    public double TranslateY
    {
        get => _translateY;
        set => SetField(ref _translateY, value);
    }

    public ICommand CloseCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand PreviousCommand { get; }
    public ICommand TogglePropertiesCommand { get; }
    public ICommand ResetTransformCommand { get; }

    private void Next()
    {
        if (_images != null && CanNext())
        {
            _currentIndex++;
            CurrentImage = _images[_currentIndex];
            ResetTransform();
        }
    }

    private bool CanNext() => _images != null && _currentIndex < _images.Count - 1;

    private void Previous()
    {
        if (_images != null && CanPrevious())
        {
            _currentIndex--;
            CurrentImage = _images[_currentIndex];
            ResetTransform();
        }
    }

    private bool CanPrevious() => _images != null && _currentIndex > 0;

    public void ZoomImage(double delta, double pointX, double pointY)
    {
        var oldScale = Scale;
        var newScale = oldScale + delta * ScaleStep;

        if (Math.Abs(newScale - oldScale) < 0.001) return;

        Scale = newScale;

        if (Math.Abs(Scale - oldScale) > 0.001)
        {
            var scaleChange = Scale / oldScale;
            TranslateX = pointX - (pointX - TranslateX) * scaleChange;
            TranslateY = pointY - (pointY - TranslateY) * scaleChange;
        }
    }

    public void StartPan(double x, double y)
    {
        _lastPanX = x;
        _lastPanY = y;
    }

    public void Pan(double x, double y)
    {
        var deltaX = x - _lastPanX;
        var deltaY = y - _lastPanY;

        if (Scale <= 1.0 && Scale >= 1.0)
        {
            _lastPanX = x;
            _lastPanY = y;
            return;
        }

        var factor = Scale < 1.0 ? 4.0 : Scale;
        
        TranslateX += deltaX / factor;
        TranslateY += deltaY / factor;

        _lastPanX = x;
        _lastPanY = y;
    }

    public void EndPan()
    {
        _lastPanX = 0;
        _lastPanY = 0;
    }

    public void ResetTransform()
    {
        Scale = 1.0;
        TranslateX = 0;
        TranslateY = 0;
    }
} 