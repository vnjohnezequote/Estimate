// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewJobWizardViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the NewJobWizardViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using ApplicationInterfaceCore;
using AppModels;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using NewJobWizardModule.Helper;

namespace NewJobWizardModule.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using CustomControls.Controls;
    using CustomControls.Helper;

    using DataType.Class;

    using JobInfoModule.Views;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The new job wizard view model.
    /// </summary>
    public class NewJobWizardViewModel : BaseViewModel
    {
        #region Private member

        /// <summary>
        /// The is saved.
        /// </summary>
        //private bool isSaved = true;

        /// <summary>
        /// The selected index tab.
        /// </summary>
        private int _selectedIndexTab;

        /// <summary>
        /// The _window.
        /// </summary>
        private FlatWindow _window;

        /// <summary>
        /// The is design visibility.
        /// </summary>
        private Visibility _isDesignVisibility = Visibility.Collapsed;

        private Visibility _isStickFrameVisibility = Visibility.Collapsed;

        /// <summary>
        /// The job.
        /// </summary>
        private IJob _job;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJobWizardViewModel"/> class.
        /// </summary>
        public NewJobWizardViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJobWizardViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        public NewJobWizardViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator,ILayerManager layerManger,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManger,jobModel)
        {
            this.JobModel.Info.JobNumber = "2220";
            this.JobModel.Info.RoofPitch = 22.5;
            this.JobModel.Info.StepDown = 170;
            this.JobModel.Info.NoggingMethod = NoggingMethodType.AsWall;
            this.JobModel.Info.RoofOverHang = 600;

            this.CheckJob = new CheckJobInfo(this.JobModel.Info);

            //this.NextTabCommand = new DelegateCommand(this.OnNextTab, this.CanNextTab);

            //this.BackTabCommand = new DelegateCommand(this.OnBackTab, this.CanBackTab);

            //this.SelectedIndexTab = 0;

            this.LoadedCommand = new DelegateCommand<FrameworkElement>(this.ControlLoaded);

            this.TabChangedCommand = new DelegateCommand(this.OnTabChanged);

            // this.JobDefaultInfo.PropertyChanged += Job_PropertyChanged;


            // delegate close button
            this.CloseWindowCommand = new DelegateCommand(() =>
                {
                    //if (this.isSaved)
                    //{
                    //    this._window.Close();
                    //}
                    this._window.Close();
                });

            // delegate Minimize window button
            this.MinimizeWindowCommand = new DelegateCommand(() =>
                {
                    this._window.WindowState = WindowState.Minimized;
                });

            this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.ChangeClient);

            this.CreateJobCommand = new DelegateCommand(this.OnCreateJob);
        }

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
        /// Gets the minimize window command.
        /// </summary>
        public ICommand MinimizeWindowCommand { get; private set; }

        /// <summary>
        /// Gets the next tab command.
        /// </summary>
        //public DelegateCommand NextTabCommand { get; private set; }

        /// <summary>
        /// Gets the back tab command.
        /// </summary>
        //public DelegateCommand BackTabCommand { get; private set; }

        /// <summary>
        /// Gets the tab changed command.
        /// </summary>
        public ICommand TabChangedCommand { get; private set; }

        /// <summary>
        /// Gets the create job command.
        /// </summary>
        public ICommand CreateJobCommand { get; private set; }

        #endregion

        #region Public Property

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        //public IJob Job
        //{
        //    get => this._job;
        //    set => this.SetProperty(ref this._job, value);
        //}

        /// <summary>
        /// Gets or sets the check job.
        /// </summary>
        public CheckJobInfo CheckJob { get; set; }

        /// <summary>
        /// Gets or sets the is design visibility.
        /// </summary>
        public Visibility IsDesignVisibility
        {
            get => this._isDesignVisibility;
            set => this.SetProperty(ref this._isDesignVisibility, value);

        }

        public Visibility IsStickFrameVisibitily
        {
            get => this._isStickFrameVisibility;
            set => this.SetProperty(ref this._isStickFrameVisibility, value);
        }
        /// <summary>
        /// Gets or sets the selected index tab.
        /// </summary>
        public int SelectedIndexTab
        {
            get => this._selectedIndexTab;
            set
            {
                this.SetProperty(ref this._selectedIndexTab, value);
                //this.NextTabCommand.RaiseCanExecuteChanged();
                //this.BackTabCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanChangedTab => !string.IsNullOrEmpty(this.JobModel.Info.ClientName);

        #endregion

        #region Private Function

        /// <summary>
        /// The control loaded.
        /// </summary>
        /// <param name="param">
        /// The obj.
        /// </param>
        private void ControlLoaded(FrameworkElement param)
        {
            var parrentWindow = WindowHelper.GetWindowParent(param);
            if (parrentWindow == null) return;
            if (parrentWindow is FlatWindow window)
            {
                this._window = window;
            }
            this.SetRegionManager();
            this.OnTabChanged();
        }

        /// <summary>
        /// The set region manager.
        /// </summary>
        private void SetRegionManager()
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();
            var wizardRegionManager = this.RegionManager.CreateRegionManager();
            Prism.Regions.RegionManager.SetRegionManager(this._window, wizardRegionManager);
            this.RegionManager = wizardRegionManager;

            //this.LoadInformationView();
        }

        /// <summary>
        /// The transfer job.
        /// </summary>
        /// <param name="regionName">
        /// The region name.
        /// </param>
        /// <param name="uriPath">
        /// The uri path.
        /// </param>
        private void TransferJob(string regionName, string uriPath)
        {
            var parameters = new NavigationParameters { { "JobDefaultInfo", this.JobModel.Info } };
            if (this.JobModel.Info != null)
            {
                this.RegionManager.RequestNavigate(regionName, uriPath, parameters);
            }
        }

        /// <summary>
        /// The transfer floor info.
        /// </summary>
        /// <param name="regionName">
        /// The region name.
        /// </param>
        /// <param name="uriPath">
        /// The uri path.
        /// </param>
        private void TransferFloorInfo(string regionName, string uriPath)
        {
            var parameters = new NavigationParameters
                                 {
                                     { "JobDefaultInfo", this.JobModel.Info }, 
                                     { "Levels", this.JobModel.Levels }
                                 };
            if (this.JobModel.Info != null && this.JobModel.Levels != null)
            {
                this.RegionManager.RequestNavigate(regionName, uriPath, parameters);
            }
        }

        /// <summary>
        /// The on tab changed.
        /// </summary>
        private void OnTabChanged()
        {
            switch (this.SelectedIndexTab)
            {
                case 0:
                    this.TransferJob(NewJobWizardRegions.NewProJectRegion, nameof(ChoosingCustomerView));
                    break;
                case 1:
                    this.TransferJob(NewJobWizardRegions.BasicInfoRegion, nameof(JobInfoView));
                    break;
                case 2:
                    this.TransferJob(NewJobWizardRegions.WindInforRegion, nameof(WindCategoryView));
                    break;
                case 3:
                    this.TransferJob(NewJobWizardRegions.RoofInfoRegion, nameof(RoofInfoView));
                    break;
                case 4:
                    this.TransferJob(NewJobWizardRegions.AddStickFrameInfoRegion, nameof(AdditionInforForStickFrameView));
                    break;
                case 5:
                    this.TransferJob(NewJobWizardRegions.DesignInfoRegion, nameof(DesignInfoView));
                    break;
                case 6:
                    this.TransferFloorInfo(NewJobWizardRegions.FloorNumberChooseRegion, nameof(LevelInfoView));
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// The on next tab.
        /// </summary>
        //private void OnNextTab()
        //{
        //    if (this.SelectedIndexTab == 3)
        //    {
        //        if (this.IsDesignVisibility == Visibility.Collapsed && this.IsStickFrameVisibitily == Visibility.Collapsed)
        //        {
        //            this.SelectedIndexTab += 3;
        //        }

        //        if (this.IsDesignVisibility == Visibility.)
        //        {
                    
        //        }
                
        //    }
        //    else
        //    {
        //        this.SelectedIndexTab++;
        //    }
        //}

        /// <summary>
        /// The on back tab.
        /// </summary>
        //private void OnBackTab()
        //{
        //    if (this.SelectedIndexTab == 5 & this.IsDesignVisibility == Visibility.Collapsed)
        //    {
        //        this.SelectedIndexTab -= 2;
        //    }
        //    else
        //    {
        //        this.SelectedIndexTab--;
        //    }
        //}

        /// <summary>
        /// The can next tab.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        //private bool CanNextTab()
        //{
        //    return this.SelectedIndexTab != 5;
        //}

        /// <summary>
        /// The can back tab.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        //private bool CanBackTab()
        //{
        //    return this.SelectedIndexTab != 0;
        //}

        /// <summary>
        /// The change clientPoco.
        /// </summary>
        /// <param name="selectClient">
        /// The select ClientPoco.
        /// </param>
        private void ChangeClient(ClientPoco selectClient)
        {
            if (selectClient == null)
            {
                this.IsDesignVisibility = Visibility.Collapsed;
                this.IsStickFrameVisibitily = Visibility.Collapsed;
                return;
            }
            this.IsDesignVisibility = selectClient.Name == ClientNames.Prenail.ToString() ? Visibility.Visible : Visibility.Collapsed;
            this.IsStickFrameVisibitily = selectClient.Name == ClientNames.StickFrame.ToString() ? Visibility.Visible : Visibility.Collapsed;
            RaisePropertyChanged(nameof(CanChangedTab));
        }

        /// <summary>
        /// The on create job.
        /// </summary>
        private void OnCreateJob()
        {
            //this.EventAggregator.GetEvent<JobModelService>().Publish(this.JobDefaultInfo);
            this._window.Close();

        }
        #endregion
    }
}
