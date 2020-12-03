using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using devDept.Geometry;
using devDept.Graphics;

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
        public static List<Point3D> SortPointInLine(List<Point3D> points)
        {
            var point1 = points[0];
            var point2 = points[points.Count - 1];
            var resultPoints = new List<Point3D>();
            point2 = FindFarPoint(points, point1);
            point1 = FindFarPoint(points, point2);
            points.Remove(point1);
            points.Remove(point2);
            resultPoints.Add(point1);
            var i = 0;
            while (points.Count>0)
            {
                var addPoint = FindClosedPoint(points, resultPoints[i]);
                points.Remove(addPoint);
                resultPoints.Add(addPoint);
                i++;
            }
            resultPoints.Add(point2);
            return resultPoints;

        }

        public static double RoundToNearestFive(this double value)
        {
            int valueTest = (int) value;
            var phandu = value - valueTest;
            if (phandu < 0.3)
            {
                return (Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2);
            }
            else if (phandu >= 0.3 && phandu < 0.5)
            {
                return  (Math.Ceiling(value * 2) / 2);
            }
            else if (phandu >= 0.5 && phandu < 0.7)
            {
                return (Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2);
            }
            else
            {
                return (Math.Ceiling(value * 2) / 2);
            }
        }
        public static Point3D FindFarPoint(List<Point3D> points, Point3D pointToCheck)
        {
            var maxDistance = 0.0;
            Point3D resultPoint = null;
            foreach (var point3D in points)
            {
                var distance = point3D.DistanceTo(pointToCheck);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    resultPoint = point3D;
                }
            }

            return resultPoint;
        }

        public static Point3D FindClosedPoint(List<Point3D> points, Point3D checkPoint)
        {
            var minDist = Double.MaxValue;
            Point3D resultPoint = null;
            foreach (var point3D in points)
            {
               
                    var disc = point3D.DistanceTo(checkPoint);
                    if (disc<minDist)
                    {
                        minDist = disc;
                        resultPoint = point3D;
                    }
                
            }

            return resultPoint;
        }
    }
}
