// ***********************************************************************
// File Name        : Station.cs
// Author           : chipenggang
// Create Date      : 2019-03-16
//
// Description      ：To describe the station info and bind it's all line info to next station 
// 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace Trains.Domain
{
    /// <summary>
    /// station info and all line info from this station
    /// </summary>
    public class Station
    {
        /// <summary>
        /// station name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// next station list
        /// </summary>
        public List<Line> Lines { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public Station()
        {
            /* nothing to do */
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">station name</param>
        /// <param name="lines">next station list</param>
        public Station(string name, List<Line> lines)
            : base()
        {
            this.Name = name;
            this.Lines = lines;
        }

        /// <summary>
        /// get the distance to next station
        /// </summary>
        /// <param name="nextStationName">next station name</param>
        /// <param name="distance">out distance</param>
        /// <returns>next station object</returns>
        public Station DistanceToNextStation(string nextStationName,out int distance)
        {
            Line line = null;
            distance = 0;
            if (this.Lines == null
                || (line = this.Lines.FirstOrDefault(route => route.Next != null && route.Next.Name.Equals(nextStationName))) == null)
            {
                throw new BusinessException("NO SUCH ROUTE");
            }

            distance = line.Distance;

            return line.Next;
        }

        /// <summary>
        /// compare two station is equal
        /// </summary>
        /// <param name="obj">to compare object</param>
        /// <returns>if two station's name is equals then return true else false</returns>
        public override bool Equals(object obj)
        {
            return obj is Station && ((Station)obj).Name == this.Name;
        }

        /// <summary>
        /// get hashCode
        /// </summary>
        /// <returns>return the hashCode of station's name</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// to string
        /// </summary>
        /// <returns>object string info</returns>
        public override string ToString()
        {
            return $"Station:{{Name:{Name}, Lines:{string.Join(",", Lines.Where(l => l.Next != null).Select(l => $"{Name + l.Next.Name + l.Distance}"))}}}";
        }
    }
}
