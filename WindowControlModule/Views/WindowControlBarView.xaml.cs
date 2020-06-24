// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowControlBarView.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for WindowControlBarView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WindowControlModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;
 

    /// <summary>
    /// Interaction logic for WindowControlBarView.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class WindowControlBarView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControlBarView"/> class.
        /// </summary>
        public WindowControlBarView()
        {
            this.InitializeComponent();
        }
    }
}
