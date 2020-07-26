// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timber.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Timber type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    /// <summary>
    /// The timber.
    /// </summary>
    public abstract class WallMember :WallMemberBase
    {
        #region Filed



        #endregion
        #region Property

        public double Length => NoItem*WallInfo.WallLength;

        public override int Thickness => _thickness != 0 ? _thickness : BaseMaterialInfo.Thickness;

        #endregion

        #region Constructor

        #endregion

        protected WallMember(IWallInfo wallInfo) : base(wallInfo)
        {
        }

        protected override void WallInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.WallInfo_PropertyChanged(sender, e);
            if (e.PropertyName =="WallLength")
            {
                RaisePropertyChanged(nameof(Length));
            }
        }
    }
}
