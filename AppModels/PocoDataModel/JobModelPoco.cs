using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class JobModelPoco
    {
        [ProtoMember(1)]
        public JobInfoPoco Info { get; set; }
        [ProtoMember(2)]
        public List<LevelWallPoco> Levels { get; set; }

        public JobModelPoco()
        {

        }
        public JobModelPoco(IJob jobModel)
        {
            Info = new JobInfoPoco(jobModel.Info);
            InitializerLevel(jobModel.Levels.ToList());
        }

        private void InitializerLevel(List<LevelWall> levels)
        {
            Levels = new List<LevelWallPoco>();
            foreach (var levelWall in levels)
            {
                var levelPoco = new LevelWallPoco(levelWall);
                Levels.Add(levelPoco);
            }
        }
    }
}
