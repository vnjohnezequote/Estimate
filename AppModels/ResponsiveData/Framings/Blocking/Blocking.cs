using System.Collections.Generic;
using AppModels.Enums;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.Blocking;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.EngineerMember;

namespace AppModels.ResponsiveData.Framings.Blocking
{
    public class Blocking: FramingBase
    {
        #region Field

        private BlockingTypes _blockingType;
        #endregion

        #region Properties
        public override double QuoteLength => ((double)FullLength / 1000);
        
        public Blocking(Blocking another) : base(another)
        {

        }
        
        public override object Clone()
        {
            return new Blocking(this);
        }

        public BlockingTypes BlockingType
        {
            get => _blockingType;
            set
            {
                SetProperty(ref _blockingType, value);
                RaisePropertyChanged(nameof(NoOfBlocking));
                SetQuantities();
            } 
        }

        public int NoOfBlocking => BlockingType == BlockingTypes.SingleBlocking ? 1 : 2;
        public Blocking():base()
        {
            

        }

        public Blocking(FramingSheet framingSheet) : base(framingSheet)
        {
            FramingType = FramingTypes.Blocking;
        }

        public Blocking(BlockingPoco blockingPoco,FramingSheet framingSheet, List<TimberBase> timberList,List<EngineerMemberInfo> engineerMemberInfos):base(blockingPoco,framingSheet,timberList,engineerMemberInfos)
        {
            BlockingType = blockingPoco.BlockingType;
            FramingType = FramingTypes.Blocking;
        }

        private void SetQuantities()
        {
            if (BlockingType == BlockingTypes.SingleBlocking)
            {
                Quantity = 1;
            }
            else
            {
                Quantity = 2;
            }
        }
        #endregion

    }
}
