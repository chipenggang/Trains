﻿// ***********************************************************************
// File Name        : Line
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：Connect two station and store the distance
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Trains.Domain;

namespace Trains.Domain
{
    /// <summary>
    /// station map
    /// </summary>
    public class StationMap
    {
        /// <summary>
        /// a static map by Singleton
        /// </summary>
        private static StationMap stationMap = null;

        /// <summary>
        /// the lock used in synchronize
        /// </summary>
        private static readonly object lockObj = new object();

        /// <summary>
        /// regular pattern of route info
        /// </summary>
        private const string ROUTE_INFO_PATTERN = @"^([a-zA-Z]{2}[0-9]+(,)*(\s)*)+$";

        private const int TIME_OF_ONE_DISTANCE_MIN = 1;

        private const int STOP_MIN = 2;

        /// <summary>
        /// the construction
        /// </summary>
        /// <param name="mapInfo">map info of string</param>
        private StationMap(string mapInfo)
        {
            try
            {
                GetStationMap = BuildStationsMap(mapInfo);
            }
            catch (BusinessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// get map list
        /// </summary>
        public List<Station> GetStationMap { get; }

        /// <summary>
        /// get current map
        /// </summary>
        /// <param name="mapInfo">map info of string</param>
        /// <returns>map object</returns>
        public static StationMap GetCurrentMap(string mapInfo)
        {
            if (stationMap == null)
            {
                lock (lockObj)
                {
                    if (stationMap == null)
                    {
                        stationMap = new StationMap(mapInfo);
                    }
                }
            }

            return stationMap;
        }
        /*A train travelling along any given route will stop in a station for two minutes.
        Assuming one unit of distance travelled takes 1 minute, 
        the 10 questions in Part 1 have been added to to include duration.*/

        public int Duration(params string[] stations)
        {
            var distance = Distance(stations);
            var stationCount = stations.Length - 2;

            //A-B-C 

            return distance * TIME_OF_ONE_DISTANCE_MIN + stationCount * STOP_MIN;
        }

        /// <summary>
        /// get distance
        /// </summary>
        /// <param name="stations">list of station name </param>
        /// <returns>total distance</returns>
        public int Distance(params string[] stations)
        {
            if (stations == null && stations.Length < 1)
            {
                throw new BusinessException("ROUTE IS EMPTY");
            }

            var startStationName = stations[0].Trim();
            var currentStation = GetStationMap.FirstOrDefault(station => station.Name.Equals(startStationName));
            var distance = 0;
            for (var i = 1; i < stations.Length; i++)
            {
                var nextStationName = stations[i].Trim();
                currentStation = currentStation.DistanceToNextStation(nextStationName, out int thisLineDistance);
                distance += thisLineDistance;
            }

            return distance;
        }

        public int DurationOfFastestRoute(string startStationName, string endStationName)
        { 
            Tuple<bool, int> validateFunction(QueueNode qNode, int cal)
            {
                var calResult = GetCalResult(cal, int.MaxValue);
                var du = qNode.TotalDistance * TIME_OF_ONE_DISTANCE_MIN + Math.Max(qNode.Level - 1, 0) * STOP_MIN;
                var isBreak = IsBreak(du, calResult);

                if (!isBreak && endStationName.Equals(qNode.Station.Name))
                {
                    calResult = calResult > du ? du : calResult;
                    isBreak = true;
                }

                return new Tuple<bool, int>(isBreak, calResult);
            }

            return BFS(startStationName, endStationName, validateFunction);
            
        }

        /// <summary>
        /// number of trips
        /// </summary>
        /// <param name="startStationName">start station name</param>
        /// <param name="endStationName">end station name</param>
        /// <param name="minDeep">min station number</param>
        /// <param name="maxDeep">max station number</param>
        /// <returns></returns>
        public int NumberOfTripsWithStopLimit(string startStationName, string endStationName, int minDeep, int maxDeep)
        {
            Tuple<bool, int> validateFunction(QueueNode qNode, int cal)
            {
                var calResult = GetCalResult(cal, 0);
                var isBreak = IsBreak(qNode.Level, maxDeep);

                if (!isBreak && endStationName.Equals(qNode.Station.Name) && qNode.Level >= minDeep)
                {
                    calResult++;
                }

                return new Tuple<bool, int>(isBreak, calResult);
            }

            return BFS(startStationName, endStationName, validateFunction);
        }

        /// <summary>
        /// length of the shortest route
        /// </summary>
        /// <param name="startStationName">start station name</param>
        /// <param name="endStationName">end station name</param>
        /// <returns>length</returns>
        public int LengthOfShortestRoute(string startStationName, string endStationName)
        {
            Tuple<bool, int> validateFunction(QueueNode qNode, int cal)
            {
                var calResult = GetCalResult(cal, int.MaxValue);
                var isBreak = IsBreak(qNode.TotalDistance, calResult);

                if (!isBreak && endStationName.Equals(qNode.Station.Name))
                {
                    //最小值交换
                    calResult = calResult > qNode.TotalDistance ? qNode.TotalDistance : calResult;
                    isBreak = true;
                }

                return new Tuple<bool, int>(isBreak, calResult);
            }

            return BFS(startStationName, endStationName, validateFunction);
        }

        /// <summary>
        /// number of different route 
        /// </summary>
        /// <param name="startStationName">start station name</param>
        /// <param name="endStationName">end station name</param>
        /// <param name="limitDistance">limit distance</param>
        /// <returns></returns>
        public int NumberOfRouteWithDistanceLimit(string startStationName, string endStationName, int limitDistance)
        {
            Tuple<bool, int> validateFunction(QueueNode qNode, int cal)
            {
                var calResult = GetCalResult(cal, 0);
                var isBreak = IsBreak(qNode.TotalDistance, limitDistance);

                if (!isBreak && endStationName.Equals(qNode.Station.Name))
                {
                    //达到终点，但未大于限定距离，继续扫描
                    if (qNode.TotalDistance < limitDistance)
                    {
                        calResult++;
                    }
                }

                return new Tuple<bool, int>(isBreak, calResult);
            }

            return BFS(startStationName, endStationName, validateFunction);
        }

        /// <summary>
        /// get statistic value
        /// </summary>
        /// <param name="initCalAmount">init value </param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        private int GetCalResult(int initCalAmount, int defaultValue) => initCalAmount < 0 ? defaultValue : initCalAmount;

        /// <summary>
        /// is break 
        /// </summary>
        /// <param name="compareValue">compare value</param>
        /// <param name="limitValue">lmit value</param>
        /// <returns></returns>
        private bool IsBreak(int compareValue, int limitValue) => compareValue > limitValue;

        /// <summary>
        /// BFS
        /// </summary>
        /// <param name="startStationName">start station name</param>
        /// <param name="endStationName">end station name</param>
        /// <param name="fun">dynamic function</param>
        /// <returns>the calculate data</returns>
        private int BFS(string startStationName, string endStationName, Func<QueueNode, int, Tuple<bool, int>> validateFunction)
        {
            var startStation = GetStationMap.FirstOrDefault(s => s.Name.Equals(startStationName));
            if (startStation == null)
            {
                throw new BusinessException("NO SUCH ROUTE");
            }      

            Queue<QueueNode> stationQueue = new Queue<QueueNode>();
            stationQueue.Enqueue(new QueueNode() { Level = 0, TotalDistance = 0, Station = startStation });
            int times = -1;
            while (stationQueue.Count > 0)
            {
                var pop = stationQueue.Dequeue();
                var popStation = pop.Station;
                if (popStation.Lines != null && popStation.Lines.Any())
                {
                    foreach (var node in popStation.Lines)
                    {
                        QueueNode qNode = new QueueNode()
                        {
                            Station = node.Next,
                            TotalDistance = pop.TotalDistance + node.Distance,
                            Level = pop.Level + 1
                        };
                        

                        var result = validateFunction(qNode, times);

                        times = result.Item2;

                        if (result.Item1)
                        {
                            break;
                        }

                        stationQueue.Enqueue(qNode);
                    }
                }
            }

            return times;
        }

        /// <summary>
        /// build map
        /// </summary>
        /// <param name="routeInfo">route string info</param>
        /// <returns>map</returns>
        private List<Station> BuildStationsMap(string routeInfo)
        {
            var routeInfoTrim = routeInfo.Trim().ToUpper();

            ValidateMapInfo(routeInfoTrim);

            //map list
            List<Station> stationsMap = new List<Station>();

            //initialize station map
            foreach (var lineInfo in routeInfoTrim.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                var lineArray = lineInfo.Trim().ToCharArray();

                var fromName = lineArray[0].ToString();
                var toName = lineArray[1].ToString();

                if (!int.TryParse(lineInfo.Replace(fromName, "").Replace(toName, ""), out int distance))
                {
                    throw new BusinessException($"station distance of {fromName}-{toName} is illegal");
                }

                var fromStation = AddToMap(fromName, stationsMap);
                var toStation = AddToMap(toName, stationsMap);

                AddLine(fromStation, toStation, distance);
            }

            return stationsMap;
        }

        /// <summary>
        /// add station to map
        /// </summary>
        /// <param name="stationName">staion name</param>
        /// <param name="stationsMap">station map</param>
        /// <returns></returns>
        private Station AddToMap(string stationName, List<Station> stationsMap)
        {
            var thisStation = stationsMap.FirstOrDefault(state => state.Name.Equals(stationName));
            if (thisStation == null)
            {
                thisStation = new Station() { Name = stationName, Lines = new List<Line>() };
                stationsMap.Add(thisStation);
            }

            return thisStation;
        }

        /// <summary>
        /// add line
        /// </summary>
        /// <param name="fromStation">from station </param>
        /// <param name="toStation">to station </param>
        /// <param name="distance">distance</param>
        private void AddLine(Station fromStation, Station toStation, int distance)
        {
            //check station route is exists
            if (CheckLine(fromStation, toStation, distance))
            {
                //bind station and create line
                fromStation.Lines.Add(new Line() { Next = toStation, Distance = distance });
            }
        }

        /// <summary>
        /// check line 
        /// </summary>
        /// <param name="fromStation">from station </param>
        /// <param name="toStation">to station </param>
        /// <param name="distance">distance</param>
        /// <returns>if line exists and conflict then return false else true</returns>
        private bool CheckLine(Station fromStation, Station toStation, int distance)
        {
            var existStation = fromStation.Lines.FirstOrDefault(node => node.Next.Equals(toStation));
            if (existStation != null)
            {
                string errInfo = $"route：{fromStation.Name}{toStation.Name}{existStation.Distance} had exists ";
                if (existStation.Distance == distance)
                {
                    //repeated route will be ignore, only print log
                    Console.WriteLine(errInfo);
                }
                else
                {
                    //conflicting route will stop running
                    throw new BusinessException($"{errInfo},and the distance is not equals {fromStation.Name}{toStation.Name}{distance}");
                }
            }

            return true;
        }

        /// <summary>
        /// validate the source info
        /// </summary>
        /// <param name="routeInfo">route info</param>
        private void ValidateMapInfo(string routeInfo)
        {
            var routeInfoTrim = routeInfo.Trim();
            if (string.IsNullOrWhiteSpace(routeInfoTrim))
            {
                throw new BusinessException($"The route information entered is not valid");
            }


            if (!Regex.IsMatch(routeInfoTrim, ROUTE_INFO_PATTERN))
            {
                throw new BusinessException($"The route information entered is not valid");
            }
        }
    }
}
