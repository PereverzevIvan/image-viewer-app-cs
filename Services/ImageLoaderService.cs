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
    public async Task<ObservableCollection<ImageFile>> LoadImagesAsync(string[] paths)
    {
        var images = new ObservableCollection<ImageFile>();
        
        foreach (var path in paths)
        {
            if (!File.Exists(path)) continue;
            
            try
            {
                await Task.Run(() =>
                {
                    var bitmap = new Bitmap(path);
                    images.Add(new ImageFile
                    {
                        Path = path,
                        Name = Path.GetFileName(path),
                        Image = bitmap,
                        GroupId = 0
                    });
                });
            }
            catch (Exception)
            {
                // Skip invalid images
            }
        }

        return images;
    }

    public async Task<ObservableCollection<ImageFile>> LoadImagesFromDirectoryAsync(string directory)
    {
        var files = Directory.GetFiles(directory, "*.*")
            .Where(file => file.ToLower().EndsWith("jpg") || 
                          file.ToLower().EndsWith("jpeg") || 
                          file.ToLower().EndsWith("png") || 
                          file.ToLower().EndsWith("gif"));
        
        return await LoadImagesAsync(files.ToArray());
    }
} 