﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpressTestWork
{
    class Linestring : IGeometry
    {
        public List<Point> point = new List<Point>();

        public Linestring(List<Point> points)
        {
            foreach (Point tempPoint in points)
            {
                point.Add(tempPoint);

                IPoint = tempPoint;
            }

            ILinestring = this;
        }

        public string GetData()
        {
            string txt = null;

            txt += "LINESTRING (";
            int indexPoint = -1;

            foreach (Point tempPoint in point)
            {
                indexPoint++;

                if (indexPoint == point.Count - 1) txt += $"{tempPoint.X} {tempPoint.Y}";
                else txt += $"{tempPoint.X} {tempPoint.Y}, ";
            }

            txt += ")\r\n\r\n";

            return txt;
        }

        public Point IPoint { get; set; }
        public Polygon IPolygon { get; set; }
        public Linestring ILinestring { get; set; }
        public Geometrycollection IGeometrycollection { get; set; }
    }
}