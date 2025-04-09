using TDMDEindopdrcaht.Domain.Services

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange: Een bekende geëncodeerde polyline.
            var encodedPolyline = "q~v~Ff}xuMbl@c@bG";

            // Act: Decodeer de polyline met de DecodePolyLine methode.
            var result = PolylineDecoder.DecodePolyLine(encodedPolyline);

            // Assert: Controleer of het resultaat correct is.
            var expectedCoordinates = new List<Location>
            {
                new Location(38.5, -120.2),
                new Location(38.6, -120.3)
            };

            Assert.Equal(expectedCoordinates.Count, result.Count);
            for (int i = 0; i < expectedCoordinates.Count; i++)
            {
                Assert.Equal(expectedCoordinates[i].Latitude, result[i].Latitude, 6);  // Vergelijk tot 6 decimalen
                Assert.Equal(expectedCoordinates[i].Longitude, result[i].Longitude, 6); // Vergelijk tot 6 decimalen
            }
        }
    }
}