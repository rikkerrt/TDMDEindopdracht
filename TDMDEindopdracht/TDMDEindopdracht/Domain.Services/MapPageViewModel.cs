
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
using TDMDEindopdracht.Domain.Model;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MapPageViewModel : ObservableObject
    {
        [ObservableProperty] private MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements= [];
        [ObservableProperty] private ObservableCollection<Pin> _pins = [];


        private IDatabaseRepository _databaseRepository;

        public event Action CreateRoute;
        public IEnumerable<Location> Locations { get; set; }
        private System.Timers.Timer _timerUpdate;
        private readonly IGeolocation geolocation;
        private bool notificationShown = false;
        public MapPageViewModel(IGeolocation location) 
        { 
            geolocation = location;
            ZoomToUserLocation();
            CreatePins();
        }

        public async Task makeRoute(Location targetLocation, IDatabaseRepository databaseRepository)
        {
            Location currentLocation = await geolocation.GetLocationAsync();
            _databaseRepository = databaseRepository;

            if (currentLocation != null)
            {
                Locations = await APIManager.GetPolyLineList(new Location(currentLocation.Latitude, currentLocation.Longitude), targetLocation);
                CreateRoute();
                Task.Run(startUpdating);
            }
        }
        public async void CreatePins()
        {
            Location location = await geolocation.GetLocationAsync();
            MapElements.Clear();
            StationNS stationNS = await APIManager.ListOfStations(location);
            Pin pin = new Pin
            {
                Label = stationNS.name,
                Location = new Location(stationNS.latitude, stationNS.longitude),
                Type = PinType.Generic
            };
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Pins.Add(pin);
            });
            Debug.WriteLine(Pins.Count);
        }
        private async void ZoomToUserLocation()
        {
            try
            {
                var userLocation = await Geolocation.GetLastKnownLocationAsync();
                if (userLocation != null)
                {
                    Location location = new Location(51.588331, 4.777802);
                    MapSpan mapSpan = new MapSpan(location, 0.015, 0.015);
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
        public void startUpdating()
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
                var location = await geolocation.GetLocationAsync();
                
                if (location is null)
                {
                    return;
                }
                foreach (var pin in Pins)
                {
                    var distance = location.CalculateDistance(pin.Location, DistanceUnits.Kilometers) * 1000;
                    Debug.WriteLine(distance.ToString());
                    if (distance < 300 && !notificationShown)
                    {
                        notificationShown = true;
                        var request = new NotificationRequest
                        {
                            NotificationId = 1337,
                            Title = "Station dichtbij",
                            Description = "U bevindt zich momenteel binnen een radius van 300 meter van het station af.",
                            CategoryType = NotificationCategoryType.Alarm
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    } 
                    if(distance > 300)
                    {
                        notificationShown = false;
                        return;
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
            await makeRoute(pin.Location, _databaseRepository);
        }
    }
}
