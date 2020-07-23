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
    public class BottomPlatesSelectConverter : IItemsSourceSelector
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
            //if (record == null)
            //{
            //    return null;
            //}

            //var wallInfor = record as WallLayer;
            //var wallThickness = wallInfor.WallThickness.Size;

            //var viewModel = dataContext as PrenailFloorInputViewModel;

            ////Returns ShipCity collection based on ShipCountry.

            //if (viewModel != null && viewModel.SelectedClient.BottomPlates.ContainsKey(wallThickness.ToString()))
            //{
            //    List<TimberBase> bottomPlates = null;
            //    viewModel.SelectedClient.BottomPlates.TryGetValue(wallThickness.ToString(), out bottomPlates);
            //    return (bottomPlates ?? throw new InvalidOperationException()).ToList();
            //}
            return null;
        }
    }
}
