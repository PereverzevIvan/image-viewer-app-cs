using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp.Views;

public partial class FullscreenWindow : Window
{
    private FullscreenViewModel ViewModel => (FullscreenViewModel)DataContext!;
    private bool _isPanning;

    public FullscreenWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is FullscreenViewModel vm)
        {
            vm.CloseRequested += (_, _) => Close();
        }
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        Focus();
    }

    private void OnImagePointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var point = e.GetPosition((Visual)sender!);
        var delta = e.Delta.Y * 0.2;
        ViewModel.ZoomImage(delta, point.X, point.Y);
    }

    private void OnImagePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint((Visual)sender!).Properties.IsLeftButtonPressed)
        {
            _isPanning = true;
            var point = e.GetPosition((Visual)sender!);
            ViewModel.StartPan(point.X, point.Y);
            e.Handled = true;
        }
    }

    private void OnImagePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_isPanning)
        {
            _isPanning = false;
            ViewModel.EndPan();
            e.Handled = true;
        }
    }

    private void OnImagePointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isPanning)
        {
            var point = e.GetPosition((Visual)sender!);
            ViewModel.Pan(point.X, point.Y);
            e.Handled = true;
        }
    }

    private void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
    {
        ViewModel.ResetTransform();
    }
} 