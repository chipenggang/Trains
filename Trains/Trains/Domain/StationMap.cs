// ***********************************************************************
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
            Func<QueueNode, int, Tuple<bool, int>> validateFunction = (qNode, cal) =>
            {
                var isBreak = false;
                var calResult = cal;
                if (calResult < 0)
                {
                    //针对累计校验值外部统一处理，因为不同场景校验值初始值不同
                    calResult = 0;
                }
                if (qNode.Level > maxDeep)
                {
                    //站台书超过限额，直接跳出
                    isBreak = true;
                }
                if (!isBreak && endStationName.Equals(qNode.Station.Name) && qNode.Level >= minDeep)
                {
                    //最小值交换
                    calResult++;
                }

                return new Tuple<bool, int>(isBreak, calResult);
            };

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
            Func<QueueNode, int, Tuple<bool, int>> validateFunction = (qNode, cal) =>
            {
                var isBreak = false;
                var calResult = cal;
                if (calResult < 0)
                {
                    //针对累计校验值外部统一处理，因为不同场景校验值初始值不同
                    calResult = int.MaxValue;
                }
                if (qNode.TotalDistance > calResult)
                {
                    //大于最小距离，直接跳出
                    isBreak = true;
                }
                if (!isBreak && endStationName.Equals(qNode.Station.Name))
                {
                    //最小值交换
                    calResult = calResult > qNode.TotalDistance ? qNode.TotalDistance : calResult;
                    isBreak = true;
                }

                return new Tuple<bool, int>(isBreak, calResult);
            };

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
            Func<QueueNode, int, Tuple<bool, int>> validateFunction = (qNode, cal) =>
            {
                var isBreak = false;
                var calResult = cal;
                if (calResult < 0)
                {
                    //针对累计校验值外部统一处理，因为不同场景校验值初始值不同
                    calResult = 0;
                }

                if (qNode.TotalDistance >= limitDistance)
                {
                    //大于最小距离，直接跳出
                    isBreak = true;
                }
                if (!isBreak && endStationName.Equals(qNode.Station.Name))
                {
                    //达到终点，但未大于限定距离，继续扫描
                    if (qNode.TotalDistance < limitDistance)
                    {
                        calResult++;
                    }
                }

                return new Tuple<bool, int>(isBreak, calResult);
            };

            return BFS(startStationName, endStationName, validateFunction);
        }

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
                var fromStation = new Station() { Name = fromName };
                var toStation = new Station() { Name = toName };
                if (!int.TryParse(lineInfo.Replace(fromName, "").Replace(toName, ""), out int distance))
                {
                    throw new BusinessException($"station distance of {fromName}-{toName} is illegal");
                }

                var thisStation = stationsMap.FirstOrDefault(state => state.Equals(fromStation));
                var nextStation = stationsMap.FirstOrDefault(state => state.Equals(toStation));

                //start station is not exist, then add
                if (thisStation == null)
                {
                    thisStation = fromStation;
                    stationsMap.Add(thisStation);
                }

                //to station is not exists, then add
                if (nextStation == null)
                {
                    nextStation = toStation;
                    stationsMap.Add(nextStation);
                }

                //initialize line between start station and next station
                if (thisStation.Lines == null)
                {
                    thisStation.Lines = new List<Line>();
                }
                if (nextStation.Lines == null)
                {
                    nextStation.Lines = new List<Line>();
                }

                //check station route is exists
                var existStation = thisStation.Lines.FirstOrDefault(node => node.Next.Equals(nextStation));
                if (existStation != null)
                {
                    string errInfo = $"route：{thisStation.Name}{nextStation.Name}{existStation.Distance} had exists ";
                    if (existStation.Distance == distance)
                    {
                        //repeated route will be ignore, only print log
                        Console.WriteLine(errInfo);
                    }
                    else
                    {
                        //conflicting route will stop running
                        throw new BusinessException($"{errInfo},且两次路径不一样{fromStation.Name}{toStation.Name}{distance}");
                    }
                }

                //bind station and create line
                thisStation.Lines.Add(new Line() { Next = nextStation, Distance = distance });
            }

            return stationsMap;
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
