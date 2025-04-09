using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDMDEindopdracht.Domain.Models;

namespace TestProjects
{
    [TestClass]
    public class StationNSTests
    {
        [TestMethod]
        public void TestStationNSProperties()
        {
            // Arrange
            var station = new StationNS
            {
                name = "Breda",
                index = 1,
                latitude = 52.379189,
                longitude = 4.90093
            };

            // Act & Assert
            Assert.AreEqual("Breda", station.name, "Naam van het station is incorrect.");
            Assert.AreEqual(1, station.index, "Index van het station is incorrect.");
            Assert.AreEqual(52.379189, station.latitude, 0.0001, "Latitude van het station is incorrect.");
            Assert.AreEqual(4.90093, station.longitude, 0.0001, "Longitude van het station is incorrect.");
        }
    }
}