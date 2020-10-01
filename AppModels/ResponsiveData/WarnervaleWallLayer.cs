using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.PocoDataModel;

namespace AppModels.ResponsiveData
{
    public class WarnervaleWallLayer : PrenailWallLayer
    {
        public override string WallName
        {
            get
            {
                if (this.Stud!=null && this.WallType!=null)
                {
                    return this.Stud.Size + " " + this.WallType.AliasName.Substring(0, 3) + " " + FinalWallHeight +
                           " " + WallSpacing + "C/C " + this.WallType.AliasName + " TYPE " + this.TypeId + " "+LevelName.ToUpper()+" PLAN "+this.WallLength+"LM";
                }

                return string.Empty;
            }
        }

        public WarnervaleWallLayer(int wallId, IGlobalWallInfo globalWallInfo, WallTypePoco wallType,string levelname, int typeID = 1) : base(wallId, globalWallInfo, wallType, levelname,typeID)
        {
            PropertyChanged += WarnervaleWallLayer_PropertyChanged;
        }

        private void WarnervaleWallLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //RaisePropertyChanged(nameof(WallName));
            if (e.PropertyName==nameof(TypeId)||e.PropertyName==nameof(WallLength)||e.PropertyName==nameof(WallSpacing)||e.PropertyName==nameof(WallThickness))
            {
                RaisePropertyChanged(nameof(WallName));
            }
        }
    }
}
