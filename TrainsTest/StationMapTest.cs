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
        /// Distance method test
        /// </summary>
        [Test]
        public void ShouldCalcCorrectDuration()
        {
            var testRoute = new string[] { "A", "B", "C" };
            var distance = stationMap.Duration(testRoute);
            Assert.AreEqual(11, distance);

            testRoute = new string[] { "A", "D" };
            distance = stationMap.Duration(testRoute);
            Assert.True(distance == 5);

            testRoute = new string[] { "A", "D", "C" };
            distance = stationMap.Duration(testRoute);
            Assert.True(distance == 15);

            testRoute = new string[] { "A", "E", "B", "C", "D" };
            distance = stationMap.Duration(testRoute);
            Assert.True(distance == 28);

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

        /* The number of trips starting at C and ending at C with a maximum duration of 30 minutes. 
         * In the sample data below, there are four such trips:
         * C©\D©\C (18), C©\E©\ B©\C (13), C©\D©\E©\B©\C (27) and C©\E©\B©\C©\E©\B©\C (28)*/

       [Test]
        public void DurationOfFastestRoute()
        {
            var distance = stationMap.DurationOfFastestRoute("A", "C");
            Assert.AreEqual(11, distance);

            distance = stationMap.DurationOfFastestRoute("B", "B");
            Assert.AreEqual(13, distance);
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