using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Models;
using static System.Net.WebRequestMethods;

namespace TDMDEindopdracht.Infrastructure
{
    public class MapsApiCall
    {
        public async static Task<string> GetPolyLineList(Location location1, Location location2)
        {
            string apiKey = "AIzaSyBXG_XrA3JRTL58osjxd0DbqH563e2t84o";
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={location1.Latitude},{location1.Longitude}&destination={location2.Latitude},{location2.Longitude}&mode=walking&key={apiKey}";
            Debug.WriteLine(url);

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"URL response: {json}");
            JsonNode jsonnode = JsonNode.Parse(json);
            JsonObject jsonObject = jsonnode.AsObject();
            string routeString = jsonObject["routes"][0]?["overview_polyline"]?["points"].ToString();
            Debug.WriteLine(routeString);

            return (routeString);
        }
    }
}
