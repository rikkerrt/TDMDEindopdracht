using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace TDMDEindopdracht.Infrastructure
{
    internal class APIManager
    {
        public static readonly string ns_key = "7eeb2ea7fb0146a98a59bcf7dcf6fa86";
        public static async Task<JsonDocument> ListOfStations(Location geolocation)
        {
            double lat = geolocation.Latitude;
            double lng = geolocation.Longitude;
            Debug.WriteLine($"lat = {lat}");
            Debug.WriteLine($"Lng = {lng}");

            //string apiLink= $"https://gateway.apiportal.ns.nl/nsapp-stations/v3/nearest?lat={lat}%lng={lng}&limit=2&includeNonPlannableStations=false";
            string apiLink= $"https://gateway.apiportal.ns.nl/nsapp-stations/v3/nearest?lat={lat}&lng={lng}&limit=2&includeNonPlannableStations=false";
            Debug.WriteLine(apiLink);
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ns_key);
            var response = await client.GetAsync(apiLink);
            string json = await response.Content.ReadAsStringAsync();
            JsonDocument jsondoc = JsonDocument.Parse(json);
            Debug.WriteLine(json);
            return jsondoc;
        }


        public static List<Location> DecodePolyLine(string encodedPolyLine)
        {


            return new List<Location>();
        }
    }
}
