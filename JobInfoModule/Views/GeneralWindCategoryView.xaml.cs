// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindCategory.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for WindCategory.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WindCategory.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class GeneralWindCategoryView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWindCategoryView"/> class.
        /// </summary>
        public GeneralWindCategoryView()
        {
            this.InitializeComponent();
        }
    }
}
