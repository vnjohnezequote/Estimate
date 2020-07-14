namespace AppModels.Interaface
{
    public interface IWall2D
    {
        string WallLevelName { get; set; }
        double Length();
    }
}