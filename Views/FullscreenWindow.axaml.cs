using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp.Views;

public partial class FullscreenWindow : Window
{
    private bool _isPanning = false;

    public FullscreenWindow()
    {
        InitializeComponent();
    }

    public FullscreenWindow(FullscreenViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.CloseRequested += (s, e) => Close();
    }

    private void OnImagePointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (DataContext is FullscreenViewModel vm)
        {
            var point = e.GetPosition(this);
            vm.ZoomImage(e.Delta.Y > 0 ? 0.2 : -0.2, point.X, point.Y);
        }
    }

    private void OnImagePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && DataContext is FullscreenViewModel vm)
        {
            var point = e.GetPosition(this);
            vm.StartPan(point.X, point.Y);
            _isPanning = true;
            this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand);
        }
    }

    private void OnImagePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (DataContext is FullscreenViewModel vm)
        {
            vm.EndPan();
            _isPanning = false;
            this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Arrow);
        }
    }

    private void OnImagePointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isPanning && DataContext is FullscreenViewModel vm)
        {
            var point = e.GetPosition(this);
            vm.Pan(point.X, point.Y);
        }
    }

    private void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
    {
        if (DataContext is FullscreenViewModel vm)
        {
            vm.ResetTransformCommand.Execute(null);
        }
    }
} 