// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Layer.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Layer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using Newtonsoft.Json;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall layer.
    /// </summary>
    public class PrenailWallLayer : WallBase
    {

        #region Field


        #endregion

        #region Property

        public override int FinalWallHeight 
        {
            get
            {
                if (WallType.IsLoadBearingWall)
                {
                    return WallHeight;
                }
                else
                {
                    if (WallType.IsRaked)
                    {
                        return WallHeight;
                    }

                    if (IsWallUnderRakedArea || ForcedWallUnderRakedArea)
                    {
                        return WallHeight;
                    }
                    return WallHeight - 35;
                }
            }
        }


        #endregion

        //#region Constructor

        ///// <summary>
        ///// Initializes a new instance of the <see cref="PrenailWallLayer"/> class.
        ///// </summary>
        ///// <param name="wallId">
        ///// The wall Id.
        ///// </param>
        ///// <param name="wallTypePoco">
        ///// The wall type.
        ///// </param>
        ///// <param name="defaultInfo">
        ///// The default info.
        ///// </param>
        public PrenailWallLayer(int wallId, IGlobalWallInfo globalWallInfo, WallTypePoco wallType, int typeID = 1) :
            base(wallId, globalWallInfo, wallType, typeID)
        {

        }


    }
}
