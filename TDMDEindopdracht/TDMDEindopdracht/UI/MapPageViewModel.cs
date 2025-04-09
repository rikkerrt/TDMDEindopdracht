
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TDMDEindopdracht.ApplicationLayer;
using TDMDEindopdracht.Domain.Interfaces;
using TDMDEindopdracht.Domain.Models;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MapPageViewModel : ObservableObject
    {
        [ObservableProperty] private MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements = [];
        [ObservableProperty] private ObservableCollection<Pin> _pins = [];

        private readonly IDatabaseRepository _databaseRepository;
        private readonly IGeolocation _geolocation;
        private readonly INSApiCall _nsApiCall;
        private bool _notificationShown = false;
        private System.Timers.Timer _timerUpdate;

        public event Action CreateRoute;

        public IEnumerable<Location> Locations { get; set; }

        public MapPageViewModel(
            IGeolocation geolocation,
            INSApiCall nsApiCall,
            IDatabaseRepository databaseRepository)
        {
            _geolocation = geolocation;
            _nsApiCall = nsApiCall;
            _databaseRepository = databaseRepository;

            ZoomToUserLocation();
            CreatePins();
        }

        public async Task MakeRoute(Location targetLocation)
        {
            Location currentLocation = await _geolocation.GetLocationAsync();

            if (currentLocation != null)
            {
                Locations = await RouteService.GetRoutesAsync(
                    new Location(currentLocation.Latitude, currentLocation.Longitude),
                    targetLocation);

                CreateRoute?.Invoke();
                Task.Run(StartUpdating);
            }
        }

        public async void CreatePins()
        {
            Location currentLocation = await _geolocation.GetLocationAsync();
            MapElements.Clear();
            Pins.Clear();

            ObservableCollection<StationNS> stations = await _nsApiCall.GetNearestStationsAsync(currentLocation, 3);

            foreach (var stationNS in stations)
            {
                var pin = new Pin
                {
                    Label = stationNS.name,
                    Location = new Location(stationNS.latitude, stationNS.longitude),
                    Type = PinType.Generic
                };

                Debug.WriteLine($"{stationNS.name} - {stationNS.latitude}, {stationNS.longitude}");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Pins.Add(pin);
                });
            }

            Debug.WriteLine($"Aantal pins: {Pins.Count}");
        }

        private async void ZoomToUserLocation()
        {
            try
            {
                var userLocation = await _geolocation.GetLastKnownLocationAsync();
                if (userLocation != null)
                {
                    var location = new Location(userLocation.Latitude, userLocation.Longitude);
                    var mapSpan = new MapSpan(location, 0.015, 0.015);
                    CurrentMapSpan = mapSpan;
                }
                else
                {
                    Debug.Write("De huidige locatie kon niet worden opgehaald");
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        public void StartUpdating()
        {
            _timerUpdate = new System.Timers.Timer(2000);
            _timerUpdate.Elapsed += OnTimedEvent;
            _timerUpdate.AutoReset = true;
            _timerUpdate.Start();
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            Task.Run(OnTimeEventAsync);
        }

        private async Task OnTimeEventAsync()
        {
            try
            {
                var location = await _geolocation.GetLocationAsync();
                if (location is null)
                    return;

                foreach (var pin in Pins)
                {
                    var distance = location.CalculateDistance(pin.Location, DistanceUnits.Kilometers) * 1000;

                    Debug.WriteLine($"Afstand tot {pin.Label}: {distance}m");

                    if (distance < 300 && !_notificationShown)
                    {
                        _notificationShown = true;

                        var request = new NotificationRequest
                        {
                            NotificationId = 1337,
                            Title = "Station dichtbij",
                            Description = "U bevindt zich momenteel binnen een radius van 300 meter van het station af.",
                            CategoryType = NotificationCategoryType.Alarm
                        };

                        await LocalNotificationCenter.Current.Show(request);
                    }
                    else if (distance > 300)
                    {
                        _notificationShown = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [RelayCommand]
        public async Task MarkerClicked(Pin pin)
        {
            await MakeRoute(pin.Location);
        }
    }
}
