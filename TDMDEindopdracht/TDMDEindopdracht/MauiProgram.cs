using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TDMDEindopdracht.Domain.Model;
using TDMDEindopdracht.Domain.Services;

namespace TDMDEindopdracht
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<ILocationPermissionService, LocationPermissionService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();
#if DEBUG

            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
