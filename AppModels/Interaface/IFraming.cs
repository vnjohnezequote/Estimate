using System;
using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;

namespace AppModels.Interaface
{
    public interface IFraming : INotifyPropertyChanged, ICloneable
    {
        Guid Id { get; }
        //Guid LevelId { get; }
        //Guid FramingSheetId { get; }
        LevelWall Level { get; set; }
        string Name { get; set; }
        string NamePrefix { get; }
        int SubFixIndex { get; set; }
        bool IsExisting { get; set; }
        double QuoteLength { get; }
        int FramingSpan { get; }
        int FullLength { get; set; }
        double ExtraLength { get; set; }
        double Pitch { get; set; }
        FramingSheet FramingSheet { get; }
        TimberBase FramingInfo { get; set; }
        string TimberGrade { get; set; }
        FramingTypes FramingType { get; set; }
        int Quantity { get; set; }
        int Index { get; set; }
        EngineerMemberInfo EngineerMember { get; set; }
        bool IsLongerStockList { get; }

    }
}