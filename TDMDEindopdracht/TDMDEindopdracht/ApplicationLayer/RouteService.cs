using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Services;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.ApplicationLayer
{
    public static class RouteService
    {
        public static async Task<List<Location>> GetRoutesAsync(Location from, Location to)
        {
            var encodedPolyline = await MapsApiCall.GetPolyLineList(from, to);
            Debug.WriteLine("POLYLINE RECEIVED");
            Debug.WriteLine(encodedPolyline);
            var route = PolylineDecoder.DecodePolyLine(encodedPolyline);
            return route;
        }
    }
}
