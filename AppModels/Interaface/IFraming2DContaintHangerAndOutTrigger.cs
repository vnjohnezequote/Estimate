

using System;
using AppModels.CustomEntity;

namespace AppModels.Interaface
{
    public interface IFraming2DContaintHangerAndOutTrigger: IFraming2D,I2DContaintHanger,I2DContaintOutTrigger,IRectangleSolid
    {
        bool IsShowFramingName { get; set; }
        Guid? FramingNameId { get; set; }
        FramingNameEntity FramingName { get; set; }
    }
}