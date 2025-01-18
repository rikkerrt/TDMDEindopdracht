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
    }

}
