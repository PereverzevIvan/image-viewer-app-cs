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

    public event EventHandler? CloseRequested;

    public FullscreenViewModel(ObservableCollection<ImageFile> images, int startIndex)
    {
        _images = images;
        _currentIndex = startIndex;
        CurrentImage = images[startIndex];

        PreviousCommand = new RelayCommand(Previous, CanPrevious);
        NextCommand = new RelayCommand(Next, CanNext);
        CloseCommand = new RelayCommand(Close);
    }

    public ImageFile? CurrentImage
    {
        get => _currentImage;
        private set => SetField(ref _currentImage, value);
    }

    public ICommand PreviousCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand CloseCommand { get; }

    private void Previous()
    {
        if (CanPrevious())
        {
            _currentIndex--;
            CurrentImage = _images[_currentIndex];
        }
    }

    private bool CanPrevious() => _currentIndex > 0;

    private void Next()
    {
        if (CanNext())
        {
            _currentIndex++;
            CurrentImage = _images[_currentIndex];
        }
    }

    private bool CanNext() => _currentIndex < _images.Count - 1;

    private void Close()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
} 