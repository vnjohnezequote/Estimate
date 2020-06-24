// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindCategory.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The beam.
    /// </summary>
    public class WindCategory : BindableBase
    {
        /// <summary>
        /// The wind rate.
        /// </summary>
        private string windRate;

        /// <summary>
        /// Gets the header.
        /// </summary>
        public string Header => "Design wind speed used";

        /// <summary>
        /// Gets or sets the wind rate.
        /// </summary>
        public string WindRate
        {
            get => this.windRate;
            set => this.SetProperty(ref this.windRate, value);
        }
    }
}
