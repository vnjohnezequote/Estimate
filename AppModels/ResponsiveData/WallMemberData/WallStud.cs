// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StudMember.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the StudMember type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    /// <summary>
    /// The stud member.
    /// </summary>
    public class WallStud: WallMemberBase
    {
        #region Field

        #endregion
        #region Property

        public sealed override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.Stud;
        public override int Thickness { get => WallInfo.WallThickness; set {} }

        public int Height => WallInfo.StudHeight;

        #endregion
        #region Constructor
        public WallStud(IWallInfo wallInfo) : base(wallInfo)
        {
            MemberType = WallMemberType.Stud;
            WallInfo.PropertyChanged += WallInfo_PropertyChanged1;
        }

        private void WallInfo_PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="WallThickness")
            {
                RaisePropertyChanged(nameof(Thickness));
            }

            if (e.PropertyName == "StudHeight")
            {
                RaisePropertyChanged(nameof(Height));
            }
        }
        #endregion





    }
}