using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class Hanger : BindableBase
    {
        private HangerMat _hangerMat;
       public HangerMat HangerMaterial { get=>_hangerMat; set=>SetProperty(ref _hangerMat,value); }
    }
}
