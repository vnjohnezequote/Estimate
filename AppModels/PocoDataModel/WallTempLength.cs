// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallTempLength.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The wall temp length.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels.PocoDataModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CsvHelper.Configuration.Attributes;

    using Prism.Mvvm;

    /// <summary>
    /// The wall temp length.
    /// </summary>
    public class WallTempLength : BindableBase
    {
        /// <summary>
        /// The id.
        /// </summary>
        private string id;

        /// <summary>
        /// The count.
        /// </summary>
        private int count;

        /// <summary>
        /// The wall length.
        /// </summary>
        private double length;

        /// <summary>
        /// The area.
        /// </summary>
        private double totalArea;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        [Name("Count")]
        public int Count
        {
            get => this.count;
            set => this.SetProperty(ref this.count, value);
        }
        
        /// <summary>
        /// Gets or sets the wall length.
        /// </summary>
        [Name("Total Length")]
        public double Length
        {
            get => this.length;
            set => this.SetProperty(ref this.length, value);
        }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        [Name("Total Area")]
        public double TotalArea
        {
            get => this.totalArea;
            set => this.SetProperty(ref this.totalArea, value);
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Name("ID")]
        public string Id
        {
            get => this.id;
            set => this.SetProperty(ref this.id, value);
        }
    }
}
