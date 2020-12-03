using AppModels.CustomEntity;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Interaface
{
    public interface IFraming2DContainHangerAndOutTriggerVm:IFramingVm
    {
        Hanger2D HangerA { get; }
        Hanger2D HangerB { get; }
        bool IsHangerA { get; set; }
        bool IsHangerB { get; set; }
        HangerMat HangerAMat { get; set; }
        HangerMat HangerBMat { get; set; }
        OutTrigger2D OutTriggerA { get;}
        OutTrigger2D OutTriggerB { get;}
        bool IsOutriggerA { get; set; }
        bool IsOutriggerB { get; set; }
        bool OutTriggerAFlipped { get; set; }
        bool OutTriggerBFlipped { get; set; }
        TimberBase OutTriggerAMat { get; set; }
        TimberBase OutTriggerBMat { get; set; }
        string OutTriggerAGrade { get; set; }
        string OutTriggerBGrade { get; set; }
        int OutTriggerAOutSize { get; set; }
        int OutTriggerBOutSize { get; set; }
        int OutTriggerAInSize { get; set; }
        int OutTriggerBInSize { get; set; }
        
    }
}