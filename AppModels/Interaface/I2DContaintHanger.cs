using System;
using AppModels.CustomEntity;

namespace AppModels.Interaface
{
    public interface I2DContaintHanger
    {
        Guid? HangerAId { get; set; }
        Guid? HangerBId { get; set; }
        Hanger2D HangerA { get; set; }
        Hanger2D HangerB { get; set; }
        bool IsHangerA { get; set; }
        bool IsHangerB { get; set; }
    }
}