﻿using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    public class WallNogging: WallMemberBase
    {
        public int Length => WallInfo?.NoggingLength ?? 0;

        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.Nogging;

        public override int Thickness
        {
            get
            {
                if (_thickness != 0)
                    return _thickness;

                return WallInfo.NoggingMethod == NoggingMethodType.AsGlobal ? BaseMaterialInfo.Thickness : WallInfo.WallThickness;
            }
            set => SetProperty(ref _thickness, value);
        }

        public sealed override WallMemberType MemberType { get; protected set; }


        public WallNogging(IWallInfo wallInfo) : base(wallInfo)
        {
            MemberType = WallMemberType.Nogging;
        }

        protected override void WallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NoggingLength")
            {
                RaisePropertyChanged(nameof(Length));
            }
        }
    }
}
