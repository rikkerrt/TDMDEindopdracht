﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using TDMDEindopdracht.Domain.Model;
using TDMDEindopdracht.Domain.Services;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            string dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "stationData");
            builder.Services.AddSingleton<IDatabaseRepository>(s => new DatabaseRepository(dataBasePath));

            builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

            builder.Services.AddSingleton<MapPageViewModel>();
            builder.Services.AddSingleton<MapPage>(s => new MapPage(s.GetRequiredService<MapPageViewModel>()));

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
