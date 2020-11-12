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
       private int _qty;
       public Guid Id { get; set; }
       public HangerMat HangerMaterial { get=>_hangerMat; set=>SetProperty(ref _hangerMat,value); }
       public int Qty
       {
           get => _qty;
           set => SetProperty(ref _qty, value);
       }

       public Hanger()
       {
            Id = Guid.NewGuid();
       }
    }
}
