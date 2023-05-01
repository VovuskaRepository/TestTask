using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpressTestWork
{
    class Point : IGeometry
    {
        public double X, Y;

        public Point(double x = 0, double y = 0)
        {
            X = x;
            Y = y;

            IPoint = this;
        }

        public string GetData()
        {
            string txt = null;

            txt += $"POINT ({X} {Y})\r\n";
            txt += $"X = {X};\r\nY = {Y};";

            txt += "\r\n\r\n";

            return txt;
        }

        public Point IPoint { get; set; }
        public Polygon IPolygon { get; set; }
        public Linestring ILinestring { get; set; }
        public Geometrycollection IGeometrycollection { get; set; }
    }
}