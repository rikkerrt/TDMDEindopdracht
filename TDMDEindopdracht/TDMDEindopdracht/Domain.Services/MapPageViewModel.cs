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

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MapPageViewModel : ObservableObject
    {
        [ObservableProperty] private MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements= [];
        private readonly IGeolocation geolocation;
        public MapPageViewModel(IGeolocation location) 
        { 
            geolocation = location;
            ZoomToUserLocation();
        }
        private async void ZoomToUserLocation()
        {
            try
            {
                var userLocation = await Geolocation.GetLastKnownLocationAsync();
                if (userLocation != null)
                {
                    Location location = new Location(userLocation.Latitude, userLocation.Longitude);
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
    }

}
