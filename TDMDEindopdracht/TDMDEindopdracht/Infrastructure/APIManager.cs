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

            string apiLink= $"https://gateway.apiportal.ns.nl/nsapp-stations/v3/nearest?lat={lat}&lng={lng}&limit=2&includeNonPlannableStations=false";
            Debug.WriteLine(apiLink);
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ns_key);
            var response = await client.GetAsync(apiLink);
            string json = await response.Content.ReadAsStringAsync();
            JsonDocument jsondoc = JsonDocument.Parse(json);
            Debug.WriteLine("json: " + json);
            return jsondoc;
        }

        public static List<Location> GetPolyLineList(Location location1, Location Location2)
        {
            
            
            return DecodePolyLine("");
        }


        public static List<Location> DecodePolyLine(string encodedPolyLine)
        {
            int index = 0;
            var polylineChars = encodedPolyLine.ToCharArray();
            var poly = new List<Location>();
            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);



                if (index >= polylineChars.Length)
                {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do
                { 
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                Location mLatLng = new Location(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }
            return poly;
        }
    }
}
