﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelInfoViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelInfoViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using AppModels;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    using Views;

    /// <summary>
    /// The level info view model.
    /// </summary>
    public class LevelInfoViewModel : BaseViewModel, INavigationAware
    {
        #region private Field
        /// <summary>
        /// The job.
        /// </summary>
        private JobInfo job;

        /// <summary>
        /// The is cost visible.
        /// </summary>
        private bool isCostVisible;

        /// <summary>
        /// The select index.
        /// </summary>
        private int? selectIndex;

        /// <summary>
        /// The levels.
        /// </summary>
        private ObservableCollection<LevelWall> levels;

        /// <summary>
        /// The floor names.
        /// </summary>
        private string[] floorNames = new string[]
                                          {
                                              "Ground Floor", "First Floor", "Second Floor", "Third Floor",
                                              "Fourth Floor", "Fifth Floor", "Sixth Floor"
                                          };


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelInfoViewModel"/> class.
        /// </summary>
        public LevelInfoViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelInfoViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        public LevelInfoViewModel(
            IUnityContainer unityContainer,
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();

            this.CreateFloorCommand = new DelegateCommand<ListBox>(this.CreateFloorFunction);

        }

        #endregion

        #region Command

        /// <summary>
        /// Gets the create floor command.
        /// </summary>
        public ICommand CreateFloorCommand { get; private set; }

        #endregion
        #region Property

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        public JobInfo Job
        {
            get => this.job;
            set => this.SetProperty(ref this.job, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is cost visible.
        /// </summary>
        public bool IsCostVisible
        {
            get => this.isCostVisible;
            set => this.SetProperty(ref this.isCostVisible, value);
        }

        /// <summary>
        /// Gets or sets the selected index.
        /// </summary>
        public int? SelectedIndex
        {
            get => this.selectIndex;
            set => this.SetProperty(ref this.selectIndex, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is created.
        /// </summary>
        public bool IsCreated { get; set; } = false;

        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        public ObservableCollection<LevelWall> Levels
        {
            get => this.levels;
            set => this.SetProperty(ref this.levels, value);
        }
        #endregion


        #region public method

        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["Job"] is JobInfo job)
            {
                this.Job = job;
            }
            if (navigationContext.Parameters["Levels"] is ObservableCollection<LevelWall> levels)
            {
                this.Levels = levels;
            }

            if (this.Job == null)
            {
                this.IsCostVisible = false;
                return;
                //this.SelectedIndex = null;
            }

            this.IsCostVisible = this.Job.ClientName == "Prenail";

            if (this.Levels == null || this.Levels.Count == 0)
            {
                this.SelectedIndex = null;
            }
            else
            {
                if (this.IsCreated)
                {
                    return;
                }
                this.LoadCostDelivery();
                this.LoadFloorNumber();
                this.SelectedIndex = this.Levels.Count - 1;
            }
        }

        /// <summary>
        /// The is navigation target.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// The on navigated from.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }


        #endregion
        #region private Method

        /// <summary>
        /// The load cost delivery.
        /// </summary>
        private void LoadCostDelivery()
        {
            var stepIn = 0;

            switch (this.Levels.Count)
            {
                case 1:
                    stepIn = 1;
                    break;
                case 2:
                    stepIn = 2;
                    break;
                default:
                    {
                        if (this.Levels.Count > 2)
                        {
                            stepIn = 3;
                        }

                        break;
                    }
            }
            for (var i = 0; i < stepIn; i++)
            {
                var levelWall = this.Levels[i];
                var parameters = new NavigationParameters { { "Level", levelWall } };
                if (levelWall != null)
                {
                    this.RegionManager.RequestNavigate("DeliveryRegion", nameof(CostDeliveryView), parameters);
                }
            }
        }

        /// <summary>
        /// The change floor number.
        /// </summary>
        private void LoadFloorNumber()
        {
            foreach (var levelWall in this.Levels)
            {
                var parameters = new NavigationParameters { { "Level", levelWall } };
                if (levelWall != null)
                {
                    this.RegionManager.RequestNavigate("FloorInfoRegion", nameof(FloorInfoView), parameters);
                }
            }
        }

        /// <summary>
        /// The creat floor.
        /// </summary>
        /// <param name="floorNumber">
        /// The floor number.
        /// </param>
        private void CreatFloor(int floorNumber)
        {
            if (this.Levels.Count == floorNumber)
            {
                return;
            }
            else if (this.Levels.Count > floorNumber)
            {
                var stepIn = this.Levels.Count - floorNumber;
                for (var i = 0; i < stepIn; i++)
                {
                    this.Levels.RemoveAt(this.Levels.Count - 1);
                }
            }
            else if (this.Levels.Count < floorNumber)
            {
                var stepIn = floorNumber - this.Levels.Count;
                var levelNameIndex = this.Levels.Count;

                for (var i = 0; i < stepIn; i++)
                {
                    var level = new LevelWall(this.Job.JobDefault);
                    level.LevelName = this.floorNames[levelNameIndex];

                    /* this is for testing */
                    level.LevelInfo.ExternalWallHeight = 2740;
                    level.LevelInfo.InternalWallHeight = 2600;
                    level.LevelInfo.ExternalWallThickness = 90;
                    level.LevelInfo.InternalWallThickness = 70;
                    
                    

                    this.Levels.Add(level);
                    levelNameIndex++;
                }

            }

            // this.RaisePropertyChanged(nameof(this.Job.Levels));
            this.IsCreated = true;

        }



        /// <summary>
        /// The create floor function.
        /// </summary>
        /// <param name="levelItem">
        /// The level item.
        /// </param>
        private void CreateFloorFunction(ListBox levelItem)
        {
            this.RegionManager.Regions["FloorInfoRegion"].RemoveAll();
            this.RegionManager.Regions["DeliveryRegion"].RemoveAll();
            this.CreatFloor(levelItem.SelectedIndex + 1);
            this.LoadCostDelivery();
            this.LoadFloorNumber();


        }



        

        #endregion


    }
}
