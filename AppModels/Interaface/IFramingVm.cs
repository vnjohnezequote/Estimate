namespace AppModels.Interaface
{
    public interface IFramingVm: IFramingVmBase
    {
        double Pitch { get; set; }
       
        double ExtraLength { get; set; }
        bool IsShowFramingName { get; set; }
        
    }
}