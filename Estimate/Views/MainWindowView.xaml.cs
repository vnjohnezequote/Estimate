// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowView.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for MainWindowView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Windows;
using System.Windows.Input;
using Estimate.ViewModels;

namespace Estimate.Views
{
    using System.Diagnostics.CodeAnalysis;
    using CustomControls.Controls;

    using Prism.Regions;


    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class MainWindowView : Window
    {
        private MainWindowViewModel _viewModel;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowView"/> class.
        /// </summary>
        public MainWindowView()
        {
            
            this.InitializeComponent();
            
            if (this.DataContext!=null && this.DataContext is MainWindowViewModel viewModel)
            {
                _viewModel = viewModel;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.S)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    _viewModel.SaveJob();
            }
        }
    }
}
