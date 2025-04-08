using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Services;
using TDMDEindopdracht.Infrastructure;

namespace TDMDEindopdracht.ApplicationLayer
{
    public class RouteService
    {
        private readonly MapsApiCall mapsApiCall;

        public RouteService(MapsApiCall mapsApiCall)
        {
            this.mapsApiCall = mapsApiCall;
        }

        public async Task<List<Location>> GetRoutesAsync(Location from, Location to)
        {
            var encodedPolyline = await mapsApiCall.GetPolyLineList(from, to);
            var route = PolylineDecoder.DecodePolyLine(encodedPolyline);
            return route;
        }
    }
}
