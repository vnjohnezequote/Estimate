// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HorizontalJobInfoView.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for JobDefaultInfo.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for JobDefaultInfo.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class HorizontalJobInfoView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalJobInfoView"/> class.
        /// </summary>
        public HorizontalJobInfoView()
        {
            this.InitializeComponent();
        }
    }
}
