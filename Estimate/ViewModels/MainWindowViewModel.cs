// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The main window view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DrawingModule.CommandClass;
using NewJobWizardModule.Views;

namespace Estimate.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using AppDataBase.DataBase;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using AppModels;

    using DataType.Class;

    using DrawingModule.Views;

    using Estimate.Views;

    using JetBrains.Annotations;

    using JobInfoModule.Views;

    using NewJobWizardModule;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    using WallFrameInputModule.Views;

    /// <summary>
    /// The main window view model.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// Minimum height size for Main Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private const int MainPanelMinHeight = 30;

        /// <summary>
        /// Minimum width size for Main Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private const int MainPanelMinWidth = 30;

        /// <summary>
        /// The window.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private MainWindowView window;

        /// <summary>
        /// Max with size for Quick Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int quickPanelMaxWidth;

        /// <summary>
        /// Max with size for Property Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int propertyPanelMaxWidth;

        /// <summary>
        /// Max Height size of bottom Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int bottomPanelMaxHeight;

        /// <summary>
        /// Width size of divider between Panels
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int splitterSize = 1;

        /// <summary>
        /// Width size of divider for Left Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int leftPanelSplitterWidth = 0;

        /// <summary>
        /// Width size of divider for Right Panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int rightPanelSplitterWidth = 0;

        /// <summary>
        /// Store Last Height for bottom panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int lastBottomHeight = 30;

        /// <summary>
        /// Store Last Height for bottom panel
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int lastPropertyWidth = 150;

        /// <summary>
        /// Decide Bottom panel opened or not
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private bool isBottomBarOpened = false;

        /// <summary>
        /// Decide Property Panel Opened or not
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private bool isPropertyBarOpened = false;

        /// <summary>
        /// The client name.
        /// </summary>
        private string clientName;

        /// <summary>
        /// The is job exist.
        /// </summary>
        private bool isJobExist = false;

        /// <summary>
        /// The job model.
        /// </summary>
        private JobModel jobModel;

        /// <summary>
        /// The client db.
        /// </summary>
        private ClientDataBase clientDB;

        /// <summary>
        /// The clients.
        /// </summary>
        private ObservableCollection<Client> clients;

        /// <summary>
        /// The _selected client.
        /// </summary>
        private Client selectedClient;

        /// <summary>
        /// The selected client index.
        /// </summary>
        private int? selectedClientIndex;

        /// <summary>
        /// The max panel changed.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private Action _maxPanelChanged;

        private List<CommandClass> commandClassList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">The unityContainer<see cref="IUnityContainer"/></param>
        /// <param name="regionManager">The regionManager<see cref="IRegionManager"/></param>
        /// <param name="eventAggregator">The eventAggregator<see cref="IEventAggregator"/></param>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1120:CommentsMustContainText", Justification = "Reviewed. Suppression is OK here.")]
        public MainWindowViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            commandClassList = new List<CommandClass>();

            

            this.clientDB = this.UnityContainer.Resolve<ClientDataBase>();
            this.GetClientCollection();
            this._maxPanelChanged = this.OnMaxWidthPanelChanged;
            this._maxPanelChanged += this.OnMaxHeightPanelChanged;
            this.WindowLoadedCommand = new DelegateCommand<MainWindowView>(this.WindowLoaded);
            this.WindowSizeChangedCommand = new DelegateCommand(this._maxPanelChanged);
            this.PanelSizeChangedCommand = new DelegateCommand<object>(this.OnPanelSizeChanged);
            this.OpenMenuCommand = new DelegateCommand(this.OnOpenMenu);
            this.CloseMenuCommand = new DelegateCommand(this.OnCloseMenu);
            this.ExpanderBottomPanelCommand = new DelegateCommand(this.OnExpanderBottomPanel);
            this.BottomPanelGridSplitterDragDeltaCommand = new DelegateCommand(this.OnBottomPanelSizeChanged);
            this.PropertyPanelGridSplitterDragDeltaCommand = new DelegateCommand(this.OnPropertyPanelSizeChanged);
            this.PropertyPanelGridSplitterDragCompleteCommand = new DelegateCommand(this.OnPropertyPanelGridSplitterDragComplete);
            this.BottomPanelGridSplitterDragCompleteCommand = new DelegateCommand(this.OnBottomPanelGridSplitterDragComplete);
            this.ExpanderPropertyPanelCommand = new DelegateCommand(this.OnExpanderPropertyPanel);
            this.LoadLogoButton();
            this.CustomerMenuSelectCommand = new DelegateCommand(this.OnCustomerMenuSelect);
            this.LoadDrawingWindowCommand = new DelegateCommand(this.LoadDrawingWindow);
            this.EventAggre.GetEvent<JobModelService>().Subscribe(this.JobModelReceive);


        }

        

        /// <summary>
        /// Gets the window loaded command.
        /// </summary>
        [NotNull]
        public ICommand WindowLoadedCommand { get; private set; }

        /// <summary>
        /// Gets the open menu command.
        /// </summary>
        [NotNull]
        public ICommand OpenMenuCommand { get; private set; }

        /// <summary>
        /// Gets the close menu command.
        /// </summary>
        [NotNull]
        public ICommand CloseMenuCommand { get; private set; }

        /// <summary>
        /// Gets the customer menu select command.
        /// </summary>
        [NotNull]
        public ICommand CustomerMenuSelectCommand { get; private set; }

        /// <summary>
        /// Gets the expander bottom panel command.
        /// </summary>
        public ICommand ExpanderBottomPanelCommand { get; private set; }

        /// <summary>
        /// Gets the window size changed command.
        /// </summary>
        public ICommand WindowSizeChangedCommand { get; private set; }

        /// <summary>
        /// Gets the panel size changed command.
        /// </summary>
        public ICommand PanelSizeChangedCommand { get; private set; }

        /// <summary>
        /// Gets the bottom panel grid splitter drag delta command.
        /// </summary>
        public ICommand BottomPanelGridSplitterDragDeltaCommand { get; private set; }

        /// <summary>
        /// Gets the property panel grid splitter drag delta command.
        /// </summary>
        public ICommand PropertyPanelGridSplitterDragDeltaCommand { get; private set; }

        /// <summary>
        /// Gets the bottom panel grid splitter drag complete command.
        /// </summary>
        public ICommand BottomPanelGridSplitterDragCompleteCommand { get; private set; }

        /// <summary>
        /// Gets the property panel grid splitter drag complete command.
        /// </summary>
        public ICommand PropertyPanelGridSplitterDragCompleteCommand { get; private set; }

        /// <summary>
        /// Gets the expander property panel command.
        /// </summary>
        public ICommand ExpanderPropertyPanelCommand { get; private set; }

        /// <summary>
        /// Gets the load drawing window command.
        /// </summary>
        public ICommand LoadDrawingWindowCommand { get; private set; }

        /// <summary>
        /// Gets or sets the quick panel max width.
        /// </summary>
        public int QuickPanelMaxWidth { get => this.quickPanelMaxWidth; set => this.SetProperty(ref this.quickPanelMaxWidth, value); }

        /// <summary>
        /// Gets the property panel max width.
        /// </summary>
        public int PropertyPanelMaxWidth { get => this.propertyPanelMaxWidth; private set => this.SetProperty(ref this.propertyPanelMaxWidth, value); }

        /// <summary>
        /// Gets the bottom panel max height.
        /// </summary>
        public int BottomPanelMaxHeight { get => this.bottomPanelMaxHeight; private set => this.SetProperty(ref this.bottomPanelMaxHeight, value); }

        /// <summary>
        /// Gets the splitter size.
        /// </summary>
        public int SplitterSize { get => this.splitterSize; private set => this.SetProperty(ref this.splitterSize, value); }

        /// <summary>
        /// Gets the left panel splitter width.
        /// </summary>
        public int LeftPanelSplitterWidth { get => this.leftPanelSplitterWidth; private set => this.SetProperty(ref this.leftPanelSplitterWidth, value); }

        /// <summary>
        /// Gets the right panel splitter width.
        /// </summary>
        public int RightPanelSplitterWidth { get => this.rightPanelSplitterWidth; private set => this.SetProperty(ref this.rightPanelSplitterWidth, value); }

        /// <summary>
        /// Gets the last bottom bar height.
        /// </summary>
        public int LastBottomBarHeight { get => this.lastBottomHeight; private set => this.SetProperty(ref this.lastBottomHeight, value); }

        /// <summary>
        /// Gets the last property panel width.
        /// </summary>
        public int LastPropertyPanelWidth { get => this.lastPropertyWidth; private set => this.SetProperty(ref this.lastPropertyWidth, value); }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public ObservableCollection<Client> Clients { get => this.clients; set => this.SetProperty(ref this.clients, value); }

        /// <summary>
        /// Gets or sets the selected client.
        /// </summary>
        public Client SelectedClient { get => this.selectedClient; set => this.SetProperty(ref this.selectedClient, value); }

        /// <summary>
        /// Gets or sets the selected client index.
        /// </summary>
        public int? SelectedClientIndex { get => this.selectedClientIndex; set => this.SetProperty(ref this.selectedClientIndex, value); }

        /// <summary>
        /// Gets or sets the Job
        /// </summary>
        public JobModel Job { get => this.jobModel; set => this.SetProperty(ref this.jobModel, value); }

        /// <summary>
        /// The load wall frame input.
        /// </summary>
        private void LoadWallFrameInput()
        {
            var parameters =
                new NavigationParameters
                    {
                        { "Job", this.Job },
                        { "SelectedClient", this.SelectedClient }
                    };
            if (this.Job != null)
            {
                this.RegionManager.RequestNavigate(MainWindowRegions.MainRegion, nameof(WallFrameInputView), parameters);
            }
        }

        /// <summary>
        /// The get client collection.
        /// </summary>
        private void GetClientCollection()
        {
            this.Clients = new ObservableCollection<Client>(this.clientDB.GetClients());
        }

        /// <summary>
        /// The job model receive.
        /// </summary>
        /// <param name="job">The job<see cref="JobModel"/></param>
        private void JobModelReceive(JobModel job)
        {
            this.Job = job;
            this.OnSetSelectedClient(this.Job.Info.ClientName);
            this.LoadJobInfor();
            this.LoadWallFrameInput();
        }

        /// <summary>
        /// The load drawing window.
        /// </summary>
        private void LoadDrawingWindow()
        {
            //var drawingWindow = new DrawingWindowView();
            var drawingWindow = this.UnityContainer.Resolve<BaseWindowService>();
            //shell.ShowShell<NewJobWizardView>();
            this.EventAggre.GetEvent<JobModelService>().Publish(this.Job);
            drawingWindow.ShowShell<DrawingWindowView>();
            //drawingWindow.Show();
        }

        /// <summary>
        /// The on set selected client.
        /// </summary>
        /// <param name="customer">The customer<see cref="string"/></param>
        private void OnSetSelectedClient(string customer)
        {
            var selectedCustomer = this.clientDB.GetClient(customer);

            if (selectedCustomer == null)
            {
                return;
            }

            this.SelectedClient = selectedCustomer;
            this.SelectedClientIndex = this.SelectedClient.Id - 1;
        }

        /// <summary>
        /// The window loaded.
        /// </summary>
        /// <param name="window">The window<see cref="MainWindowView"/></param>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        private void WindowLoaded([NotNull] MainWindowView window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            this.window = window ?? throw new ArgumentNullException(nameof(window));
            this._maxPanelChanged.Invoke();

            if (this.isJobExist == false)
            {
                this.CreateJobWizard();
            }
        }

        /// <summary>
        /// The load job infor.
        /// </summary>
        private void LoadJobInfor()
        {
            var parameters = new NavigationParameters { { "Job", Job } };
            if (this.Job != null)
            {
                this.RegionManager.RequestNavigate(MainWindowRegions.RightContentRegion, nameof(JobInfomationView), parameters);
            }
        }

        /// <summary>
        /// The create job wizard.
        /// </summary>
        private void CreateJobWizard()
        {
            var shell = this.UnityContainer.Resolve<BaseWindowService>();
            shell.ShowShell<NewJobWizardView>();
        }

        /// <summary>
        /// Expander Property Panel
        /// </summary>
        private void OnExpanderPropertyPanel()
        {
            if (this.isPropertyBarOpened == false)
            {
                this.isPropertyBarOpened = true;
                var openProperty = this.window.FindResource("OpenPropertyPanel") as Storyboard;
                this.window.ButtonOpenPropertyPanel.Visibility = Visibility.Collapsed;
                this.window.ButtonClosePropertyPanel.Visibility = Visibility.Visible;
                openProperty?.Begin();
            }
            else
            {
                this.isPropertyBarOpened = false;
                var closeProperty = this.window.FindResource("ClosePropertyPanel") as Storyboard;
                this.window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
                this.window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
                closeProperty?.Begin();
            }
        }

        /// <summary>
        /// Expander Bottom Panel
        /// </summary>
        private void OnExpanderBottomPanel()
        {

            if (this.isBottomBarOpened == false)
            {
                this.isBottomBarOpened = true;
                var openBottom = this.window.FindResource("OpenCalc") as Storyboard;
                this.window.ButtonOpenBottomPanel.Visibility = Visibility.Collapsed;
                openBottom?.Begin();
            }
            else
            {
                this.isBottomBarOpened = false;
                var closeBottom = this.window.FindResource("CloseCalc") as Storyboard;
                this.window.ButtonOpenBottomPanel.Visibility = Visibility.Visible;
                closeBottom?.Begin();
            }
        }

        /// <summary>
        /// The on panel size changed.
        /// </summary>
        /// <param name="objectChanged">The objectChanged<see cref="object"/></param>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        private void OnPanelSizeChanged(object objectChanged)
        {
            if (!(objectChanged is FrameworkElement frameWorkElement)) return;
            switch (frameWorkElement.Name)
            {
                case "QuickBar":
                    this.LeftPanelSplitterWidth = (int)this.window.QuickPanel.ActualWidth == 0 ? 0 : 1;
                    break;
                case "PropertyBar":
                    this.RightPanelSplitterWidth = (int)this.window.PropertyPanel.ActualWidth == 0 ? 0 : 1;
                    break;
                case "BottomBar":
                    break;
                default:
                    break;
            }

            this.OnMaxWidthPanelChanged();
        }

        /// <summary>
        /// Control Panel When Grid Splitter drag Complete
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void OnBottomPanelGridSplitterDragComplete()
        {
            if ((int)this.window.BottomBarPanel.ActualHeight <= 0
                || (int)this.window.BottomBarPanel.ActualHeight >= 30) return;
            this.isBottomBarOpened = false;
            this.window.ButtonOpenBottomPanel.Visibility = Visibility.Visible;
            this.window.BottomBarPanel.Height = new GridLength(0);
            this.LastBottomBarHeight = 30;
        }

        /// <summary>
        /// The on property panel grid splitter drag complete.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        private void OnPropertyPanelGridSplitterDragComplete()
        {
            if ((int)this.window.PropertyPanel.ActualWidth <= 0
                || (int)this.window.PropertyPanel.ActualWidth >= 150) return;
            this.isPropertyBarOpened = false;
            this.window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
            this.window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
            this.window.PropertyPanel.Width = new GridLength(0);
            this.LastPropertyPanelWidth = 150;
        }

        /// <summary>
        /// Control Bottom Panel when Grid splitter Drag delta event
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void OnBottomPanelSizeChanged()
        {
            // hide or Unhide Bottom Panel Button
            // _window.ButtonOpenBottomPanel.Visibility = _window.BottomBarPanel.ActualHeight > 0 ? Visibility.Collapsed : Visibility.Visible;
            if ((int)this.window.BottomBarPanel.ActualHeight > 0)
            {
                this.isBottomBarOpened = true;
                this.window.ButtonOpenBottomPanel.Visibility = Visibility.Collapsed;
                this.LastBottomBarHeight = (int)this.window.BottomBarPanel.ActualHeight;
            }
            else
            {
                this.isBottomBarOpened = false;
                this.window.ButtonOpenBottomPanel.Visibility = Visibility.Visible;
                this.LastBottomBarHeight = 30;
            }
        }

        /// <summary>
        /// The on property panel size changed.
        /// </summary>
        private void OnPropertyPanelSizeChanged()
        {
            if ((int)this.window.PropertyPanel.ActualWidth > 0)
            {
                this.isPropertyBarOpened = true;
                this.window.ButtonOpenPropertyPanel.Visibility = Visibility.Collapsed;
                this.window.ButtonClosePropertyPanel.Visibility = Visibility.Visible;
                this.LastPropertyPanelWidth = (int)this.window.PropertyPanel.ActualWidth;
            }
            else
            {
                this.isPropertyBarOpened = false;
                this.window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
                this.window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
                this.LastPropertyPanelWidth = 150;
            }
        }

        /// <summary>
        /// Control Open Menu
        /// </summary>
        private void OnOpenMenu()
        {
            this.window.ButtonCloseMenu.Visibility = Visibility.Visible;
            this.window.ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Control Close Menu
        /// </summary>
        private void OnCloseMenu()
        {
            this.window.ButtonCloseMenu.Visibility = Visibility.Collapsed;
            this.window.ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Change maximum width of Panel
        /// Fixed issue with column definition
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void OnMaxWidthPanelChanged()
        {
            this.QuickPanelMaxWidth = (int)(this.window.ActualWidth
                                            - (this.window.LeftPanel.ActualWidth + this.window.RightPanel.ActualWidth
                                                                                 + this.window.PropertyPanel.ActualWidth
                                                                                 + 2 * this.SplitterSize
                                                                                 + this.LeftPanelSplitterWidth
                                                                                 + this.RightPanelSplitterWidth
                                                                                 + MainPanelMinWidth));
            this.PropertyPanelMaxWidth = (int)(this.window.ActualWidth
                                               - (this.window.LeftPanel.ActualWidth + this.window.RightPanel.ActualWidth
                                                                                    + this.window.QuickPanel.ActualWidth
                                                                                    + 2 * this.SplitterSize
                                                                                    + this.LeftPanelSplitterWidth
                                                                                    + this.RightPanelSplitterWidth
                                                                                    + MainPanelMinWidth));
        }

        /// <summary>
        /// Changed Maximum Height of Bottom Panel
        /// Fixed issue with Row definition
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void OnMaxHeightPanelChanged()
        {
            this.BottomPanelMaxHeight = (int)(this.window.ActualHeight
                                              - (this.window.TitlePanel.ActualHeight
                                                 + this.window.ToolBarPanel.ActualHeight + 2 * this.SplitterSize
                                                 + MainPanelMinHeight));
        }

        /// <summary>
        /// The load region.
        /// </summary>
        private void LoadLogoButton()
        {
            this.RegionManager.RegisterViewWithRegion("LogoButtonRegion", typeof(LogoMenuView));
        }

        /// <summary>
        /// The Select Customer Function
        /// </summary>
        private void OnCustomerMenuSelect()
        {
            if (this.SelectedClient is null)
            {
                return;
            }
            this.Job.Info.ClientName = this.SelectedClient.Name;
            this.EventAggre.GetEvent<CustomerService>().Publish(this.SelectedClient);
        }

        
        
    }
}
