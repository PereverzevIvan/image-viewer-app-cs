using Avalonia.Media.Imaging;

namespace ImageViewerApp.Models;

public class ImageFile
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Bitmap? Image { get; set; }
    public int GroupId { get; set; }
} 