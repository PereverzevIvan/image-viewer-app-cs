using Avalonia;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace ImageViewerApp;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        var builder = BuildAvaloniaApp();
        
        if (args.Length > 0)
        {
            return ProcessCommandLineArgs(args, builder);
        }
        
        return builder.StartWithClassicDesktopLifetime(args);
    }

    private static int ProcessCommandLineArgs(string[] args, AppBuilder builder)
    {
        try
        {
            var app = builder.SetupWithoutStarting();
            var window = new MainWindow();
            var viewModel = window.DataContext as ViewModels.MainWindowViewModel;

            if (viewModel == null)
            {
                Console.WriteLine("Ошибка: не удалось инициализировать приложение");
                return 1;
            }

            if (args[0] == "-d" || args[0] == "--directory")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Ошибка: не указана директория");
                    return 1;
                }
                
                Task.Run(async () => await viewModel.LoadImagesFromDirectoryAsync(args[1])).Wait();
            }
            else
            {
                Task.Run(async () => await viewModel.LoadImagesAsync(args)).Wait();
            }

            var lifetime = new ClassicDesktopStyleApplicationLifetime
            {
                MainWindow = window,
                Args = args
            };

            app.Instance.ApplicationLifetime = lifetime;
            lifetime.Start(args);
            
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return 1;
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
