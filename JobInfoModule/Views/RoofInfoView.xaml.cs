// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoofInfoView.xaml.cs" company="John Nguyen">
// John Nguyen
// </copyright>
// <summary>
//   Interaction logic for RoofInfo.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RoofInfo.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class RoofInfoView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoofInfoView"/> class.
        /// </summary>
        public RoofInfoView()
        {
            this.InitializeComponent();
        }
    }
}
