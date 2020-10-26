using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;

namespace AppModels.Interaface
{
    public interface IJob
    {
        JobInfo Info { get; }
        GlobalWallInfo GlobalWallInfo { get; }
        ObservableCollection<LevelWall> Levels { get; }
        //ObservableCollection<EngineerMemberInfo> EngineerMemberList { get; }
        MyObservableCollection<EngineerMemberInfo> EngineerMemberList { get; }
        ObservableCollection<OpeningInfo> DoorSchedules { get; }
        FramingSheet ActiveFloorSheet { get; set; }
        void LoadJob(JobModelPoco jobOpen,List<ClientPoco> clients);
    }
}