// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelNote.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Prism.Mvvm;

    /// <summary>
    /// The beam.
    /// </summary>
    public class ExcelNote : BindableBase
    {
        /// <summary>
        /// Gets or sets the wind rate.
        /// </summary>
        public WindCategory WindRate {get;set;}

        /// <summary>
        /// Gets or sets the design infors.
        /// </summary>
        public List<DesignInfor> DesignInfors {get;set;}

        /// <summary>
        /// Gets or sets the custom notes.
        /// </summary>
        public ObservableCollection <string> CustomNotes {get;set;}

        /// <summary>
        /// The default note.
        /// </summary>
        public string DefaultNote => "Please check and confirm.";
    }
}
