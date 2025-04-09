using Microsoft.Maui.Devices.Sensors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TDMDEindopdracht.Domain.Services;

namespace MyApp.Tests
{
    [TestClass]
    public class TestPolylines
    {
        [TestMethod]
        public void TestPolylineWrong()
        {
            string encodedPolyline = "_p~iF~ps|U_ulLnnqC_mqNvxqT";

            List<Location> result = PolylineDecoder.DecodePolyLine(encodedPolyline);

            Assert.AreEqual(4, result.Count, "De gedecodeerde polyline moet 4 locaties bevatten.");

            Assert.AreEqual(38.5, result[0].Latitude, 0.0001, "De eerste locatie latitude is incorrect.");
            Assert.AreEqual(-120.2, result[0].Longitude, 0.0001, "De eerste locatie longitude is incorrect.");

            Assert.AreEqual(40.7, result[1].Latitude, 0.0001, "De tweede locatie latitude is incorrect.");
            Assert.AreEqual(-120.95, result[1].Longitude, 0.0001, "De tweede locatie longitude is incorrect.");
        }

        [TestMethod]
        public void TestPolylineRight()
        {
            string encodedPolyline = "_p~iF~ps|U_ulLnnqC_mqNvxqT";

            List<Location> result = PolylineDecoder.DecodePolyLine(encodedPolyline);

            Assert.AreEqual(3, result.Count, "De gedecodeerde polyline moet 3 locaties bevatten.");

            Assert.AreEqual(38.5, result[0].Latitude, 0.0001, "De eerste locatie latitude is incorrect.");
            Assert.AreEqual(-120.2, result[0].Longitude, 0.0001, "De eerste locatie longitude is incorrect.");

            Assert.AreEqual(40.7, result[1].Latitude, 0.0001, "De tweede locatie latitude is incorrect.");
            Assert.AreEqual(-120.95, result[1].Longitude, 0.0001, "De tweede locatie longitude is incorrect.");
        }
    }
}