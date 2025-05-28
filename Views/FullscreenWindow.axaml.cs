using Avalonia.Controls;
using ImageViewerApp.ViewModels;

namespace ImageViewerApp.Views;

public partial class FullscreenWindow : Window
{
    public FullscreenWindow()
    {
        InitializeComponent();
    }

    public FullscreenWindow(FullscreenViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.CloseRequested += (s, e) => Close();
    }
} 