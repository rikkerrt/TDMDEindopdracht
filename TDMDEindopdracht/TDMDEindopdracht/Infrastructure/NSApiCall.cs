using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Models;

namespace TDMDEindopdracht.Infrastructure
{
    internal class NSApiCall
    {
        public static DatabaseRepository DatabaseRepository { get; set; }
        public static readonly string ns_key = "7eeb2ea7fb0146a98a59bcf7dcf6fa86";
        public static bool save;
        public static async Task<StationNS> ListOfStations(Location geolocation)
        {
            double lat = geolocation.Latitude;
            double lng = geolocation.Longitude;
            Debug.WriteLine($"lat = {lat}");
            Debug.WriteLine($"Lng = {lng}");

            string apiLink = $"https://gateway.apiportal.ns.nl/nsapp-stations/v3/nearest?lat={lat}&lng={lng}&limit=2&includeNonPlannableStations=false";
            Debug.WriteLine(apiLink);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ns_key);
            var response = await client.GetAsync(apiLink);
            string json = await response.Content.ReadAsStringAsync();

            JsonNode node = JsonNode.Parse(json);
            JsonObject jsonObject = node.AsObject();

            string nameStation = jsonObject["payload"][0]?["names"]?["short"].ToString();
            double latitude = ((double)jsonObject["payload"][0]?["location"]?["lat"]);
            double longitude = ((double)jsonObject["payload"][0]?["location"]?["lng"]);

            return new StationNS
            {
                name = nameStation,
                latitude = latitude,
                longitude = longitude
            };

        }
    }
}
