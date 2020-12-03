using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using devDept.Eyeshot.Entities;

namespace AppModels.Interaface
{
    public interface IFramingVmBase
    {
        IEntitiesManager EntitiesManager { get; }
       
        TimberBase FramingInfo { get; set; }
        EngineerMemberInfo EngineerMember { get; set; }
        string BeamGrade { get; set; }
        string Name { get; set; }
        FramingTypes FramingType { get; set; }
        int Index { get; }
        int SubFixIndex { get; }
        int FullLength { get; }
        double QuoteLength { get; }
        bool IsExisting { get; }


    }
}