using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;

namespace AppModels.Interaface
{
    public interface IFramingVmBase
    {
        IEntitiesManager EntitiesManager { get; }
        TimberBase FramingInfo { get; set; }
        EngineerMemberInfo EngineerMember { get; set; }
        string BeamGrade { get; set; }
        FramingTypes FramingType { get; set; }
        int FullLength { get; }
        double QuoteLength { get; }
        bool IsExisting { get; }


    }
}