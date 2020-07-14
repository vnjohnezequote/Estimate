using System.Collections.ObjectModel;

namespace AppModels.Interaface
{
    public interface IJob
    {
        JobInfo Info { get; }
        ObservableCollection<LevelWall> Levels { get; }
    }
}