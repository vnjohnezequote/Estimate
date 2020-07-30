using System.Collections.ObjectModel;
using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface IJob
    {
        JobInfo Info { get; }
        GlobalWallInfo GlobalWallInfo { get; }
        ObservableCollection<LevelWall> Levels { get; }
    }
}