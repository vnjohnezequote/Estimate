using System.Collections.Generic;
using System.ComponentModel;
using AppModels.Enums;
using AppModels.PocoDataModel.Framings.Blocking;
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
        protected override List<FramingTypes> FramingTypeAccepted { get; } = new List<FramingTypes>() {FramingTypes.Blocking};

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
            FramingSheet.PropertyChanged+= FramingSheetOnPropertyChanged;
        }

        private void FramingSheetOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FramingSheet.FramingSpacing))
            {
                this.FullLength = FramingSheet.FramingSpacing;
            }    
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
