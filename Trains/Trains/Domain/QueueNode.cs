// ***********************************************************************
// File Name        : QueueNode.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：a node used in Breadth-First Search, 
//                      Station filed store the station info, 
//                      the Level filed store it deep in all search,
//                      the TotalDistance store the total distance from root node
// 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Domain
{
    /// <summary>
    /// a node used in Breadth-First Search
    /// </summary>
    public class QueueNode
    {
        /// <summary>
        /// store the station info
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        /// it deep in all search
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// the total distance from root node
        /// </summary>
        public int TotalDistance { get; set; }

        /// <summary>
        /// empty constructor
        /// </summary>
        public QueueNode()
        {
            /* nothing to do */
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="station">next station</param>
        /// <param name="totalDistance">distance to next</param>
        public QueueNode(Station station,int level, int totalDistance)
            : base()
        {
            this.Station = station;
            this.Level = level;
            this.TotalDistance = totalDistance;
        }

        /// <summary>
        /// to string
        /// </summary>
        /// <returns>object string info</returns>
        public override string ToString()
        {
            return $"QueueNode:{{Station:{Station.Name}, Level:{Level}, TotalDistance:{TotalDistance}}}";
        }
    }
}
