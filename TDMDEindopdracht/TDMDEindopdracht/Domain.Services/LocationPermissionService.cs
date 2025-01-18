using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Model;

namespace TDMDEindopdracht.Domain.Services
{
    public class LocationPermissionService : ILocationPermissionService
    {
        
        public async Task<PermissionStatus> CheckAndRequestPermissionForLocationAsync()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
                return status;
            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.Android)
            {
                if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
                {
                    bool continueProcess = await Application.Current.MainPage.DisplayAlert(
                        "Deze app maakt gebruik van uw locatie.",
                        "Deze locatie is nodig voor het goed opereren van de app, deze moet u dus toestaan",
                        "Toestaan",
                        "Annuleren");
                    if (!continueProcess)
                        return PermissionStatus.Denied;
                }
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            return status;
        }

        public async Task<bool> NavigateToSettingsWhenPermissionDenied()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
            if (currentPage == null)
            {
                currentPage = Application.Current.MainPage;
            }
            var openSettings = await currentPage.DisplayAlert(
                "Deze app maakt gebruik van uw locatie.",
                "Deze locatie is nodig voor het goed opereren van de app, deze moet u dus toestaan",
                "Open Instellingen",
                "Annuleren"
                );
            if (openSettings)
            {
#if ANDROID
            var context = Android.App.Application.Context;
            var intent = new Android.Content.Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(Android.Content.ActivityFlags.NewTask);
            var uri = Android.Net.Uri.FromParts("package", context.PackageName, null);
            intent.SetData(uri);
            context.StartActivity(intent);
#endif
                return true;
            }
            return false;
        }
    }
}
