﻿// --------------------------------------------------------------------------------------------------------------------
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
    public class TopPlatesSelectConverter : IItemsSourceSelector
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

            var wallInfor = record as PrenailWallLayer;
            if (wallInfor == null)
            {
                return null;
            }
            var wallThickness = wallInfor.WallThickness;

            ////Returns ShipCity collection based on ShipCountry.

            if (!(dataContext is PrenailFloorInputViewModel viewModel) ||
                !viewModel.SelectedClient.TopPlates.ContainsKey(wallThickness.ToString())) return null;
            viewModel.SelectedClient.TopPlates.TryGetValue(wallThickness.ToString(), out var topPlates);
            var result = new List<string>();
            if (topPlates != null)
            {
                result.AddRange(topPlates.Select(topPlate => topPlate.SizeGrade));
            }

            return result;
        }
    }
}
