﻿using System.Security.Permissions;
using System.Text;
using devDept.Eyeshot.Entities;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class TimberBasePoco
    {
        [ProtoMember(1)]
        public ushort Id { get; set; }
        [ProtoMember(2)]
        public ushort NoItem { get; set; }
        [ProtoMember(3)]
        public ushort Thickness { get; set; }
        [ProtoMember(4)]
        public ushort Depth { get; set; }
        [ProtoMember(5)]
        public string TimberGrade { get; set; }
        [ProtoMember(6)]
        public string Treatment { get; set; }
        [ProtoMember(7)]
        public double UnitPrice { get; set; }
        [ProtoMember(8)]
        public string CurrencyUnit { get; set; }
        [ProtoMember(9)]
        public double MaximumLength { get; set; }
        public TimberBasePoco(ushort id, ushort noItem = 1, ushort thickness= 90, ushort depth =35, string timberGrade = "MGP10",string treatment ="Untreated",double unitPrice = 0, string currencyUnit = "AUD",double maximumLength = 6.0)
        {
            this.Id = id;
            this.NoItem = noItem;
            this.Thickness = thickness;
            this.Depth = depth;
            this.TimberGrade = timberGrade;
            this.Treatment = treatment;
            this.UnitPrice = unitPrice;
            this.CurrencyUnit = currencyUnit;
            this.MaximumLength = maximumLength;
        }
        public TimberBasePoco(TimberBasePoco another)
        {
            this.Id = another.Id;
            this.NoItem = another.NoItem;
            this.Thickness = another.Thickness;
            this.Depth = another.Depth;
            this.TimberGrade = another.TimberGrade;
            this.Treatment = another.Treatment;
            this.UnitPrice = another.UnitPrice;
            this.CurrencyUnit = another.CurrencyUnit;
            this.MaximumLength = another.MaximumLength;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.NoItem !=0)
            {
                stringBuilder = stringBuilder.Append(NoItem + "/");
            }

            stringBuilder = stringBuilder.Append(Thickness + "x" + Depth + " "+ TimberGrade + " " + Treatment + "/" + UnitPrice + CurrencyUnit);

            return stringBuilder.ToString();
        }
    }
}