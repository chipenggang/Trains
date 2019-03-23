// ***********************************************************************
// File Name        : Program.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：main program
// ***********************************************************************
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Config;
using Trains.Domain;
using Trains.Utils;

namespace Trains
{
    /// <summary>
    /// main program
    /// </summary>
    class Program
    {
        /// <summary>
        /// main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("loading map data...");

            MapConfigInfo mapConfigInfo = new MapConfigInfo();

            string lineMapStr = mapConfigInfo.GetMapConfigInfo();

            StationMap stationMap = StationMap.GetCurrentMap(lineMapStr);
            if (stationMap == null || stationMap.GetStationMap == null)
            {
                Console.WriteLine("build map error");
                Console.WriteLine("press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            List<Station> stations = stationMap.GetStationMap;

            Console.WriteLine("1. The distance of the route A-B-C.");
            Console.WriteLine("2. The distance of the route A - D.");
            Console.WriteLine("3. The distance of the route A - D - C.");
            Console.WriteLine("4. The distance of the route A - E - B - C - D.");
            Console.WriteLine("5. The distance of the route A - E - D.");
            Console.WriteLine("6. The number of trips starting at C and ending at C with a maximum of 3 stops.In the sample data below, there are two such trips: C - D - C(2 stops).and C - E - B - C(3 stops).");
            Console.WriteLine("7. The number of trips starting at A and ending at C with exactly 4 stops.In the sample data below, there are three such trips: A to C(via B, C, D); A to C(via D, C, D); and A to C(via D, E, B).");
            Console.WriteLine("8. The length of the shortest route(in terms of distance to travel) from A to C.");
            Console.WriteLine("9. The length of the shortest route(in terms of distance to travel) from B to B.");
            Console.WriteLine("10.The number of different routes from C to C with a distance of less than 30.In the sample data, the trips are: CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC.");


            Console.WriteLine("");
            Console.WriteLine("please wait....");
            Console.WriteLine("");

            //calculate distance of the route
            Console.WriteLine($"Output #1: { BusinessException.TryRun(() => { return stationMap.Distance(new string[] { "A", "B", "C" }); })   }");
            Console.WriteLine($"Output #2: { BusinessException.TryRun(() => { return stationMap.Distance(new string[] { "A", "D" }); }) }");
            Console.WriteLine($"Output #3: { BusinessException.TryRun(() => { return stationMap.Distance(new string[] { "A", "D", "C" }); }) }");
            Console.WriteLine($"Output #4: { BusinessException.TryRun(() => { return stationMap.Distance(new string[] { "A", "E", "B", "C", "D" }); }) }");
            Console.WriteLine($"Output #5: { BusinessException.TryRun(() => { return stationMap.Distance(new string[] { "A", "E", "D" }); }) }");

            //calculate number of trips      
            Console.WriteLine($"Output #6: { BusinessException.TryRun(() => { return stationMap.NumberOfTripsWithStopLimit("C", "C", 1, 3); }) }");
            Console.WriteLine($"Output #7: { BusinessException.TryRun(() => { return stationMap.NumberOfTripsWithStopLimit("A", "C", 4, 4); }) }");

            ////calculate calculate          
            Console.WriteLine($"Output #8: { BusinessException.TryRun(() => { return stationMap.LengthOfShortestRoute("A", "C"); }) }");
            Console.WriteLine($"Output #9: { BusinessException.TryRun(() => { return stationMap.LengthOfShortestRoute("B", "B"); }) }");
            Console.WriteLine($"Output #10:{ BusinessException.TryRun(() => { return stationMap.NumberOfRouteWithDistanceLimit("C", "C", 30); }) }");
            Console.WriteLine("");

            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }
    }
}
