
using CommunityToolkit.Mvvm.ComponentModel;
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
        private System.Timers.Timer _timerUpdate;
        private readonly IGeolocation geolocation;
        public MapPageViewModel(IGeolocation location) 
        { 
            geolocation = location;
            ZoomToUserLocation();
            setPin("station");
            createRoute();
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

        public void createRoute()
        {
            Polyline routeLine =  new Polyline
            {
                StrokeColor = Color.FromArgb("1F51FF"),
                StrokeWidth = 1
            };

            List<Location> locations = APIManager.DecodePolyLine("knjmEnjunUbKCfEA?_@]@kMBeE@qIIoF@wH@eFFk@WOUI_@?u@j@k@`@EXLTZHh@Y`AgApAaCrCUd@cDpDuAtAoApA{YlZiBdBaIhGkFrDeCtBuFxFmIdJmOjPaChDeBlDiAdD}ApGcDxU}@hEmAxD}[tt@yNb\\\\yBdEqFnJqB~DeFxMgK~VsMr[uKzVoCxEsEtG}BzCkHhKWh@]t@{AxEcClLkCjLi@`CwBfHaEzJuBdEyEhIaBnCiF|K_Oz\\\\{MdZwAbDaKbUiB|CgCnDkDbEiE|FqBlDsLdXqQra@kX|m@aF|KcHtLm@pAaE~JcTxh@w\\\\`v@gQv`@}F`MqK`PeGzIyGfJiG~GeLhLgIpIcE~FsDrHcFfLqDzH{CxEwAbBgC|B}F|DiQzKsbBdeA{k@~\\\\oc@bWoKjGaEzCoEzEwDxFsUh^wJfOySx[uBnCgCbCoFlDmDvAiCr@eRzDuNxC_EvAiFpCaC|AqGpEwHzFoQnQoTrTqBlCyDnGmCfEmDpDyGzGsIzHuZzYwBpBsC`CqBlAsBbAqCxAoBrAqDdDcNfMgHbHiPtReBtCkD|GqAhBwBzBsG~FoAhAaCbDeBvD_BlEyM``@uBvKiA~DmAlCkA|B}@lBcChHoJnXcB`GoAnIS~CIjFDd]A|QMlD{@jH[vAk@`CoGxRgPzf@aBbHoB~HeMx^eDtJ}BnG{DhJU`@mBzCoCjDaAx@mAnAgCnBmAp@uAj@{Cr@wBPkB@kBSsEW{GV}BEeCWyAWwHs@qH?cIHkDXuDn@mCt@mE`BsH|CyAp@}AdAaAtAy@lBg@pCa@jE]fEcBhRq@pJKlCk@hLFrB@lD_@xCeA`DoBxDaHvM_FzImDzFeCpDeC|CkExDiJrHcBtAkDpDwObVuCpFeCdHoIl\\\\uBjIuClJsEvMyDbMqAhEoDlJ{C|J}FlZuBfLyDlXwB~QkArG_AnDiAxC{G|OgEdLaE`LkBbEwG~KgHnLoEjGgDxCaC`BuJdFkFtCgCnBuClD_HdMqEzHcBpB_C|BuEzCmPlIuE|B_EtDeBhCgAdCw@rCi@|DSfECrCAdCS~Di@jDYhA_AlC{AxCcL`U{GvM_DjFkBzBsB`BqDhBaEfAsTvEmEr@iCr@qDrAiFnCcEzCaE~D_@JmFdGQDwBvCeErEoD|BcFjC}DbEuD~D`@Zr@h@?d@Wr@}@vAgCbEaHfMqA`Cy@dAg@bAO`@gCi@w@W\"");
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
                    if (distance < 300)
                    {
                        var request = new NotificationRequest
                        {
                            NotificationId = 1337,
                            Title = "Station dichtbij",
                            Description = "U bevindt zich momenteel binnen een radius van 300 meter van het station af.",
                            CategoryType = NotificationCategoryType.Alarm
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    } else
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
