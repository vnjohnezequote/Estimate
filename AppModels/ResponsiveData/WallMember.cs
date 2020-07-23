// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timber.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Timber type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The timber.
    /// </summary>
    public class WallMember : WallMemberBase
    {
        #region Filed



        #endregion
        #region MyRegion


        public double Length { get; set; }
        #endregion

        #region Constructor
        public WallMember(IWallInfo wallInfo,IWallMemberInfo baseMaterialInfo):base(wallInfo,baseMaterialInfo)
        {
            
        }
        #endregion
    }
}
