// ***********************************************************************
// File Name        : Line
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：Connect two station and store the distance
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Trains.Domain
{
    /// <summary>
    /// the line of station
    /// </summary>
    public class Line
    {
        /// <summary>
        /// the next station info
        /// </summary>
        public Station Next { get; set; }

        /// <summary>
        /// the instance to next station
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// empty constructor
        /// </summary>
        public Line()
        {
            /* nothing to do */
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="next">next station</param>
        /// <param name="distance">distance to next</param>
        public Line(Station next, int distance)
            : base()
        {
            this.Next = next;
            this.Distance = distance;
        }

        /// <summary>
        /// to string
        /// </summary>
        /// <returns>object string info</returns>
        public override string ToString()
        {
            return $"LineInfo:{{NextStation:{Next.Name}, Distance:{Distance}}}";
        }
    }
}
