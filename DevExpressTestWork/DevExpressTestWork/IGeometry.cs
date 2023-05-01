using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpressTestWork
{
    interface IGeometry
    {
        Point IPoint { get; set; }

        Polygon IPolygon { get; set; }

        Linestring ILinestring { get; set; }

        Geometrycollection IGeometrycollection { get; set; }

        string GetData();
    }
}