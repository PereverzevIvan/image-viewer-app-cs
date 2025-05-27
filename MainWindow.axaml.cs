using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void OnImagePointerEnter(object? sender, PointerEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.HoverZoomLevel = 1.5;
        }
    }

    private void OnImagePointerLeave(object? sender, PointerEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.HoverZoomLevel = 1.0;
        }
    }

    private void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement fullscreen view
    }
}
