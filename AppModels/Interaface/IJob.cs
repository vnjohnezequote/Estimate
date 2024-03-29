﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IJob: INotifyPropertyChanged
    {
        JobInfo Info { get; }
        GlobalWallInfo GlobalWallInfo { get; }
        ObservableCollection<LevelWall> Levels { get; }
        //ObservableCollection<EngineerMemberInfo> EngineerMemberList { get; }
        MyObservableCollection<EngineerMemberInfo> EngineerMemberList { get; }
        bool CurrentIsLoadBearingWall { get; set; }
        bool CCMode { get; set; }
        int DefaultJoistSpacing { get; set; }
        ObservableCollection<OpeningInfo> DoorSchedules { get; }
        FramingSheet ActiveFloorSheet { get; set; }
        TimberBase SelectedJoitsMaterial { get; set; }
        SelectionTypes SelectionType { get; set; }
        int SelectedWallThickness { get; set; }
        void LoadJob(JobModelPoco jobOpen,List<ClientPoco> clients);
    }
}