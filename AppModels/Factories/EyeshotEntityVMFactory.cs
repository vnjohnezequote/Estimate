using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using GeometryGym.Ifc;

namespace AppModels.Factories
{
    public static class EyeshotEntityVMFactory
    {
        public static EntityVm CreateEntityVm(this Entity entity)
        {
            return new EntityVm(entity);
        }

        public static LineVm CreateLineVm(this Line line)
        {
            return new LineVm(line);
        }

        public static LinearPathVm CreateLinearPathVm(this LinearPath linearPath)
        {
            return new LinearPathVm(linearPath);
        }

        public static TextVm CreateTextVm(this Text text)
        {
            return new TextVm(text);
        }

        public static LeaderVM CreateLeaderVm(this Leader leader)
        {
            return new LeaderVM(leader);
        }

        public static VectorViewVm CreateVectorViewVm(this VectorView vectorView)
        {
            return new VectorViewVm(vectorView);
        }

        public static BlockReferenceVm CreateBlockReferenceVm(this BlockReference blockRef)
        {
            return new BlockReferenceVm(blockRef);
        }
    }
}
