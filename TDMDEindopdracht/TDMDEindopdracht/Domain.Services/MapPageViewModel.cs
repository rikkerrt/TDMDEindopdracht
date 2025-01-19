
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MapPageViewModel : ObservableObject
    {
        [ObservableProperty] private MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements= [];
        [ObservableProperty] private ObservableCollection<Pin> _pins = [];
        private readonly IGeolocation geolocation;
        public MapPageViewModel(IGeolocation location) 
        { 
            geolocation = location;
            ZoomToUserLocation();
            makeRoute();
        }

        public async void makeRoute()
        {
            List<Location> locations = await APIManager.GetPolyLineList(new Location(51.588311, 4.776298), new Location(51.595548, 4.779577));
            createRoute(locations);
            setPin("station");

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

        public void createRoute(List<Location> locations)
        {
            Polyline routeLine =  new Polyline
            {
                StrokeColor = Color.FromHex("1F51FF"),
                StrokeWidth = 10
            };

            foreach (Location location in locations) {
                Debug.WriteLine($"lat = {location.Latitude}, lng = {location.Longitude}");
                routeLine.Geopath.Add(location);
            }

            MapElements.Add(routeLine);
        }

        public async void setPin(string longname)
        {
            var userLocation = await Geolocation.GetLastKnownLocationAsync();
            var ListOfPins = await APIManager.ListOfStations(userLocation);
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
    }
}
