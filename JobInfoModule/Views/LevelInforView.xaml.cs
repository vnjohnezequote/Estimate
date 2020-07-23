using System.Windows.Input;
using JobInfoModule.ViewModels;

namespace JobInfoModule.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class LevelInfoView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfoView"/> class.
        /// </summary>
        public LevelInfoView()
        {
            this.InitializeComponent();
        }

        private void NumericInputChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!string.IsNullOrEmpty(textBox.Text)) return;
                textBox.Text = "0";
                textBox.SelectAll();
                if (textBox.Name != "HackNonLbwCmb") return;
                if (string.IsNullOrEmpty(this.NonLbwCmb.Text))
                {
                    NonLbwCmb.Text = "0";
                }

            }

       
        }

        
    }
}
