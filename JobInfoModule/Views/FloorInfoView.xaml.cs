// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FloorInfoView.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for FloorInfoView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;
    
    /// <summary>
    /// Interaction logic for FloorInfoView.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class FloorInfoView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloorInfoView"/> class.
        /// </summary>
        public FloorInfoView()
        {
            this.InitializeComponent();
        }

        private void WallHeightTextBox(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!string.IsNullOrEmpty(textBox.Text)) return;
                textBox.Text = "0";
                textBox.SelectAll();

            }

            if (sender is ComboBox comboBox)
            {
                if (!string.IsNullOrEmpty(comboBox.Text)) return;
                comboBox.Text = "0";
            }
            
        }
    }
}
