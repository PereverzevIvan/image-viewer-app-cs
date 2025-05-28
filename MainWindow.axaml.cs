using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
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

    private async void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is Border border && 
            border.DataContext is ImageFile imageFile && 
            DataContext is MainWindowViewModel vm)
        {
            var index = vm.Images.IndexOf(imageFile);
            var fullscreenWindow = new FullscreenWindow
            {
                DataContext = new FullscreenViewModel(vm.Images, index)
            };
            await fullscreenWindow.ShowDialog(this);
        }
    }

    private async void OnOpenFolderClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var options = new FolderPickerOpenOptions
            {
                Title = "Select folder with images",
                AllowMultiple = false
            };

            var result = await StorageProvider.OpenFolderPickerAsync(options);
            if (result.Count > 0)
            {
                await vm.LoadImagesFromDirectoryAsync(result[0].Path.LocalPath);
            }
        }
    }
}
