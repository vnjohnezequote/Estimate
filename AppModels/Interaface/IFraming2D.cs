using System;
using devDept.Eyeshot.Entities;

namespace AppModels.Interaface
{
    public interface IFraming2D: IEntity
    {
        Guid Id { get; set; }
        Guid LevelId { get; set; }
        Guid FramingSheetId { get; set; }

        //Guid FramingReferenceId { get; }
        //FramingTypes FramingType { get; set; }
        IFraming FramingReference { get; set; }
        Guid FramingReferenceId { get; set; }
        double FullLength { get; set; }
        //colorMethodType ColorMethod { get; set; }
        //void SetFramingType(FramingTypes framingType);

    }
}