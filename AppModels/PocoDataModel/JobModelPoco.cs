using System.Collections.Generic;
using System.Linq;
using AppModels.Interaface;
using AppModels.PocoDataModel.EngineerMember;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class JobModelPoco
    {
        [ProtoMember(1)]
        public JobInfoPoco Info { get; set; }
        [ProtoMember(2)] 
        public GlobalWallInfoPoco GlobalWallInfo { get; set; }
        [ProtoMember(3)]
        public List<LevelWallPoco> Levels { get; set; }
        [ProtoMember(4)]
        public List<EngineerMemberInfoPoco> EngineerMemberList { get; set; }
        [ProtoMember(5)]
        public List<OpeningInfoPoco> DoorSchedules { get; set; }

        public JobModelPoco()
        {

        }
        public JobModelPoco(IJob jobModel)
        {
            Info = new JobInfoPoco(jobModel.Info);
            GlobalWallInfo = new GlobalWallInfoPoco(jobModel.GlobalWallInfo);
            InitializerEngineerList(jobModel.EngineerMemberList.ToList());
            InitializerDoorSchedules(jobModel.DoorSchedules.ToList());
            InitializerLevel(jobModel.Levels.ToList());
        }

        private void InitializerLevel(List<LevelWall> levels)
        {
            Levels = new List<LevelWallPoco>();
            foreach (var levelPoco in levels.Select(levelWall => new LevelWallPoco(levelWall)))
            {
                Levels.Add(levelPoco);
            }
        }

        private void InitializerEngineerList(List<EngineerMemberInfo> engineerMemberInfos)
        {
            EngineerMemberList=new List<EngineerMemberInfoPoco>();
            foreach (var engineerMemberPoco in engineerMemberInfos.Select(engineerMemberInfo => new EngineerMemberInfoPoco(engineerMemberInfo)))
            {
                EngineerMemberList.Add(engineerMemberPoco);
            }
        }

        private void InitializerDoorSchedules(List<OpeningInfo> doorSchedules)
        {

           DoorSchedules = new List<OpeningInfoPoco>();
           foreach (var doorSchedule in doorSchedules)
           {
             var doorSchedulePoco = new OpeningInfoPoco(doorSchedule);   
             DoorSchedules.Add(doorSchedulePoco);
           }
        }
    }
}
