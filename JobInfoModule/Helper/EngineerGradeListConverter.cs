﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using JobInfoModule.ViewModels;
using Syncfusion.UI.Xaml.Grid;

namespace JobInfoModule.Helper
{
    public class EngineerGradeListConverter: IItemsSourceSelector
    {
        private Dictionary<string, List<string>> TimberList { get; } = new Dictionary<string,List<string>>()
        {
            
        };

        public EngineerGradeListConverter()
        {
            CreateData();
        }


        private void CreateData()
        {
            var lvl = new List<string>()
            {
                "90x35 LVL",
                "2/90x35 LVL",
                "90x45 LVL",
                "2/90x45 LVL",
            };
            TimberList.Add("LVL",lvl);
            var mgp10 = new List<string>()
            {
                "90x35 MGP10",
                "2/90x35 MGP10",
                "90x45 MGP10",
                "2/90x45 MGP10",
            };
            TimberList.Add("MGP10", mgp10);
            var mgp12 = new List<string>()
            {
                "90x35 MGP12",
                "2/90x35 MGP12",
                "90x45 MGP12",
                "2/90x45 MGP12",
            };
            TimberList.Add("MGP12", mgp12);
            var gl17C = new List<string>()
            {
                "165x65 17C",
                "165x85 17C",
                "195x65 17C",
                "195x85 17C",
            };
            TimberList.Add("17C", gl17C);

        }
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            switch (record)
            {
                case ITimberInfo member when string.IsNullOrEmpty(member.TimberGrade):
                    return null;
                case ITimberInfo member:
                {
                    var grade = member.TimberGrade;
                    if (!TimberList.ContainsKey(grade)) return null;
                    TimberList.TryGetValue(grade, out var timberList);
                    return timberList ?? null;
                }
                case Opening door when door.Lintel == null:
                    return null;
                case Opening door when string.IsNullOrEmpty(door.Lintel.TimberGrade):
                    return null;
                case Opening door:
                {
                    var grade = door.Lintel.TimberGrade;
                    if (!TimberList.ContainsKey(grade)) return null;
                    TimberList.TryGetValue(grade, out var timberList);
                    return timberList ?? null;
                }
                default:
                    return null;
            }
        }
    }
}
