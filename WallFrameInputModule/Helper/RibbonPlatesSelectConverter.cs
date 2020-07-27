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
    public class RibbonPlatesSelectConverter : IItemsSourceSelector
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
            if (wallInfor==null)
            {
                return null;
            }
            var wallThickness = wallInfor.WallThickness;

            ////Returns ShipCity collection based on ShipCountry.

            if (!(dataContext is PrenailFloorInputViewModel viewModel) ||
                !viewModel.SelectedClient.RibbonPlates.ContainsKey(wallThickness.ToString())) return null;
            viewModel.SelectedClient.RibbonPlates.TryGetValue(wallThickness.ToString(), out var ribbonPlates);
            var result = new List<string>();
            if (ribbonPlates!=null)
            {
                result.AddRange(ribbonPlates.Select(ribbonPlate => ribbonPlate.SizeGrade));
            }

            return result;
            //return (ribbonPlates ?? throw new InvalidOperationException()).ToList();
        }
    }
}
