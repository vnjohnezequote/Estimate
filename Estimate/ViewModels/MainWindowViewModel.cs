// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The main window view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
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
    using DataType.Class;

    using DrawingModule.Views;

    using Views;

    using JetBrains.Annotations;

    using JobInfoModule.Views;
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
        #region Field

        

        #endregion
        /// <summary>
        /// Minimum height size for Main Panel
        /// </summary>
        private const int MainPanelMinHeight = 30;

        /// <summary>
        /// Minimum width size for Main Panel
        /// </summary>
        private const int MainPanelMinWidth = 30;

        /// <summary>
        /// The window.
        /// </summary>
        private MainWindowView _window;

        /// <summary>
        /// Max with size for Quick Panel
        /// </summary>
        private int _quickPanelMaxWidth;

        /// <summary>
        /// Max with size for Property Panel
        /// </summary>
        private int _propertyPanelMaxWidth;

        /// <summary>
        /// Max Height size of bottom Panel
        /// </summary>
        private int _bottomPanelMaxHeight;

        /// <summary>
        /// Width size of divider between Panels
        /// </summary>
        private int _splitterSize = 1;

        /// <summary>
        /// Width size of divider for Left Panel
        /// </summary>
        private int _leftPanelSplitterWidth = 0;

        /// <summary>
        /// Width size of divider for Right Panel
        /// </summary>
        private int _rightPanelSplitterWidth = 0;

        /// <summary>
        /// Store Last Height for bottom panel
        /// </summary>
        private int _lastBottomHeight = 30;

        /// <summary>
        /// Store Last Height for bottom panel
        /// </summary>
        private int _lastPropertyWidth = 150;

        /// <summary>
        /// Decide Bottom panel opened or not
        /// </summary>
        private bool _isBottomBarOpened = false;

        /// <summary>
        /// Decide Property Panel Opened or not
        /// </summary>
        private bool _isPropertyBarOpened = false;

        /// <summary>
        /// The clientPoco name.
        /// </summary>
        private string _clientName;

        /// <summary>
        /// The is job exist.
        /// </summary>
        private bool _isJobExist = false;

        /// <summary>
        /// The clientPoco db.
        /// </summary>
        private ClientDataBase _clientDb;

        /// <summary>
        /// The clients.
        /// </summary>
        private ObservableCollection<ClientPoco> _clients;

        /// <summary>
        /// The _selected clientPoco.
        /// </summary>
        private ClientPoco _selectedClient;

        /// <summary>
        /// The selected clientPoco index.
        /// </summary>
        private int? _selectedClientIndex;

        /// <summary>
        /// The max panel changed.
        /// </summary>
        private readonly Action _maxPanelChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            //this._clientName = clientName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">The unityContainer<see cref="IUnityContainer"/></param>
        /// <param name="regionManager">The regionManager<see cref="IRegionManager"/></param>
        /// <param name="eventAggregator">The eventAggregator<see cref="IEventAggregator"/></param>
        /// <param name="layerManager"></param>
        public MainWindowViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator,
            ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            //this._clientName = clientName;
            this._clientDb = this.UnityContainer.Resolve<ClientDataBase>();
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
            SaveJobCommand = new DelegateCommand(OnSaveJobCommand);
            JobModel.Info.PropertyChanged += Info_PropertyChanged;
            //this.EventAggregator.GetEvent<JobModelService>().Subscribe(this.OnChangeClient);


        }

        private void Info_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName== "ClientName")
            {
                this.OnChangeClient(JobModel.Info.ClientName);
            }
        }

        #region Property

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

        public ICommand SaveJobCommand { get; private set; }
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
        public int QuickPanelMaxWidth { get => this._quickPanelMaxWidth; set => this.SetProperty(ref this._quickPanelMaxWidth, value); }

        /// <summary>
        /// Gets the property panel max width.
        /// </summary>
        public int PropertyPanelMaxWidth { get => this._propertyPanelMaxWidth; private set => this.SetProperty(ref this._propertyPanelMaxWidth, value); }

        /// <summary>
        /// Gets the bottom panel max height.
        /// </summary>
        public int BottomPanelMaxHeight { get => this._bottomPanelMaxHeight; private set => this.SetProperty(ref this._bottomPanelMaxHeight, value); }

        /// <summary>
        /// Gets the splitter size.
        /// </summary>
        public int SplitterSize { get => this._splitterSize; private set => this.SetProperty(ref this._splitterSize, value); }

        /// <summary>
        /// Gets the left panel splitter width.
        /// </summary>
        public int LeftPanelSplitterWidth { get => this._leftPanelSplitterWidth; private set => this.SetProperty(ref this._leftPanelSplitterWidth, value); }

        /// <summary>
        /// Gets the right panel splitter width.
        /// </summary>
        public int RightPanelSplitterWidth { get => this._rightPanelSplitterWidth; private set => this.SetProperty(ref this._rightPanelSplitterWidth, value); }

        /// <summary>
        /// Gets the last bottom bar height.
        /// </summary>
        public int LastBottomBarHeight { get => this._lastBottomHeight; private set => this.SetProperty(ref this._lastBottomHeight, value); }

        /// <summary>
        /// Gets the last property panel width.
        /// </summary>
        public int LastPropertyPanelWidth { get => this._lastPropertyWidth; private set => this.SetProperty(ref this._lastPropertyWidth, value); }

        /// <summary>
        /// Gets or sets the clientPoco name.
        /// </summary>
        public ObservableCollection<ClientPoco> Clients { get => this._clients; set => this.SetProperty(ref this._clients, value); }

        /// <summary>
        /// Gets or sets the selected clientPoco.
        /// </summary>
        public ClientPoco SelectedClient { get => this._selectedClient; set => this.SetProperty(ref this._selectedClient, value); }

        /// <summary>
        /// Gets or sets the selected clientPoco index.
        /// </summary>
        public int? SelectedClientIndex { get => this._selectedClientIndex; set => this.SetProperty(ref this._selectedClientIndex, value); }

        /// <summary>
        /// Gets or sets the JobDefaultInfo
        /// </summary>
        #endregion

        private void OnSaveJobCommand()
        {
            if (this.JobModel != null)
            {
                var fileName = JobModel.Info.JobLocation + @"\" + JobModel.Info.JobNumber + ".db";

                
            }
        }
        /// <summary>
        /// The load wall frame input.
        /// </summary>
        private void LoadWallFrameInput()
        {
            var parameters =
                new NavigationParameters
                    {
                        { "JobDefaultInfo", this.JobModel },
                        { "SelectedClient", this.SelectedClient }
                    };
            if (this.JobModel != null)
            {
                this.RegionManager.RequestNavigate(MainWindowRegions.MainRegion, nameof(WallFrameInputView), parameters);
            }
        }

        /// <summary>
        /// The get clientPoco collection.
        /// </summary>
        private void GetClientCollection()
        {
            this.Clients = new ObservableCollection<ClientPoco>(this._clientDb.Clients);
        }

        /// <summary>
        /// The job model receive.
        /// </summary>
        /// <param name="job">The job<see cref="JobModel"/></param>
        private void OnChangeClient(string clientName)
        {
            this.OnSetSelectedClient(clientName);
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
            drawingWindow.ShowShell<DrawingWindowView>(false);
            //drawingWindow.Show();
        }

        /// <summary>
        /// The on set selected clientPoco.
        /// </summary>
        /// <param name="customer">The customer<see cref="string"/></param>
        private void OnSetSelectedClient(string customer)
        {
            var selectedCustomer = this._clientDb.GetClient(customer);

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
        private void WindowLoaded([NotNull] MainWindowView window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            this._window = window ?? throw new ArgumentNullException(nameof(window));
            this._maxPanelChanged.Invoke();

            if (this._isJobExist == false)
            {
                this.CreateJobWizard();
            }
        }

        /// <summary>
        /// The load job infor.
        /// </summary>
        private void LoadJobInfor()
        {
            var parameters = new NavigationParameters {{ "JobModel",JobModel  }};
            if (JobModel!=null)
            {
                this.RegionManager.RequestNavigate(MainWindowRegions.RightContentRegion, nameof(JobInfomationView));
            }
        }

        /// <summary>
        /// The create job wizard.
        /// </summary>
        private void CreateJobWizard()
        {
            var shell = this.UnityContainer.Resolve<BaseWindowService>();
            shell.ShowShell<NewJobWizardView>(true);
            EventAggregator.GetEvent<RefreshFloorEvent>().Publish(true);


        }

        /// <summary>
        /// Expander Property Panel
        /// </summary>
        private void OnExpanderPropertyPanel()
        {
            if (this._isPropertyBarOpened == false)
            {
                this._isPropertyBarOpened = true;
                var openProperty = this._window.FindResource("OpenPropertyPanel") as Storyboard;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Collapsed;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Visible;
                openProperty?.Begin();
            }
            else
            {
                this._isPropertyBarOpened = false;
                var closeProperty = this._window.FindResource("ClosePropertyPanel") as Storyboard;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
                closeProperty?.Begin();
            }
        }

        /// <summary>
        /// Expander Bottom Panel
        /// </summary>
        private void OnExpanderBottomPanel()
        {

            if (this._isBottomBarOpened == false)
            {
                this._isBottomBarOpened = true;
                var openBottom = this._window.FindResource("OpenCalc") as Storyboard;
                this._window.ButtonOpenBottomPanel.Visibility = Visibility.Collapsed;
                openBottom?.Begin();
            }
            else
            {
                this._isBottomBarOpened = false;
                var closeBottom = this._window.FindResource("CloseCalc") as Storyboard;
                this._window.ButtonOpenBottomPanel.Visibility = Visibility.Visible;
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
                    this.LeftPanelSplitterWidth = (int)this._window.QuickPanel.ActualWidth == 0 ? 0 : 1;
                    break;
                case "PropertyBar":
                    this.RightPanelSplitterWidth = (int)this._window.PropertyPanel.ActualWidth == 0 ? 0 : 1;
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
            if ((int)this._window.BottomBarPanel.ActualHeight <= 0
                || (int)this._window.BottomBarPanel.ActualHeight >= 30) return;
            this._isBottomBarOpened = false;
            this._window.ButtonOpenBottomPanel.Visibility = Visibility.Visible;
            this._window.BottomBarPanel.Height = new GridLength(0);
            this.LastBottomBarHeight = 30;
        }

        /// <summary>
        /// The on property panel grid splitter drag complete.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        private void OnPropertyPanelGridSplitterDragComplete()
        {
            if ((int)this._window.PropertyPanel.ActualWidth <= 0
                || (int)this._window.PropertyPanel.ActualWidth >= 150) return;
            this._isPropertyBarOpened = false;
            this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
            this._window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
            this._window.PropertyPanel.Width = new GridLength(0);
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
            if ((int)this._window.BottomBarPanel.ActualHeight > 0)
            {
                this._isBottomBarOpened = true;
                this._window.ButtonOpenBottomPanel.Visibility = Visibility.Collapsed;
                this.LastBottomBarHeight = (int)this._window.BottomBarPanel.ActualHeight;
            }
            else
            {
                this._isBottomBarOpened = false;
                this._window.ButtonOpenBottomPanel.Visibility = Visibility.Visible;
                this.LastBottomBarHeight = 30;
            }
        }

        /// <summary>
        /// The on property panel size changed.
        /// </summary>
        private void OnPropertyPanelSizeChanged()
        {
            if ((int)this._window.PropertyPanel.ActualWidth > 0)
            {
                this._isPropertyBarOpened = true;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Collapsed;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Visible;
                this.LastPropertyPanelWidth = (int)this._window.PropertyPanel.ActualWidth;
            }
            else
            {
                this._isPropertyBarOpened = false;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
                this.LastPropertyPanelWidth = 150;
            }
        }

        /// <summary>
        /// Control Open Menu
        /// </summary>
        private void OnOpenMenu()
        {
            this._window.ButtonCloseMenu.Visibility = Visibility.Visible;
            this._window.ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Control Close Menu
        /// </summary>
        private void OnCloseMenu()
        {
            this._window.ButtonCloseMenu.Visibility = Visibility.Collapsed;
            this._window.ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Change maximum width of Panel
        /// Fixed issue with column definition
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void OnMaxWidthPanelChanged()
        {
            this.QuickPanelMaxWidth = (int)(this._window.ActualWidth
                                            - (this._window.LeftPanel.ActualWidth + this._window.RightPanel.ActualWidth
                                                                                 + this._window.PropertyPanel.ActualWidth
                                                                                 + 2 * this.SplitterSize
                                                                                 + this.LeftPanelSplitterWidth
                                                                                 + this.RightPanelSplitterWidth
                                                                                 + MainPanelMinWidth));
            this.PropertyPanelMaxWidth = (int)(this._window.ActualWidth
                                               - (this._window.LeftPanel.ActualWidth + this._window.RightPanel.ActualWidth
                                                                                    + this._window.QuickPanel.ActualWidth
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
            this.BottomPanelMaxHeight = (int)(this._window.ActualHeight
                                              - (this._window.TitlePanel.ActualHeight
                                                 + this._window.ToolBarPanel.ActualHeight + 2 * this.SplitterSize
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
            this.JobModel.Info.ClientName = this.SelectedClient.Name;
            this.EventAggregator.GetEvent<CustomerService>().Publish(this.SelectedClient);
        }

        
        
    }
}
