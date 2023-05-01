using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DevExpressTestWork
{
    public partial class FormMain : DevExpress.XtraEditors.XtraForm
    {
        List<Point> points = new List<Point>();
        List<Polygon> polygons = new List<Polygon>();
        List<Linestring> linestrings = new List<Linestring>();
        List<Geometrycollection> geometrycollections = new List<Geometrycollection>();

        List<IGeometry> geometries = new List<IGeometry>();

        public FormMain()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            InitializeComponent();

            btnOpenFile.Click += OpenFile;
            btnSaveFile.Click += SaveFile;

            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void LinePolygon(string tempLine)
        {
            List<Point> pointsPolygon = new List<Point>();

            string[] coords = tempLine.Substring(10).Split(' '); // Убрать 10 символов ДО через пробел

            double x;
            double y;
            int tempDataIndex = 0;

            foreach (string tempData in coords)
            {
                if (tempData.EndsWith(","))
                {
                    x = double.Parse(coords[tempDataIndex - 1]);
                    y = double.Parse(coords[tempDataIndex].Substring(0, coords[tempDataIndex].Length - 1));

                    Point tempPoint = new Point(x, y);
                    pointsPolygon.Add(tempPoint);
                }
                else if (tempData.EndsWith("))"))
                {
                    x = double.Parse(coords[tempDataIndex - 1]);
                    y = double.Parse(coords[tempDataIndex].Substring(0, coords[tempDataIndex].Length - 2));

                    Point tempPoint = new Point(x, y);
                    pointsPolygon.Add(tempPoint);
                }

                tempDataIndex++;
            }

            Polygon tempPolygon = new Polygon(pointsPolygon);
            polygons.Add(tempPolygon);
        }

        public void OpenFile(object sender, EventArgs e)
        {
            string fileName;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("POINT"))
                        {
                            string[] coords = line.Substring(7 ).Split(' '); // убираем "POINT (" и разделяем координаты пробелом
                            double x = double.Parse(coords[0]);
                            double y = double.Parse(coords[1].Substring(0, coords[1].Length - 1)); // убираем ")" в конце второй координаты

                            Point tempPoint = new Point(x, y);
                            points.Add(tempPoint);
                        }

                        if (line.StartsWith("POLYGON"))
                        {
                            LinePolygon(line);
                        }

                        if (line.StartsWith("LINESTRING"))
                        {
                            List<Point> pointsLinestring = new List<Point>();

                            string[] coords = line.Substring(12).Split(' ');

                            double x;
                            double y;
                            int tempDataIndex = 0;

                            foreach (string tempData in coords)
                            {
                                if (tempData.EndsWith(","))
                                {
                                    x = double.Parse(coords[tempDataIndex - 1]);
                                    y = double.Parse(coords[tempDataIndex].Substring(0, coords[tempDataIndex].Length - 1));

                                    Point tempPoint = new Point(x, y);
                                    pointsLinestring.Add(tempPoint);
                                }
                                else if (tempData.EndsWith(")"))
                                {
                                    x = double.Parse(coords[tempDataIndex - 1]);
                                    y = double.Parse(coords[tempDataIndex].Substring(0, coords[tempDataIndex].Length - 1));

                                    Point tempPoint = new Point(x, y);
                                    pointsLinestring.Add(tempPoint);
                                }

                                tempDataIndex++;
                            }

                            Linestring tempLinestring = new Linestring(pointsLinestring);
                            linestrings.Add(tempLinestring);
                        }

                        if (line.StartsWith("GEOMETRYCOLLECTION"))
                        {
                            string[] data = line.Substring(20).Split(' ');

                            List<Point> pointsCollection = new List<Point>();
                            List<Polygon> polygonsCollection = new List<Polygon>();
                            List<Linestring> linestringsCollection = new List<Linestring>();

                            for (int indexData = 0; indexData < data.Length - 1; indexData++)
                            {
                                if (data[indexData].Contains("POINT"))
                                {
                                    double x = double.Parse(data[indexData + 1].Substring(1));
                                    double y = double.Parse(data[indexData + 2].Substring(0, data[indexData + 2].Length - 2)); // убираем ")" в конце второй координаты

                                    Point tempPoint = new Point(x, y);
                                    pointsCollection.Add(tempPoint);
                                }

                                if (data[indexData].Contains("POLYGON"))
                                {
                                    List<Point> pointsPolygon = new List<Point>();

                                    double x;
                                    double y;

                                    int minIndex = indexData + 1;
                                    int maxIndex = minIndex;

                                    for (int checkIndexMax = minIndex; checkIndexMax < data.Length; checkIndexMax++)
                                    {
                                        if (data[checkIndexMax].Contains("N"))
                                        {
                                            maxIndex = checkIndexMax - 1;
                                            break;
                                        }

                                        else if (data[checkIndexMax].Contains(")"))
                                        {
                                            maxIndex = checkIndexMax;
                                            break;
                                        }
                                    }

                                    for (int tempIndex = minIndex; tempIndex < maxIndex; tempIndex += 2)
                                    {
                                        if (tempIndex == minIndex) x = double.Parse(data[tempIndex].Substring(2));
                                        else x = double.Parse(data[tempIndex]);

                                        if (tempIndex == maxIndex - 1) y = double.Parse(data[tempIndex + 1].Substring(0, data[tempIndex + 1].Length - 3));
                                        else y = double.Parse(data[tempIndex + 1].Substring(0, data[tempIndex + 1].Length - 1));

                                        Point tempPoint = new Point(x, y);
                                        pointsPolygon.Add(tempPoint);
                                    }

                                    Polygon tempPolygon = new Polygon(pointsPolygon);
                                    polygonsCollection.Add(tempPolygon);
                                }

                                if (data[indexData].Contains("LINESTRING"))
                                {
                                    List<Point> pointsLinestring = new List<Point>();

                                    double x;
                                    double y;

                                    int minIndex = indexData + 1;
                                    int maxIndex = minIndex;

                                    for (int checkIndexMax = minIndex; checkIndexMax < data.Length; checkIndexMax++)
                                    {
                                        if (data[checkIndexMax].Contains("N"))
                                        {
                                            maxIndex = checkIndexMax - 1;
                                            break;
                                        }

                                        else if (data[checkIndexMax].Contains(")"))
                                        {
                                            maxIndex = checkIndexMax;
                                            break;
                                        }
                                    }

                                    for (int tempIndex = minIndex; tempIndex < maxIndex; tempIndex += 2)
                                    {
                                        if (tempIndex == minIndex) x = double.Parse(data[tempIndex].Substring(1));
                                        else x = double.Parse(data[tempIndex]);

                                        if (tempIndex == maxIndex - 1) y = double.Parse(data[tempIndex + 1].Substring(0, data[tempIndex + 1].Length - 2));
                                        else y = double.Parse(data[tempIndex + 1].Substring(0, data[tempIndex + 1].Length - 1));

                                        Point tempPoint = new Point(x, y);
                                        pointsLinestring.Add(tempPoint);
                                    }

                                    Linestring tempLinestring = new Linestring(pointsLinestring);
                                    linestringsCollection.Add(tempLinestring);
                                }
                            }

                            Geometrycollection tempGeometrycollection = new Geometrycollection(pointsCollection, polygonsCollection, linestringsCollection);
                            geometrycollections.Add(tempGeometrycollection);
                        }

                        //if (line.StartsWith("GEOMETRYCOLLECTION"))
                        //{
                        //    string tempLine = line.Substring(20); // Вырезаем "GEOMETRYCOLLECTION ("
                        //    int lenghtCheck = 0;
                        //    bool isPolygon = false;

                        //    List<Polygon> polygonsGeometry = new List<Polygon>();

                        //    if (tempLine.StartsWith("POLYGON"))
                        //    {
                        //        lenghtCheck += 10;

                        //        string[] datas = tempLine.Substring(lenghtCheck).Split(' ');

                        //        List<Point> pointsPolygon = new List<Point>();

                        //        double x;
                        //        double y;
                        //        int tempDataIndex = 0;

                        //        foreach (string tempData in datas)
                        //        {
                        //            if (tempData.EndsWith(","))
                        //            {
                        //                if (tempData.EndsWith(")),"))
                        //                {
                        //                    x = double.Parse(datas[tempDataIndex - 1]);
                        //                    y = double.Parse(datas[tempDataIndex].Substring(0, datas[tempDataIndex].Length - 3));

                        //                    tempLine.Substring(lenghtCheck);
                        //                    isPolygon = true;
                        //                }
                        //                else
                        //                {
                        //                    x = double.Parse(datas[tempDataIndex - 1]);
                        //                    y = double.Parse(datas[tempDataIndex].Substring(0, datas[tempDataIndex].Length - 1));
                        //                }

                        //                Point tempPoint = new Point(x, y);
                        //                pointsPolygon.Add(tempPoint);
                        //            }
                        //            else if (tempData.EndsWith(")))"))
                        //            {
                        //                x = double.Parse(datas[tempDataIndex - 1]);
                        //                y = double.Parse(datas[tempDataIndex].Substring(0, datas[tempDataIndex].Length - 3));

                        //                Point tempPoint = new Point(x, y);
                        //                pointsPolygon.Add(tempPoint);
                        //            }

                        //            tempDataIndex++;
                        //            lenghtCheck++;

                        //            if (isPolygon) break;
                        //        }

                        //        Polygon tempPolygon = new Polygon(pointsPolygon);
                        //        polygonsGeometry.Add(tempPolygon);
                        //    }

                        //    Geometrycollection tempGeometrycollection = new Geometrycollection(null, polygonsGeometry, null);
                        //    geometrycollections.Add(tempGeometrycollection);
                        //}
                    }
                }
            }

            foreach (Point tempPoint in points)
            {
                geometries.Add(tempPoint);

                //memoEdit.Text += $"Point({tempPoint.X}, {tempPoint.Y})\r\n";
                //memoEdit.Text += $"X = {tempPoint.X};\r\nY = {tempPoint.Y};";

                //memoEdit.Text += "\r\n\r\n";
            }

            foreach (Polygon tempPolygon in polygons)
            {
                geometries.Add(tempPolygon);

                //memoEdit.Text += "Polygon((";
                //int indexPoint = -1;

                //foreach (Point tempPoint in tempPolygon.point)
                //{
                //    indexPoint++;

                //    if (indexPoint == tempPolygon.point.Count - 1) memoEdit.Text += $"{tempPoint.X} {tempPoint.Y}";
                //    else memoEdit.Text += $"{tempPoint.X} {tempPoint.Y}, ";
                //}

                //memoEdit.Text += "))\r\n\r\n";
            }

            foreach (Linestring tempLinestring in linestrings) { geometries.Add(tempLinestring); }

            foreach (Geometrycollection tempGeometrycollection in geometrycollections) { geometries.Add(tempGeometrycollection); }

            foreach (IGeometry tempGeometry in geometries) { memoEdit.Text += tempGeometry.GetData(); }
        }

        public void SaveFile(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;

            string fileName = saveFileDialog.FileName;

            System.IO.File.WriteAllText(fileName, memoEdit.Text);

            MessageBox.Show("Файл сохранён!");
        }
    }
}