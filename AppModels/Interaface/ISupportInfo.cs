using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface ISupportInfo
    {

        MaterialTypes MaterialType { get; }
        int NoItem { get; }
        int Thickness { get; }
        int Depth { get; }
        string Material { get; }
    }
}