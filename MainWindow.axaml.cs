using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ImageViewerApp.Models;
using ImageViewerApp.ViewModels;
using ImageViewerApp.Views;

namespace ImageViewerApp;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        LayoutUpdated += OnLayoutUpdated;
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.WindowWidth = Bounds.Width;
            ViewModel.WindowHeight = Bounds.Height;
        }
    }

    private void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is Border border && 
            border.DataContext is ImageFile image &&
            DataContext is MainWindowViewModel vm)
        {
            var index = vm.Images.IndexOf(image);
            var fullscreenViewModel = new FullscreenViewModel(vm.Images, index);
            var fullscreenWindow = new FullscreenWindow(fullscreenViewModel);
            fullscreenWindow.Show();
        }
    }
}
