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
            var viewer = new ImageViewer
            {
                DataContext = new ImageViewerViewModel(vm.Images, index)
            };
            await viewer.ShowDialog(this);
        }
    }

    private async void OnOpenFolderClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            try
            {
                var options = new FolderPickerOpenOptions
                {
                    Title = "Select folder with images",
                    AllowMultiple = false
                };

                Console.WriteLine("Opening folder picker...");
                var result = await StorageProvider.OpenFolderPickerAsync(options);
                
                if (result.Count > 0)
                {
                    Console.WriteLine($"Selected folder: {result[0].Path.LocalPath}");
                    await vm.LoadImagesFromDirectoryAsync(result[0].Path.LocalPath);
                }
                else
                {
                    Console.WriteLine("No folder selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnOpenFolderClick: {ex}");
            }
        }
    }
}
