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
        private readonly INSApiCall _nsApiCall;

        [ObservableProperty] public string _nameOfStation;
        [ObservableProperty] public ObservableCollection<string> _stations;

        public MainPageViewModel(
            IDatabaseRepository databaseRepo,
            ILocationPermissionService locationPermissionService,
            IGeolocation geolocation,
            INSApiCall nsApiCall)
        {
            _databaseRepository = databaseRepo;
            _permissionServiceUsed = locationPermissionService;
            _nsApiCall = nsApiCall;
            _stations = new ObservableCollection<string>();

            LoadStations();
        }

        [RelayCommand]
        public async Task LoadInPage()
        {
            var currentStatus = await _permissionServiceUsed.CheckAndRequestPermissionForLocationAsync();

            if (currentStatus == PermissionStatus.Denied)
            {
                await _permissionServiceUsed.NavigateToSettingsWhenPermissionDenied();
                return;
            }

            Location location = await Geolocation.GetLocationAsync();
            ObservableCollection<StationNS> stations = await _nsApiCall.GetNearestStationsAsync(location, 3);

            foreach (var station in stations)
            {
                if (_stations.Contains(station.name))
                {
                    Debug.WriteLine("String already in stations list.");
                    continue;
                }

                Debug.WriteLine($"{station.name} - {station.latitude}, {station.longitude}");
                _stations.Add(station.name);
            }

            await _databaseRepository.Init();

            foreach (string name in _stations)
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
                //Stations.Add(stat); // Uncomment if station names should be reloaded
            }
        }
    }
}
