using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMDEindopdracht.Domain.Interfaces
{
    public interface ILocationPermissionService
    {
        Task<PermissionStatus> CheckAndRequestPermissionForLocationAsync();
        Task<bool> NavigateToSettingsWhenPermissionDenied();
    }
}
