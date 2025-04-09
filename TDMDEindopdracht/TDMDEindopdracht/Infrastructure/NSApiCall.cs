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
    public interface INSApiCall
    {
        Task<ObservableCollection<StationNS>> GetNearestStationsAsync(Location location, int limit);
    }

    public class NSApiCall : INSApiCall
    {
        private readonly HttpClient _httpClient;
        private const string NsApiKey = "7eeb2ea7fb0146a98a59bcf7dcf6fa86";

        public NSApiCall(HttpClient httpClient)
        {
            _httpClient = httpClient;
            if (!_httpClient.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", NsApiKey);
            }
        }

        public async Task<ObservableCollection<StationNS>> GetNearestStationsAsync(Location location, int limit)
        {
            string url = $"https://gateway.apiportal.ns.nl/nsapp-stations/v2/nearest?lat={location.Latitude}&lng={location.Longitude}&limit={limit}";
            Debug.WriteLine(url);

            try
            {
                string response = await _httpClient.GetStringAsync(url);
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
