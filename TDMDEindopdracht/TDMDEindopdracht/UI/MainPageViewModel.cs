using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Interfaces;
using TDMDEindopdracht.Domain.Models;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly ILocationPermissionService _permissionServiceUsed;
        private readonly IDatabaseRepository _databaseRepository;

        [ObservableProperty] public string _nameOfStation;
        [ObservableProperty] public ObservableCollection<string> _stations;
        public MainPageViewModel(IDatabaseRepository databaseRepo,ILocationPermissionService locationPermissionService, IGeolocation geolocation)
        {
            _databaseRepository = databaseRepo;
            _permissionServiceUsed = locationPermissionService;
            Stations = new ObservableCollection<string>();
            LoadStations();

        }
        [RelayCommand]
        public async Task LoadInPage()
        {
            var currentStatus = await _permissionServiceUsed.CheckAndRequestPermissionForLocationAsync();

            if (currentStatus == PermissionStatus.Denied)
            {
                await _permissionServiceUsed.NavigateToSettingsWhenPermissionDenied();
            }

            Location location = await Geolocation.GetLocationAsync();
            ObservableCollection<StationNS> stations = await NSApiCall.GetNearestStationsAsync(location, 3);

            foreach (var station in stations)
            {
                Debug.WriteLine(station.name);
                Debug.WriteLine(station.latitude);
                Debug.WriteLine(station.longitude);
                Stations.Add(station.name);
            }
            await _databaseRepository.Init();

            foreach(string name in Stations)
            {
                Debug.WriteLine(name);
            }
        }
        public async Task LoadStations()
        {
            var allStations = await _databaseRepository.getVisitedStations();
            Debug.WriteLine("Stations: " + allStations.Count);
            Stations.Clear();
            foreach (var stat in allStations)
            {
                //Stations.Add(stat);
            }
        }
    }
}
