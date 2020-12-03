using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface ITimberVm
    {
        string BeamGrade { get; set; }
        TimberBase FramingInfo { get; set; }
    }
}