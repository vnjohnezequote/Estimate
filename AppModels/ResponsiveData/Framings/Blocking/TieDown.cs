using System;
using AppModels.Database;
using AppModels.PocoDataModel.Framings.Blocking;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.Blocking
{
    public class TieDown: BindableBase
    {
        private TieDownMat _material;
        private int _qty;
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public FramingSheet FramingSheet { get; set; }
        public LevelWall Level { get; set; }
        public TieDownMat Material { get=>_material; set=>SetProperty(ref _material,value); }
        public int Qty { get=>_qty; set=>SetProperty(ref _qty,value); }
        
        public TieDown()
        {
            
        }

        public TieDown(FramingSheet framignSheet)
        {
            Id = Guid.NewGuid();
            LevelId = framignSheet.LevelId;
            FramingSheetId = framignSheet.Id;
            FramingSheet = framignSheet;
        }
        public TieDown (TieDownPoco tiewDownPoco, TieDownMat timberMat)
        {
            Id = tiewDownPoco.Id;
            Qty = tiewDownPoco.Qty;
            Material = timberMat;
        }

    }
}
