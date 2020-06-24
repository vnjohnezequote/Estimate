// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignInfoView.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for DesignInfoView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DesignInfoView.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class DesignInfoView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignInfoView"/> class.
        /// </summary>
        public DesignInfoView()
        {
            this.InitializeComponent();
        }
    }
}
