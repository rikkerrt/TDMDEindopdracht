using GoogleMapsApi.StaticMaps.Enums;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class BindableMap : Microsoft.Maui.Controls.Maps.Map
    {
        public static readonly BindableProperty MvvmMapElementsProperty =
            BindableProperty.Create(
                nameof(MvvmMapElements),
                typeof(ICollection<MapElement>),
                typeof(BindableMap),
                null,

                // Runt alleen op '= new()', niet op Add() of Clear()!!!
                propertyChanged: (b, _, n) =>
                {
                    if (b is BindableMap map)
                    {
                        map.MapElements.Clear();
                        foreach (var element in (IEnumerable<MapElement>)n)
                        {
                        }
                    }
                });

        public static readonly BindableProperty VisibleRegionInMapProperty =
          BindableProperty.Create(
              nameof(VisibleRegionInMap),
              typeof(MapSpan),
              typeof(BindableMap),
              default(MapSpan),
              propertyChanged: OnVisibleRegionChanged);
        public ICollection<MapElement> MvvmMapElements
        {
            get => (ICollection<MapElement>)GetValue(MvvmMapElementsProperty);
            set => SetValue(MvvmMapElementsProperty, value);
        }
        public MapSpan VisibleRegionInMap
        {
            get => (MapSpan)GetValue(VisibleRegionInMapProperty);
            set => SetValue(VisibleRegionInMapProperty, value);
        }
        private static void OnVisibleRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BindableMap map && newValue is MapSpan newRegion)
            {
                map.MoveToRegion(newRegion);
            }
        }
    }
}
