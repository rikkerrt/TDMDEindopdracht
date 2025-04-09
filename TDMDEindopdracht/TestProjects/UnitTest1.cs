using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDMDEindopdracht.Domain.Models; // Namespace waar StationNS staat

namespace MyApp.Tests
{
    [TestClass]
    public class StationTests
    {
        [TestMethod]
        public void StationNS_Should_Have_Name_And_Coordinates()
        {
            var station = new StationNS
            {
                name     = "Breda",
                latitude = 51.59,
                longitude = 4.78
            };

            Assert.IsNotNull(station.name);
            Assert.IsTrue(station.latitude > 0);
            Assert.IsTrue(station.longitude > 0);
        }
    }
}