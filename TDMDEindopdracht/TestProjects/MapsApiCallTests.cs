using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using TDMDEindopdracht.Infrastructure;
using System.Text.Json.Nodes;
using System.Net;
using Moq.Protected;
using System.Threading;
using Microsoft.Maui.Devices.Sensors;

namespace TDMDEindopdracht.Tests
{
    [TestClass]
    public class MapsApiCallTests
    {
        // Happy flow test
        [TestMethod]
        public async Task GetPolyLineList_Returns_Correct_Route_When_Api_Success()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"routes\": [{\"overview_polyline\": {\"points\": \"encoded_polyline_here\"}}]}")
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mapsApiCall = new MapsApiCallMock(httpClient);

            var location1 = new Location(52.379189, 4.900254);
            var location2 = new Location(52.379600, 4.901300);

            string result = await mapsApiCall.GetPolyLineList(location1, location2);

            Assert.AreEqual("encoded_polyline_here", result);
        }

        // Unhappy flow test
        [TestMethod]
        public async Task GetPolyLineList_Returns_Empty_When_Api_Fails()
        {
           
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("{\"error_message\": \"Invalid API request\"}")
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mapsApiCall = new MapsApiCallMock(httpClient);

            var location1 = new Location(52.379189, 4.900254);
            var location2 = new Location(52.379600, 4.901300);

            string result = await mapsApiCall.GetPolyLineList(location1, location2);

            Assert.AreEqual(string.Empty, result);
        }

    }

    public class MapsApiCallMock : MapsApiCall
    {
        private readonly HttpClient _httpClient;

        public MapsApiCallMock(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public new async Task<string> GetPolyLineList(Location location1, Location location2)
        {
            string apiKey = "AIzaSyBXG_XrA3JRTL58osjxd0DbqH563e2t84o";
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={location1.Latitude},{location1.Longitude}&destination={location2.Latitude},{location2.Longitude}&mode=walking&key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(json))
            {
                return string.Empty; 
            }

            JsonNode jsonnode = JsonNode.Parse(json);
            JsonObject jsonObject = jsonnode.AsObject();

            var routeString = jsonObject["routes"]?[0]?["overview_polyline"]?["points"]?.ToString();

            return routeString ?? string.Empty; 
        }
    }
}