// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallTempLength.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The wall temp length.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CsvHelper.Configuration.Attributes;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall temp length.
    /// </summary>
    public class WallTempLength : BindableBase
    {
        /// <summary>
        /// The id.
        /// </summary>
        private string _id;

        /// <summary>
        /// The count.
        /// </summary>
        private int _count;

        /// <summary>
        /// The wall length.
        /// </summary>
        private double _length;

        /// <summary>
        /// The area.
        /// </summary>
        private double _totalArea;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        [Name("Count")]
        public int Count
        {
            get => this._count;
            set => this.SetProperty(ref this._count, value);
        }
        
        /// <summary>
        /// Gets or sets the wall length.
        /// </summary>
        [Name("Total Length")]
        public double Length
        {
            get => this._length;
            set => this.SetProperty(ref this._length, value);
        }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        [Name("Total Area")]
        public double TotalArea
        {
            get => this._totalArea;
            set => this.SetProperty(ref this._totalArea, value);
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Name("ID")]
        public string Id
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
        }
    }
}
