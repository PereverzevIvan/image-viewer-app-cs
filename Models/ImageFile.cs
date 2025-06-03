using System;
using Avalonia.Media.Imaging;

namespace ImageViewerApp.Models;

public class ImageFile
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public string Extension { get; set; } = "";
    public long Size { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastWriteTime { get; set; }
    public Bitmap? Image { get; set; }
    public int GroupId { get; set; }
} 