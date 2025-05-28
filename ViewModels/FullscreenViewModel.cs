using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ImageViewerApp.Models;
using ImageViewerApp.Commands;
using Avalonia.Media;

namespace ImageViewerApp.ViewModels;

public class FullscreenViewModel : ViewModelBase
{
    private readonly IList<ImageFile> _images;
    private int _currentIndex;
    private ImageFile? _currentImage;
    private double _zoom = 1.0;
    private double _offsetX = 0;
    private double _offsetY = 0;
    private double _lastPanX;
    private double _lastPanY;
    private double _viewportWidth;
    private double _viewportHeight;

    private const double MinZoom = 0.667; // Позволяет уменьшить до 2/3 от оригинального размера
    private const double MaxZoom = 8.0;

    public event EventHandler? CloseRequested;

    public FullscreenViewModel(IList<ImageFile> images, int initialIndex)
    {
        _images = images;
        _currentIndex = initialIndex;
        CurrentImage = images[initialIndex];

        PreviousCommand = new RelayCommand(Previous, CanPrevious);
        NextCommand = new RelayCommand(Next, CanNext);
        CloseCommand = new RelayCommand(() => CloseRequested?.Invoke(this, EventArgs.Empty));
        ResetTransformCommand = new RelayCommand(ResetTransform);

        // Команды для перемещения с клавиатуры
        MoveLeftCommand = new RelayCommand(() => MoveByKeyboard(-1, 0));
        MoveRightCommand = new RelayCommand(() => MoveByKeyboard(1, 0));
        MoveUpCommand = new RelayCommand(() => MoveByKeyboard(0, -1));
        MoveDownCommand = new RelayCommand(() => MoveByKeyboard(0, 1));
    }

    public ImageFile? CurrentImage
    {
        get => _currentImage;
        private set => SetField(ref _currentImage, value);
    }

    public string CurrentImageInfo => $"{_currentIndex + 1} / {_images.Count}";

    public double Zoom
    {
        get => _zoom;
        set
        {
            if (value < MinZoom) value = MinZoom;
            if (value > MaxZoom) value = MaxZoom;
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

    public double ViewportWidth
    {
        get => _viewportWidth;
        set => SetField(ref _viewportWidth, value);
    }

    public double ViewportHeight
    {
        get => _viewportHeight;
        set => SetField(ref _viewportHeight, value);
    }

    public ICommand PreviousCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand ResetTransformCommand { get; }
    public ICommand MoveLeftCommand { get; }
    public ICommand MoveRightCommand { get; }
    public ICommand MoveUpCommand { get; }
    public ICommand MoveDownCommand { get; }

    public void StartPan(double x, double y)
    {
        _lastPanX = x;
        _lastPanY = y;
    }

    public void Pan(double x, double y)
    {
        var deltaX = x - _lastPanX;
        var deltaY = y - _lastPanY;

        // Если масштаб равен 1 или меньше, движение возможно только если изображение уменьшено
        if (Zoom <= 1.0 && Zoom >= 1.0)
        {
            _lastPanX = x;
            _lastPanY = y;
            return;
        }

        // При отдалении делаем движение медленнее
        var factor = Zoom < 1.0 ? 4.0 : Zoom;
        
        OffsetX += deltaX / factor;
        OffsetY += deltaY / factor;

        _lastPanX = x;
        _lastPanY = y;
    }

    public void EndPan()
    {
        // Ничего делать не нужно
    }

    public void ZoomImage(double delta, double centerX, double centerY)
    {
        var oldZoom = Zoom;
        Zoom += delta;

        // Корректируем смещение относительно центра масштабирования
        var zoomFactor = Zoom / oldZoom;
        OffsetX = centerX - (centerX - OffsetX) * zoomFactor;
        OffsetY = centerY - (centerY - OffsetY) * zoomFactor;
    }

    public void ResetTransform()
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
            OnPropertyChanged(nameof(CurrentImageInfo));
            ((RelayCommand)NextCommand).RaiseCanExecuteChanged();
            ((RelayCommand)PreviousCommand).RaiseCanExecuteChanged();
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
            OnPropertyChanged(nameof(CurrentImageInfo));
            ((RelayCommand)NextCommand).RaiseCanExecuteChanged();
            ((RelayCommand)PreviousCommand).RaiseCanExecuteChanged();
        }
    }

    private bool CanNext() => _currentIndex < _images.Count - 1;

    public void MoveByKeyboard(double deltaX, double deltaY)
    {
        // Если масштаб равен 1, движение не нужно
        if (Zoom <= 1.0 && Zoom >= 1.0)
        {
            return;
        }

        // При отдалении делаем движение медленнее
        var factor = Zoom < 1.0 ? 4.0 : Zoom;
        
        // Базовый шаг перемещения
        const double baseStep = 20.0;
        
        OffsetX += (deltaX * baseStep) / factor;
        OffsetY += (deltaY * baseStep) / factor;
    }
} 