// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RLWCalc.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.PocoDataModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The beam.
    /// </summary>
    public class RlwCalc : BindableBase
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the roof pitch info.
        /// </summary>
        public JobWallDefaultInfo RoofPitchInfo { get; set; }

        /// <summary>
        /// Gets or sets the roof span.
        /// </summary>
        public int RoofSpan { get; set; }

        /// <summary>
        /// Gets or sets the over hang.
        /// </summary>
        public int OverHang { get; set; }

        /// <summary>
        /// Gets or sets the roof type.
        /// </summary>
        public string RoofType { get; set; }

        /// <summary>
        /// Gets or sets the selected clientPoco.
        /// </summary>
        public ClientPoco SelectedClient { get; set; }

        /// <summary>
        /// Gets or sets the roof load width.
        /// </summary>
        public int RoofLoadWidth { get; set; }
    }
}
