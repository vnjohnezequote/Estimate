using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Factories;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public abstract class WallMemberBase: BindableBase,IWallMember
    {
        #region Field
        private IWallMemberInfo _timberMaterialInfo;
        #endregion

        #region Property
        public IWallMemberInfo TimberMaterialInfo
        {
            get => this._timberMaterialInfo;
            set => this.SetProperty(ref this._timberMaterialInfo, value);
        }
        public IWallInfo WallInfo { get; set; }
        public int Thickness => WallInfo.WallThickness;

        #endregion

        #region Constructor

        protected WallMemberBase(IWallInfo wallInfo, IWallMemberInfo baseMaterialInfo)
        {
            WallInfo = wallInfo;
            TimberMaterialInfo = WallMemberFactory.CreateWallMemberInfo(baseMaterialInfo, wallInfo);
            WallInfo.PropertyChanged += WallInfo_PropertyChanged;

        }

        private void WallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(Thickness))
            {
                RaisePropertyChanged(propertyName);
            }
            
        }

        #endregion
    }
}
