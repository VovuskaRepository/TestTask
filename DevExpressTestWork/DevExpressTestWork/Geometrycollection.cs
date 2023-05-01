using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpressTestWork
{
    class Geometrycollection : IGeometry
    {
        List<Point> point = new List<Point>();
        List<Polygon> polygon = new List<Polygon>();
        List<Linestring> linestring = new List<Linestring>();

        public Geometrycollection(List<Point> points = null, List<Polygon> polygons = null, List<Linestring> linestrings = null)
        {
            if (points != null)
            {
                foreach (Point tempPoint in points)
                {
                    point.Add(tempPoint);

                    IPoint = tempPoint;
                }
            }

            if (polygons != null)
            {
                foreach (Polygon tempPolygon in polygons)
                {
                    Polygon poly = new Polygon(tempPolygon.point);
                    polygon.Add(poly);

                    IPolygon = tempPolygon;
                }

                //foreach (Polygon tempPolygon in polygons)
                //{
                //    foreach (Point tempPoint in tempPolygon.point)
                //    {
                //        tempPolygon.point.Add(tempPoint);

                //        IPoint = tempPoint;
                //    }

                //    polygon.Add(tempPolygon);

                //    IPolygon = tempPolygon;
                //}
            }

            if (linestrings != null)
            {
                foreach (Linestring tempLinestring in linestrings)
                {
                    Linestring line = new Linestring(tempLinestring.point);
                    linestring.Add(line);

                    ILinestring = tempLinestring;
                }
            }

            //if (linestrings != null)
            //{
            //    foreach (Linestring tempLinestring in linestrings)
            //    {
            //        linestring.Add(tempLinestring);

            //        ILinestring = tempLinestring;
            //    }
            //}

            IGeometrycollection = this;
        }

        public string GetData()
        {
            string txt = null;
            int indexPoint;

            txt += "GEOMETRYCOLLECTION (";

            foreach (Point tempPoint in point)
            {
                txt += $"POINT({tempPoint.X}, {tempPoint.Y}), ";
            }

            int indexPolygon = -1;
            foreach (Polygon tempPolygon in polygon)
            {
                indexPoint = -1;
                indexPolygon++;

                txt += "POLYGON ((";

                foreach (Point tempPoint in tempPolygon.point)
                {
                    indexPoint++;

                    if (indexPoint == tempPolygon.point.Count - 1) txt += $"{tempPoint.X} {tempPoint.Y}";
                    else txt += $"{tempPoint.X} {tempPoint.Y}, ";
                }

                txt += "))";

                if (indexPoint == polygon.Count - 1) txt += ", ";
                if (indexPolygon != polygon.Count - 1) txt += ", ";
            }

            int indexLinestring = -1;

            foreach (Linestring tempLinestring in linestring)
            {
                indexPoint = -1;
                indexLinestring++;

                if (indexLinestring == 0) txt += ", ";

                txt += "LINESTRING (";

                foreach (Point tempPoint in tempLinestring.point)
                {
                    indexPoint++;

                    if (indexPoint == tempLinestring.point.Count - 1) txt += $"{tempPoint.X} {tempPoint.Y}";
                    else txt += $"{tempPoint.X} {tempPoint.Y}, ";
                }

                txt += ")";

                if (indexPoint == linestring.Count - 1) txt += ", ";
                if (indexLinestring != linestring.Count - 1) txt += ", ";
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
