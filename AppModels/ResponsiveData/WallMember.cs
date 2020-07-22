

using AppModels.Interaface;

namespace AppModels.ResponsiveData
{
    public class WallMember: TimberWallMemberBase
    {
        //public GlobalMemberInfo { get; set; }
        public override string NoItem
        {
            get => string.IsNullOrEmpty(_noItem) ? GlobalMemberInfo.NoItem : this._noItem;
            set
            {
                this.SetProperty(ref this._noItem, value);
                this.CallBackPropertyChanged();
            }
        }
        public override string Depth
        {
            get => string.IsNullOrEmpty(_depth) ? GlobalMemberInfo.Depth : this._depth;
            set
            {
                this.SetProperty(ref this._depth, value);
                this.CallBackPropertyChanged();
            }
        }

        public override string TimberGrade
        {
            get => string.IsNullOrEmpty(_timberGrade) ? GlobalMemberInfo.TimberGrade : _timberGrade;
            set
            {
                if (value == GlobalMemberInfo.TimberGrade)
                {
                    value = string.Empty;
                }
                this.SetProperty(ref this._timberGrade, value);
                CallBackPropertyChanged();
            }
        }
        public WallMember(IGlobalWallInfo globalWallInfo,TimberWallMemberBase topPlateInfo) : base(globalWallInfo)
        {
            GlobalMemberInfo = topPlateInfo;
            GlobalMemberInfo.PropertyChanged += TopPlate_PropertyChanged;
        }

        private void TopPlate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Thickness")
            {
                RaisePropertyChanged(nameof(Thickness));
            }
            if (e.PropertyName == "Depth")
            {
                RaisePropertyChanged(nameof(Depth));
            }

            if (e.PropertyName=="NoItem")
            {
                RaisePropertyChanged(nameof(NoItem));
            }

            if (e.PropertyName == "TimberGrade")
            {
                RaisePropertyChanged(nameof(TimberGrade));
            }
        }
    }
}
