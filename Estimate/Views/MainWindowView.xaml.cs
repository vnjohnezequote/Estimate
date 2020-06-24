// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowView.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for MainWindowView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estimate.Views
{
    using System.Diagnostics.CodeAnalysis;
    using CustomControls.Controls;

    using Prism.Regions;


    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class MainWindowView : FlatWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowView"/> class.
        /// </summary>
        public MainWindowView()
        {
            
            this.InitializeComponent();
        }

    }
}
