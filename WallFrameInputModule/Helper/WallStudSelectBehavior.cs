// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallStudSelectBehavior.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The wall stud select converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WallFrameInputModule.Helper
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Xaml.Behaviors;

    using Syncfusion.UI.Xaml.Grid;

    /// <summary>
    /// The wall stud select converter.
    /// </summary>
    public class WallStudSelectBehavior : Behavior<SfDataGrid>
    {
        /// <summary>
        /// The data grid.
        /// </summary>
        private SfDataGrid dataGrid = null;

        /// <summary>
        /// The on attached.
        /// </summary>
        protected override void OnAttached()
        {
            this.dataGrid = this.AssociatedObject as SfDataGrid;
            this.dataGrid.CurrentCellEndEdit += this.DataGridCurrentCellEndEdit;
        }

        /// <summary>
        /// The on detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            this.dataGrid.CurrentCellEndEdit -= this.DataGridCurrentCellEndEdit;
        }

        /// <summary>
        /// The data grid_ current cell end edit.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="CurrentCellEndEditEventArgs"/></param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private void DataGridCurrentCellEndEdit(object sender, CurrentCellEndEditEventArgs e)
        {
        }
    }
}
