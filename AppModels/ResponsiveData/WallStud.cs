// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StudMember.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the StudMember type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The stud member.
    /// </summary>
    public class WallStud : WallMemberBase
    {
        #region Field

        #endregion
        #region Property
        public int Height { get; set; }
        #endregion
        #region Constructor
        public WallStud(IWallInfo wallInfo, IWallMemberInfo baseMaterialInfo):base(wallInfo,baseMaterialInfo)
        {
            
        }
        #endregion


    }
}