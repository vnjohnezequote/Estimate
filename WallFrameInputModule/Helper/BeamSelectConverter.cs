// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamSelectConverter.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the BeamSelectConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;

namespace WallFrameInputModule.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AppModels;

    using Syncfusion.UI.Xaml.Grid;

    using WallFrameInputModule.ViewModels;

    /// <summary>
    /// The beam select converter.
    /// </summary>
    public class BeamSelectConverter : IItemsSourceSelector
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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            if (record == null)
            {
                return null;
            }

            var beam = record as Beam;
            var beamGrade = beam.BeamGrade;
            if (beamGrade == null)
            {
                return null;
            }
            var viewModel = dataContext as PrenailFloorInputViewModel;
            

            if (viewModel != null && viewModel.SelectedClient.Beams.ContainsKey(beamGrade.ToString()))
            {
                List<TimberBase> beams = null;
                viewModel.SelectedClient.Beams.TryGetValue(beamGrade, out beams);
                return (beams ?? throw new InvalidOperationException()).ToList();
            }
            return null;
        }
    }
}
