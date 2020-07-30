using System.Collections.ObjectModel;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface IJob
    {
        JobInfo Info { get; }
        GlobalWallInfo GlobalWallInfo { get; }
        ObservableCollection<LevelWall> Levels { get; }

        void LoadJob(JobModelPoco jobOpen);
    }
}