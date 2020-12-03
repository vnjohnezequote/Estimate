// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindCategory.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The beam.
    /// </summary>
    public class WindCategory : BindableBase
    {
        /// <summary>
        /// The wind rate.
        /// </summary>
        private string _windRate;

        /// <summary>
        /// Gets the header.
        /// </summary>
        public string Header => "Design wind speed used";

        /// <summary>
        /// Gets or sets the wind rate.
        /// </summary>
        public string WindRate
        {
            get => this._windRate;
            set => this.SetProperty(ref this._windRate, value);
        }
    }
}
