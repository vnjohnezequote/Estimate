// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralWindCategoryViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the GeneralWindCategoryViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule.ViewModels
{
    using ApplicationCore.BaseModule;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using System.Windows.Input;
    using Unity;

    /// <summary>
    /// The general wind category view model.
    /// </summary>
    public class GeneralWindCategoryViewModel : BaseViewModelAware
    {
        /// <summary>
        /// Defines the windN1
        /// </summary>
        private bool windN1;

        /// <summary>
        /// Defines the windN2
        /// </summary>
        private bool windN2;

        /// <summary>
        /// Defines the windN3
        /// </summary>
        private bool windN3;

        /// <summary>
        /// Defines the windN4
        /// </summary>
        private bool windN4;

        /// <summary>
        /// Defines the unTreated
        /// </summary>
        private bool unTreated;

        /// <summary>
        /// Defines the h2Treated
        /// </summary>
        private bool h2Treated;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWindCategoryViewModel"/> class.
        /// </summary>
        public GeneralWindCategoryViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWindCategoryViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">The unityContainer<see cref="IUnityContainer"/></param>
        /// <param name="regionManager">The regionManager<see cref="IRegionManager"/></param>
        /// <param name="eventAggregator">The eventAggregator<see cref="IEventAggregator"/></param>
        public GeneralWindCategoryViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            this.WindCommand = new DelegateCommand<string>(OnSetWindCategory);
            this.WindLoadedCommand = new DelegateCommand(OnControlLoaded);
            this.TreatmentCommand = new DelegateCommand<string>(OnSetTreatment);
        }

        /// <summary>
        /// Gets the WindLoadedCommand
        /// </summary>
        public ICommand WindLoadedCommand { get; private set; }

        /// <summary>
        /// Gets the WindCommand
        /// </summary>
        public ICommand WindCommand { get; private set; }

        /// <summary>
        /// Gets the TreatmentCommand
        /// </summary>
        public ICommand TreatmentCommand { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 1.
        /// </summary>
        public bool WindN1
        {
            get => this.CheckWindRate("N1");
            set
            {
                if (this.Job.WindRate.WindRate == "N1")
                {
                    this.SetProperty(ref this.windN1, true);
                }
                else
                {
                    this.SetProperty(ref this.windN1, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 2.
        /// </summary>
        public bool WindN2
        {
            get => this.CheckWindRate("N2");
            set
            {
                if (this.Job.WindRate.WindRate == "N2")
                {
                    this.SetProperty(ref this.windN2, true);
                }
                else
                {
                    this.SetProperty(ref this.windN2, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 3.
        /// </summary>
        public bool WindN3
        {
            get => this.CheckWindRate("N3");
            set
            {
                if (this.Job.WindRate.WindRate == "N3")
                {
                    this.SetProperty(ref this.windN3, true);
                }
                else
                {
                    this.SetProperty(ref this.windN3, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindN4
        {
            get => this.CheckWindRate("N4");
            set
            {
                if (this.Job.WindRate.WindRate == "N4")
                {
                    this.SetProperty(ref this.windN4, true);
                }
                else
                {
                    this.SetProperty(ref this.windN4, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether un treated.
        /// </summary>
        public bool UnTreated
        {
            get => this.CheckTreatment("Untreated");
            set
            {
                if (this.Job.Treatment == "Untreated")
                {
                    this.SetProperty(ref this.unTreated, true);
                }
                else
                {
                    this.SetProperty(ref this.unTreated, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether h 2 treated.
        /// </summary>
        public bool H2Treated
        {
            get => this.CheckTreatment("H2");
            set
            {
                if (this.Job.Treatment == "H2")
                {
                    this.SetProperty(ref this.h2Treated, true);
                }
                else
                {
                    this.SetProperty(ref this.h2Treated, false);
                }
            }
        }

        /// <summary>
        /// The on control loaded.
        /// </summary>
        private void OnControlLoaded()
        {
            this.CheckWindRate();
            this.CheckTreatment();
        }

        /// <summary>
        /// The check treatment.
        /// </summary>
        private void CheckTreatment()
        {
            if (!string.IsNullOrEmpty(this.Job.Treatment))
            {
                switch (this.Job.Treatment)
                {
                    case "H2":
                        this.H2Treated = true;
                        this.UnTreated = false;
                        break;
                    case "Untreated":
                        this.H2Treated = false;
                        this.UnTreated = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// The check wind rate.
        /// </summary>
        private void CheckWindRate()
        {
            if (!string.IsNullOrEmpty(this.Job.WindRate.WindRate))
            {
                switch (this.Job.WindRate.WindRate)
                {
                    case "N1":
                        this.WindN1 = true;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        break;
                    case "N2":
                        this.WindN1 = false;
                        this.WindN2 = true;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        break;
                    case "N3":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = true;
                        this.WindN4 = false;
                        break;
                    case "N4":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = true;
                        break;
                }
            }
        }

        /// <summary>
        /// The CheckWindRate
        /// </summary>
        /// <param name="windRate">The windRate<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool CheckWindRate(string windRate)
        {
            if (this.Job == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.Job.WindRate.WindRate))
                {
                    return false;
                }
                else
                {
                    return this.Job.WindRate.WindRate == windRate;
                }
            }
        }

        /// <summary>
        /// The CheckTreatment
        /// </summary>
        /// <param name="treatMent">The treatMent<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool CheckTreatment(string treatMent)
        {
            if (this.Job == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.Job.Treatment))
                {
                    return false;
                }
                else
                {
                    return this.Job.Treatment == treatMent;
                }
            }
        }

        /// <summary>
        /// The on set wind category.
        /// </summary>
        /// <param name="windRate">The windRate<see cref="string"/></param>
        private void OnSetWindCategory(string windRate)
        {
            this.Job.WindRate.WindRate = windRate;
            this.RaisePropertyChanged("WindN1");
            this.RaisePropertyChanged("WindN2");
            this.RaisePropertyChanged("WindN3");
            this.RaisePropertyChanged("WindN4");
        }

        /// <summary>
        /// The OnSetTreatment
        /// </summary>
        /// <param name="treatMent">The treatMent<see cref="string"/></param>
        private void OnSetTreatment(string treatMent)
        {
            this.Job.Treatment = treatMent;
            this.RaisePropertyChanged(nameof(UnTreated));
            this.RaisePropertyChanged(nameof(H2Treated));
        }
    }
}
