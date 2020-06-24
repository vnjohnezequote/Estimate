// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignInfoViewModel.cs" company="John nguyen">
//   John nguyen
// </copyright>
// <summary>
//   The design info view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using AppModels;

    using JetBrains.Annotations;

    using LiteDB;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The design info view model.
    /// </summary>
    public class DesignInfoViewModel : BaseViewModelAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignInfoViewModel"/> class.
        /// </summary>
        public DesignInfoViewModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignInfoViewModel"/> class.
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
        public DesignInfoViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            using (var db = new LiteDatabase(@"DesignInfo.db"))
            // using (var db = new LiteDatabase(@"filename=DesignInfo.db;upgrade=true"))
            {
                var designInfors = db.GetCollection<DesignInfor>("DesignInfors");

                var result = designInfors.Find(x => x.InfoType == "Beam").ToList();
                this.BeamDesignInfors = new ObservableCollection<DesignInfor>(result);
                result = designInfors.Find(x => x.InfoType == "Frame").ToList();
                this.FrameDesignInfors = new ObservableCollection<DesignInfor>(result);
                result = designInfors.Find(x => x.InfoType == "Bracing").ToList();
                this.BracingInfors = new ObservableCollection<DesignInfor>(result);

                // this.CreateDatabase(designInfors);
            }

            this.BeamDesignInforChangedCommand = new DelegateCommand(this.OnBeamDesignInfoChanged);
        }

        /// <summary>
        /// Gets the beam design infor changed command.
        /// </summary>
        public ICommand BeamDesignInforChangedCommand { get; private set; }

        /// <summary>
        /// Gets or sets the beam design infors.
        /// </summary>
        public ObservableCollection<DesignInfor> BeamDesignInfors { get; set; }

        /// <summary>
        /// Gets or sets the frame design infors.
        /// </summary>
        public ObservableCollection<DesignInfor> FrameDesignInfors { get; set; }

        /// <summary>
        /// Gets or sets the bracing infors.
        /// </summary>
        public ObservableCollection<DesignInfor> BracingInfors { get; set; }

        /// <summary>
        /// The on beam design info changed.
        /// </summary>
        private void OnBeamDesignInfoChanged()
        {
            if (this.Job is null)
            {
                return;
            }

            if (this.Job.FrameDesignInfor != null && this.Job.FrameDesignInfor.Content != null)
            {
                if (this.Job.FrameDesignInfor.Content == "Engineer")
                {
                    this.Job.IsEngineer = true;
                }
            }

            if (this.Job.BeamDesignInfor != null && this.Job.BeamDesignInfor.Content != null)
            {
                if (Job.BeamDesignInfor.Content == "Engineer")
                {
                    this.Job.IsEngineer = true;
                }
            }

            if (this.Job.BracingDesignInfor!=null && this.Job.BracingDesignInfor.Content!=null)
            {
                if (this.Job.BracingDesignInfor.Content == "Engineer")
                {
                    this.Job.IsEngineer = true;
                }
            }
           
        }

        /// <summary>
        /// The create database.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        private void CreateDatabase(LiteCollection<DesignInfor> db)
        {
            var designInfor = new DesignInfor(){Content = "Engineer",Header = "Frame follow as per",InfoType = "Frame"};
            db.Insert(designInfor);
            designInfor = new DesignInfor(){Content = "Australia Standard",Header = "Frame Design as per", InfoType = "Frame"};
            db.Insert(designInfor);
            designInfor = new DesignInfor(){Content = "Framing Schedule", Header = "Frame follow as per",InfoType = "Frame"};
            db.Insert(designInfor);
            designInfor = new DesignInfor(){Content = "Engineer and Australia Standard", Header = "Frame design as per",InfoType = "Frame"};
            db.Insert(designInfor);

            designInfor = new DesignInfor() { Content = "Engineer", Header = "Beam follow as per", InfoType = "Beam" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Australia Standard", Header = "Beam Design as per", InfoType = "Beam" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Framing Schedule", Header = "Beam follow as per",InfoType = "Beam"};
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Engineer and Australia Standard", Header = "Frame design as per", InfoType = "Beam" };
            db.Insert(designInfor);

            designInfor = new DesignInfor() { Content = "Engineer", Header = "Bracing supply as per", InfoType = "Bracing" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Bracing Plan", Header = "Bracing supply as per", InfoType = "Bracing" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "TBA", Header = "No Bracing supply", InfoType = "Bracing" };
            db.Insert(designInfor);

            db.EnsureIndex(x => x.Content);
            db.EnsureIndex(x => x.InfoType);

        }

    }
}