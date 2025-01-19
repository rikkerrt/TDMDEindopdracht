
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
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MapPageViewModel : ObservableObject
    {
        [ObservableProperty] private MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements= [];
        [ObservableProperty] private ObservableCollection<Pin> _pins = [];
        public event Action CreateRoute;
        public IEnumerable<Location> Locations { get; set; }
        private System.Timers.Timer _timerUpdate;
        private readonly IGeolocation geolocation;
        private bool notificationShown = false;
        public MapPageViewModel(IGeolocation location) 
        { 
            geolocation = location;
            ZoomToUserLocation();
            makeRoute();
        }

        public async void makeRoute()
        {
            Locations = await APIManager.GetPolyLineList(new Location(51.588311, 4.776298), new Location(51.595548, 4.779577));
            setPin("station");
            CreateRoute();
            Task.Run(startUpdating);
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

        public async void setPin(string longname)
        {
            var userLocation = await Geolocation.GetLastKnownLocationAsync();
            var ListOfPins = await APIManager.ListOfStations(userLocation,false);
            Debug.WriteLine("API call nu ook in mapPageviewmodel");
            Pin pin = new Pin
            {
                Label = longname,
                Location = new Location(51.595554, 4.780000),
                Type = PinType.Generic
            };
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Pins.Add(pin);
            });
            Debug.WriteLine(Pins.Count);
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
            await APIManager.ListOfStations(pin.Location, true);
        }
    }
}
