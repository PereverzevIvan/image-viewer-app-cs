using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ImageViewerApp.Models;

namespace ImageViewerApp.Services;

public class ImageLoaderService
{
    private static readonly string[] SupportedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

    public async Task<ObservableCollection<ImageFile>> LoadImagesAsync(string[] paths)
    {
        var images = new ObservableCollection<ImageFile>();
        var filteredPaths = paths.Where(p => SupportedExtensions.Contains(Path.GetExtension(p).ToLower())).ToList();
        Console.WriteLine($"Found {filteredPaths.Count} supported image files");

        foreach (var path in filteredPaths)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                try
                {
                    Console.WriteLine($"Loading image: {path}");
                    var bitmap = await Task.Run(() => new Bitmap(path));
                    images.Add(new ImageFile
                    {
                        Path = path,
                        Name = Path.GetFileName(path),
                        Extension = Path.GetExtension(path).ToLower(),
                        Size = fileInfo.Length,
                        CreationTime = fileInfo.CreationTime,
                        LastWriteTime = fileInfo.LastWriteTime,
                        Image = bitmap
                    });
                    Console.WriteLine($"Successfully loaded: {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки изображения {path}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"File not found: {path}");
            }
        }
        return images;
    }

    public async Task<ObservableCollection<ImageFile>> LoadImagesFromDirectoryAsync(string directory)
    {
        Console.WriteLine($"Scanning directory: {directory}");
        var paths = await Task.Run(() => 
        {
            var files = Directory.GetFiles(directory)
                .Where(file => SupportedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToArray();
            Console.WriteLine($"Found {files.Length} files in directory");
            return files;
        });
        return await LoadImagesAsync(paths);
    }
} 