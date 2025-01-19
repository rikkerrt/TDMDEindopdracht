using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Model;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.Domain.Services
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly ILocationPermissionService _permissionServiceUsed;

        public MainPageViewModel(ILocationPermissionService locationPermissionService)
        {
            _permissionServiceUsed = locationPermissionService;
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
        }
    }
}
