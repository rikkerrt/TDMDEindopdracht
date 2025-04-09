using TDMDEindopdracht.Domain.Services;

namespace TestProjects
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Gegeven een gecodeerde polyline string (voorbeeld van een polyline)
            string encodedPolyline = "_p~iF~ps|U_ulLnnqC_mqNvxqT";

            // Wanneer we de DecodePolyLine-methode aanroepen
            List<Location> result = PolylineDecoder.DecodePolyLine(encodedPolyline);

            // Dan moeten we verwachten dat de polyline correct wordt gedeecodeerd
            // We controleren bijvoorbeeld het aantal resultaten
            Assert.AreEqual(4, result.Count, "De gedecodeerde polyline moet 4 locaties bevatten.");

            // Controleer specifieke locaties (gebruik hier de verwachte waarden)
            Assert.AreEqual(38.5, result[0].Latitude, 0.0001, "De eerste locatie latitude is incorrect.");
            Assert.AreEqual(-120.2, result[0].Longitude, 0.0001, "De eerste locatie longitude is incorrect.");

            Assert.AreEqual(40.7, result[1].Latitude, 0.0001, "De tweede locatie latitude is incorrect.");
            Assert.AreEqual(-120.95, result[1].Longitude, 0.0001, "De tweede locatie longitude is incorrect.");

            // Voeg eventueel meer assert statements toe voor extra validatie
        }
    }
}