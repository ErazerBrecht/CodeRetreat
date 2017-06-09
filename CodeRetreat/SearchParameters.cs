using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRetreat
{
    public class SearchParameters
    {
        public Point StartLocation { get; set; }

        public Point EndLocation { get; set; }

        public Tile[,] Map { get; set; }

        public IEnumerable<Teleport> Teleports { get; set; }

        public SearchParameters(Point startLocation, Point endLocation, Tile[,] map, IEnumerable<Teleport> teleports)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;
            this.Map = map;
            Teleports = teleports;
        }
    }

}
