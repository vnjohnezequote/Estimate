// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewJobWizardView.xaml.cs" company="John Nguyen">
// John Nguyen
// </copyright>
// <summary>
//   Interaction logic for NewJobView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NewJobWizardModule.Views
{
    using System.Diagnostics.CodeAnalysis;

    using CustomControls.Controls;

    /// <summary>
    /// Interaction logic for NewJobView.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class NewJobWizardView : FlatWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewJobWizardView"/> class.
        /// </summary>
        public NewJobWizardView()
        {
            this.InitializeComponent();
        }
    }
}
