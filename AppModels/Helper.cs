using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using devDept.Geometry;

namespace AppModels
{
    public static class Helper
    {
        public static int RoundUpTo300(this int number)
        {
            return (int)(Math.Ceiling((double)number / 300)*300);
        }

        public static int RoundUpto5(this double number)
        {
            return (int)(Math.Ceiling(number / 5) * 5);
        }
        public static bool ConvertStringToDouble(string inputString, out double outputDouble)
        {
            outputDouble = 0;

            if (string.IsNullOrEmpty(inputString))
            {
                outputDouble = 0;
                return false;
            }
            var match = Regex.Match(inputString, @"([+|-]?[\d+]?[+|\-|\*|\/]?\d+)+");
            if (match.Success)
            {
                var dataTable = new DataTable();
                var output = dataTable.Compute(match.Value, String.Empty);
                outputDouble = Convert.ToDouble(output);

            }

            if (Math.Abs(outputDouble) > 0.0001)
            {
                return true;
            }

            return false;
        }
        public static Point3D ConvertPoint2DtoPoint3D(this Point2D point)
        {
            return new Point3D(point.X, point.Y, 0);
        }

        public static Point2D ConvertToPoint2D(this Point3D point)
        {
            return new Point2D(point.X,point.Y);
        }

        public static List<Point3D> SortPointInLine(List<Point3D> points)
        {
            var listocheck = new List<Point3D>(points);
                listocheck.Remove(points[0]);
                listocheck.Remove(points[points.Count - 1]);

                List<Point3D> sortedPoints = new List<Point3D>();
                sortedPoints.Add(points[0]);
                var i = 0;
                while (listocheck.Count > 0)
                {
                    var distance = double.MaxValue;
                    Point3D pointadd = null;
                    foreach (var point3D in listocheck)
                    {
                        var d = sortedPoints[i].DistanceTo(point3D);
                        if (d < distance)
                        {
                            pointadd = point3D;
                            distance = d;
                        }
                    }
                    sortedPoints.Add(pointadd);
                    listocheck.Remove(pointadd);
                    i++;
                }
                sortedPoints.Add(points[points.Count - 1]);
                return sortedPoints;
        }
    }
}
