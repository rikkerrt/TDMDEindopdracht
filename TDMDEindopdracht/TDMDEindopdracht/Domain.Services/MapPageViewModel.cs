using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MapPageViewModel : ObservableObject
    {
        [ObservableProperty] private MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements= [];
        [ObservableProperty] private ObservableCollection<Pin> _pins;
        private readonly IGeolocation geolocation;
        public MapPageViewModel(IGeolocation location) 
        { 
            geolocation = location;
            ZoomToBreda();
        }
        private void ZoomToBreda()
        {
            Location location = new Location(51.588331, 4.777802);
            MapSpan mapSpan = new MapSpan(location, 0.015, 0.015);
            CurrentMapSpan = mapSpan;
        }

        public void setPin(double lat, double lng, string longname)
        {
            Pin pin = new Pin
            {
                Label = longname,
                Location = new Location(lat, lng),
                Type = PinType.Place
            };

            Pins.Add(pin);
        }
    }
}
