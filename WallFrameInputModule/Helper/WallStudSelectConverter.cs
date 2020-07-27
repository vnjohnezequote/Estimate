// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallStudSelectConverter.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallStudSelectConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.ResponsiveData;

namespace WallFrameInputModule.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using AppModels;

    using Syncfusion.UI.Xaml.Grid;

    using WallFrameInputModule.ViewModels;

    /// <summary>
    /// The wall stud select converter.
    /// </summary>
    public class WallStudSelectConverter : IItemsSourceSelector
    {
        /// <summary>
        /// The get items source.
        /// </summary>
        /// <param name="record">
        /// The record.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            if (record == null)
            {
                return null;
            }

            var wallInfor = record as WallLayer;
            string wallKey = "LBW";
            if (!wallInfor.WallType.IsLoadBearingWall)
            {
                wallKey = "NONLBW";
            }
            var wallThickness = wallInfor.WallThickness;


            if (dataContext is PrenailFloorInputViewModel viewModel && viewModel.SelectedClient.Studs.ContainsKey(wallKey))
            {
                List<TimberBase> studs = null;
                List<TimberBase> selectedStuds = null;
                viewModel.SelectedClient.Studs.TryGetValue(wallKey, out studs);
                if (studs != null)
                {
                    var result = new List<string>();
                    selectedStuds = studs.FindAll(x => x.Thickness == wallThickness);
                    
                        result.AddRange(selectedStuds.Select(stud => stud.SizeGrade));
                        return result;
                }
            }

            return null;

        }
    }
}
