// ***********************************************************************
// File Name        : StationMapTest.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      £ºtest of station map
// ***********************************************************************
using NUnit.Framework;
using Trains.Domain;

namespace Tests
{
    /// <summary>
    /// test of station map
    /// </summary>
    public class StationMapTest
    {
        /// <summary>
        /// station map
        /// </summary>
        private readonly StationMap stationMap;

        /// <summary>
        /// construction
        /// </summary>
        public StationMapTest()
        {
            string testMapInfo = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";
            stationMap = StationMap.GetCurrentMap(testMapInfo);
        }

        /// <summary>
        /// set up
        /// </summary>
        [SetUp]
        public void Setup()
        {
            /* nothing to do */
        }

        /// <summary>
        /// Distance method test
        /// </summary>
        [Test]
        public void DistanceTest()
        {
            var testRoute = new string[] { "A", "B", "C" };
            var distance = stationMap.Distance(testRoute);
            Assert.True(distance == 9);

            testRoute = new string[] { "A", "D" };
            distance = stationMap.Distance(testRoute);
            Assert.True(distance == 5);

            testRoute = new string[] { "A", "D", "C" };
            distance = stationMap.Distance(testRoute);
            Assert.True(distance == 13);

            testRoute = new string[] { "A", "E", "B", "C", "D" };
            distance = stationMap.Distance(testRoute);
            Assert.True(distance == 22);

            testRoute = new string[] { "A", "E", "D" };
            var excetion = Assert.Throws<BusinessException>(() => { stationMap.Distance(testRoute); });
            Assert.That(excetion.Message, Is.EqualTo("NO SUCH ROUTE"));
        }

        /// <summary>
        /// NumberOfTripsWithStopLimit method test
        /// </summary>
        [Test]
        public void NumberOfTripsWithStopLimitTest()
        {
            var distance = stationMap.NumberOfTripsWithStopLimit("C", "C", 1, 3);
            Assert.True(distance == 2);

            distance = stationMap.NumberOfTripsWithStopLimit("A", "C", 4, 4);

            Assert.True(distance == 3);
        }

        /// <summary>
        /// LengthOfShortestRoute  method test
        /// </summary>
        [Test]
        public void LengthOfShortestRouteTest()
        {
            var distance = stationMap.LengthOfShortestRoute("A","C");
            Assert.True(distance == 9);

            distance = stationMap.LengthOfShortestRoute("B", "B");
            Assert.True(distance == 9);
        }

        /// <summary>
        /// NumberOfRouteWithDistanceLimit method test
        /// </summary>
        [Test]
        public void NumberOfRouteWithDistanceLimitTest()
        {
            var distance = stationMap.NumberOfRouteWithDistanceLimit("C", "C", 30);

            Assert.True(distance == 7);
        }
    }
}