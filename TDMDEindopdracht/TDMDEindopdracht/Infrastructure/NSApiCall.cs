using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Models;
using TDMDEindopdracht.Domain.Services;

namespace TDMDEindopdracht.Infrastructure
{
    internal class NSApiCall
    {
        public static DatabaseRepository DatabaseRepository { get; set; }
        public static readonly string ns_key = "7eeb2ea7fb0146a98a59bcf7dcf6fa86";
        public static bool save;
        public static async Task<ObservableCollection<StationNS>> GetNearestStationsAsync(Location location, int limit)
        {
            string url = $"https://gateway.apiportal.ns.nl/nsapp-stations/v2/nearest?lat={location.Latitude}&lng={location.Longitude}&limit={limit}";
            Debug.WriteLine(url);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7eeb2ea7fb0146a98a59bcf7dcf6fa86");



            try
            {
                string response = await client.GetStringAsync(url);
                Debug.WriteLine(response);
                JObject json = JObject.Parse(response);

                ObservableCollection<StationNS> stations = new();

                foreach (var station in json["payload"])
                {
                    var stationInfo = new StationNS
                    {
                        name = station["namen"]?["lang"]?.ToString() ?? "Unknown",
                        latitude = station["lat"]?.ToObject<double>() ?? 0,
                        longitude = station["lng"]?.ToObject<double>() ?? 0
                    };
                    Debug.WriteLine(stationInfo.name);

                    stations.Add(stationInfo);
                }

                return stations;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching stations: " + ex.Message);
                return new ObservableCollection<StationNS>(); 
            }
        }
    }
}
