// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowControlBarViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WindowControlBarViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WindowControlModule.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;

    using CustomControls.Helper;

    using MaterialDesignThemes.Wpf;

    using Prism.Commands;
    using Prism.Mvvm;

    /// <summary>
    /// The window control bar view model.
    /// </summary>
    internal class WindowControlBarViewModel : BindableBase
    {
        #region private member

        /// <summary>
        /// maximize icon button
        /// </summary>
        private readonly PackIcon maximizeIcon = new PackIcon { Kind = PackIconKind.SquareOutline };

        /// <summary>
        /// normalize icon button
        /// </summary>
        private readonly PackIcon normalizeIcon = new PackIcon { Kind = PackIconKind.LayersOutline };

        /// <summary>
        /// the window of Control bar control
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private Window _window;

        /// <summary>
        /// Icon for maximize Button
        /// </summary>
        private PackIcon maximizeButtonIcon = new PackIcon() { Kind = PackIconKind.LayersOutline };

        /// <summary>
        /// The Window Title
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private string title;

        /// <summary>
        /// The _is saved.
        /// </summary>
        private bool isSaved = true;
        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControlBarViewModel"/> class. 
        /// Constructor of ControlBarViewModel
        /// </summary>
        public WindowControlBarViewModel()
        {
            // delegate Loaded
            this.LoadedCommand = new DelegateCommand<FrameworkElement>(this.ControlLoaded);

            // delegate close button
            this.CloseWindowCommand = new DelegateCommand(() =>
            {
                if (this.isSaved)
                {
                    this._window.Close();
                }
            });

            // delegate Maximize window button
            // ReSharper disable once ComplexConditionExpression
            this.MaximizeWindowCommand = new DelegateCommand(() =>
            {
                this._window.WindowState = this._window.WindowState != WindowState.Maximized
                                               ? WindowState.Maximized
                                               : WindowState.Normal;
            });

            // delegate Minimize window button
            this.MinimizeWindowCommand = new DelegateCommand(() =>
                {
                    this._window.WindowState = WindowState.Minimized;
                });
        }
        #endregion

        #region Public member

        /// <summary>
        /// Gets the maximize button icon.
        /// </summary>
        public PackIcon MaximizeButtonIcon
        {
            get => this.maximizeButtonIcon;
            private set => this.SetProperty(ref this.maximizeButtonIcon, value);
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get => this.title; set => this.SetProperty(ref this.title, value); }
        #endregion

        #region Command

        /// <summary>
        /// Gets the loaded command.
        /// </summary>
        public ICommand LoadedCommand { get; private set; }

        /// <summary>
        /// Gets the close window command.
        /// </summary>
        public ICommand CloseWindowCommand { get; private set; }

        /// <summary>
        /// Gets the maximize window command.
        /// </summary>
        public ICommand MaximizeWindowCommand { get; private set; }

        /// <summary>
        /// Gets the minimize window command.
        /// </summary>
        public ICommand MinimizeWindowCommand { get; private set; }
        #endregion
        #region Private Func

        /// <summary>
        /// Constructor for ControlBar when this loaded
        /// </summary>
        /// <param name="param">param should be a window element</param>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        private void ControlLoaded(FrameworkElement param)
        {
            var parrentWindow = WindowHelper.GetWindowParent(param);
            if (parrentWindow == null) return;
            if (parrentWindow is Window window)
            {
                this._window = window;
            }

            this.Title = this._window.Title;
            this.ChangedIcon();
            this._window.StateChanged += this.WindowStateChanged;
        }

        /// <summary>
        /// Change Icon for Maximize button
        /// </summary>
        private void ChangedIcon()
        {
            this.MaximizeButtonIcon = this._window.WindowState == WindowState.Maximized ? this.normalizeIcon : this.maximizeIcon;
        }

        /// <summary>
        /// Change icon for Maximize button when WindowState Changed
        /// </summary>
        /// <param name="sender"> sender is a window </param>
        /// <param name="e"> e store information of window </param>
        private void WindowStateChanged(object sender, System.EventArgs e)
        {
            this.ChangedIcon();
        }

        #endregion
    }
}
