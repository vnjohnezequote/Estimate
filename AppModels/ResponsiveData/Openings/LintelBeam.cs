using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class LintelBeam : BindableBase,IBeam
    {
        #region Field

        private Opening _openingInfo;

        #endregion

        #region Properties

        public Opening OpeningInfo
        {
            get => _openingInfo; 
            set => SetProperty(ref _openingInfo, value);
        }

        #endregion

        #region Constructor

        

        #endregion


    }
}
