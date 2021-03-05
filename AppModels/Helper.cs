using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
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

        public static bool IsListContainerSameMaterial(this List<IFraming> framingList, IFraming framingCheck, ref SameFramingTypes sameFramingType,out IFraming sameInfoFraming,ref int maxIndex,ref int minSubIndex)
        {
            sameInfoFraming = null;
            foreach (var framing in framingList)
            {
                if (framing == framingCheck)
                {
                    continue;
                }

                if (framing.FramingType !=framingCheck.FramingType)
                {
                    continue;
                }

                if (framing.FramingInfo == framingCheck.FramingInfo)
                {
                    sameInfoFraming = framing;
                    if (Math.Abs(framing.QuoteLength - framingCheck.QuoteLength) < 0.0001)
                    {
                        sameFramingType = SameFramingTypes.SameInfoSameLength;
                    }
                    sameFramingType = SameFramingTypes.SameInfoDifferneceLength;
                    return true;
                }
                else
                {
                    if (maxIndex<framing.Index)
                    {
                        maxIndex = framing.Index;
                    }
                }
            }
            return false;
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

        public static void RegenerationFramingName(List<IFraming> framingList)
        {
            Dictionary<FramingTypes, Dictionary<TimberBase, Dictionary<double, List<IFraming>>>> framingDict =
                new Dictionary<FramingTypes, Dictionary<TimberBase, Dictionary<double, List<IFraming>>>>();
            Dictionary<FramingTypes, Dictionary<TimberBase, Dictionary<double, List<IFraming>>>> sortedframingDict =
                new Dictionary<FramingTypes, Dictionary<TimberBase, Dictionary<double, List<IFraming>>>>();
            foreach (var framing in framingList)
            {
                if (framing.FramingInfo == null)
                {
                    continue;
                }
                if (!framingDict.ContainsKey(framing.FramingType))
                {
                    Dictionary<TimberBase, Dictionary<double, List<IFraming>>> framingInfoDict =
                        new Dictionary<TimberBase, Dictionary<double, List<IFraming>>>();
                    Dictionary<double, List<IFraming>> framingLengthDict = new Dictionary<double, List<IFraming>>();
                    var tempFramings = new List<IFraming>();
                    tempFramings.Add(framing);
                    framingLengthDict.Add(framing.QuoteLength,tempFramings);
                    framingInfoDict.Add(framing.FramingInfo,framingLengthDict);
                    framingDict.Add(framing.FramingType, framingInfoDict);
                }
                else
                {
                    framingDict.TryGetValue(framing.FramingType, out var framingInfoDic);
                    if (framingInfoDic!=null)
                    {
                        if (!framingInfoDic.ContainsKey(framing.FramingInfo))
                        {
                            Dictionary<double, List<IFraming>> framingLengthDict =
                                new Dictionary<double, List<IFraming>>();
                            var temtFramings = new List<IFraming>();
                            temtFramings.Add(framing);
                            framingLengthDict.Add(framing.QuoteLength,temtFramings);
                            framingInfoDic.Add(framing.FramingInfo,framingLengthDict);
                        }
                        else
                        {
                            framingInfoDic.TryGetValue(framing.FramingInfo, out var framingLengthDic);
                            if (framingLengthDic!=null)
                            {
                                if (!framingLengthDic.ContainsKey(framing.QuoteLength))
                                {
                                    var tempFramings = new List<IFraming>();
                                    tempFramings.Add(framing);
                                    framingLengthDic.Add(framing.QuoteLength,tempFramings);
                                }
                                else
                                {
                                    framingLengthDic.TryGetValue(framing.QuoteLength, out var framingAddList);
                                    if (framingAddList != null)
                                    {
                                        framingAddList.Add(framing);
                                    }
                                        
                                }
                            }
                        }
                    }
                }
            }
            foreach (var framingType in framingDict)
            {
                var tempInfoDic = new Dictionary<TimberBase, Dictionary<double, List<IFraming>>>();
                var sortedInforDic = new Dictionary<TimberBase, Dictionary<double, List<IFraming>>>();
                foreach (var inforFraming in framingType.Value)
                {
                    var lengthDic = inforFraming.Value;
                    Dictionary<double, List<IFraming>> sortedLengthDic = new Dictionary<double, List<IFraming>>();
                    var items = from pair in lengthDic orderby pair.Value.Count descending select pair;
                    foreach (var item in items)
                    {
                        sortedLengthDic.Add(item.Key, item.Value);
                    }

                    tempInfoDic.Add(inforFraming.Key, sortedLengthDic);
                }

                var items2 = from pair in tempInfoDic orderby pair.Value.Count descending select pair;
                foreach (var item in items2)
                {
                    sortedInforDic.Add(item.Key, item.Value);
                }
                sortedframingDict.Add(framingType.Key, sortedInforDic);
            }
            foreach (var framingType in sortedframingDict)
            {
                var index = 1;
                foreach (var framingInfor in framingType.Value)
                {
                    var subIndex = 0;
                    foreach (var framingLengths in framingInfor.Value)
                    {

                        foreach (var framing in framingLengths.Value)
                        {
                            framing.Index = index;
                            framing.SubFixIndex = subIndex;
                        }

                        subIndex++;

                    }

                    index++;
                }
            }
        }

        public static void RegenerationHangerName(List<Hanger> hangerList)
        {
            Dictionary<HangerMat, List<Hanger>> hangerDict = new Dictionary<HangerMat, List<Hanger>>();
            foreach (var hanger in hangerList)
            {
                if (hanger.HangerMaterial != null)
                {
                    if (!hangerDict.ContainsKey(hanger.HangerMaterial))
                    {
                        var listHanger = new List<Hanger>();
                        listHanger.Add(hanger);
                        hangerDict.Add(hanger.HangerMaterial, listHanger);
                    }
                    else
                    {
                        hangerDict.TryGetValue(hanger.HangerMaterial, out var hangers);
                        if (hangers != null)
                        {
                            hangers.Add(hanger);
                        }
                    }
                }
            }

            var index = 1;
            foreach (var hangerk in hangerDict)
            {
                foreach (var hanger in hangerk.Value)
                {
                    hanger.Index = index;
                }
                index++;
            }
        }

        public static void RegenerationBeamName(List<IFraming> beams)
        {
            foreach (var beam in beams)
            {
                beam.Index = beams.IndexOf(beam) + 1;
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
