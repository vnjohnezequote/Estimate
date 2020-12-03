using System;
using AppModels.Enums;
using AppModels.ResponsiveData.Framings;

namespace AppModels.Interaface
{
    public interface IFraming2D
    {
        Guid Id { get; set; }
        //Guid LevelId { get; set; }
        //Guid FramingSheetId { get; set; }
        //Guid FramingReferenceId { get; }
        //FramingTypes FramingType { get; set; }
        IFraming FramingReference { get; }
        Guid FramingReferenceId { get; set; }
        double FullLength { get; set; }
        //void SetFramingType(FramingTypes framingType);

    }
}