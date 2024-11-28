using MauiAppEpubReader.Models;
using MauiAppEpubReader.Services;
using Microsoft.Extensions.Logging;

namespace MauiAppEpubReader
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<HtmlContentService>();
            // Register view models
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<EditViewModel>();

            var app = builder.Build();

            

            return app;
        }

        public static void CloseApplication()
        {
            //IsFileOpen = false;
            Application.Current?.Quit();
        }
    }
}
