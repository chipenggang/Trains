using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Service
{
    public interface IStationRouteService
    {
        /// <summary>
        /// get distance
        /// </summary>
        /// <param name="stations">list of station name </param>
        /// <returns>total distance</returns>
        int Distance(params string[] stations);

        /// <summary>
        /// number of trips
        /// </summary>
        /// <param name="startStationName">start station name</param>
        /// <param name="endStationName">end station name</param>
        /// <param name="minDeep">min station number</param>
        /// <param name="maxDeep">max station number</param>
        /// <returns></returns>
        int NumberOfTripsWithStopLimit2(string startStationName, string endStationName, int minDeep, int maxDeep);
    }
}
