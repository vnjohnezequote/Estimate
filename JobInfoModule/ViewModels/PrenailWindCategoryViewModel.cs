namespace JobInfoModule.ViewModels
{
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using JetBrains.Annotations;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// Defines the <see cref="PrenailWindCategoryViewModel" />
    /// </summary>
    public class PrenailWindCategoryViewModel : BaseViewModelAware
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
        /// Defines the windN5
        /// </summary>
        private bool windN5;

        /// <summary>
        /// Defines the windN6
        /// </summary>
        private bool windN6;

        /// <summary>
        /// Defines the windC1
        /// </summary>
        private bool windC1;

        /// <summary>
        /// Defines the windC2
        /// </summary>
        private bool windC2;

        /// <summary>
        /// Defines the windC3
        /// </summary>
        private bool windC3;

        /// <summary>
        /// Defines the windC4
        /// </summary>
        private bool windC4;

        /// <summary>
        /// Defines the unTreated
        /// </summary>
        private bool unTreated;

        /// <summary>
        /// Defines the h2Treated
        /// </summary>
        private bool h2Treated;

        /// <summary>
        /// Defines the h2STreated
        /// </summary>
        private bool h2STreated;

        /// <summary>
        /// Defines the t2Blue
        /// </summary>
        private bool t2Blue;

        /// <summary>
        /// Defines the t2Red
        /// </summary>
        private bool t2Red;

        /// <summary>
        /// Defines the t3Green
        /// </summary>
        private bool t3Green;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailWindCategoryViewModel"/> class.
        /// </summary>
        public PrenailWindCategoryViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailWindCategoryViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">The unityContainer<see cref="IUnityContainer"/></param>
        /// <param name="regionManager">The regionManager<see cref="IRegionManager"/></param>
        /// <param name="eventAggregator">The eventAggregator<see cref="IEventAggregator"/></param>
        public PrenailWindCategoryViewModel(
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
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindN5
        {
            get => this.CheckWindRate("N5");
            set
            {
                if (this.Job.WindRate.WindRate == "N5")
                {
                    this.SetProperty(ref this.windN5, true);
                }
                else
                {
                    this.SetProperty(ref this.windN5, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindN6
        {
            get => this.CheckWindRate("N6");
            set
            {
                if (this.Job.WindRate.WindRate == "N6")
                {
                    this.SetProperty(ref this.windN6, true);
                }
                else
                {
                    this.SetProperty(ref this.windN6, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindC1
        {
            get => this.CheckWindRate("C1");
            set
            {
                if (this.Job.WindRate.WindRate == "C1")
                {
                    this.SetProperty(ref this.windC1, true);
                }
                else
                {
                    this.SetProperty(ref this.windC1, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindC2
        {
            get => this.CheckWindRate("C2");
            set
            {
                if (this.Job.WindRate.WindRate == "C2")
                {
                    this.SetProperty(ref this.windC2, true);
                }
                else
                {
                    this.SetProperty(ref this.windC2, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindC3
        {
            get => this.CheckWindRate("C3");
            set
            {
                if (this.Job.WindRate.WindRate == "C3")
                {
                    this.SetProperty(ref this.windC3, true);
                }
                else
                {
                    this.SetProperty(ref this.windC3, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wind n 4.
        /// </summary>
        public bool WindC4
        {
            get => this.CheckWindRate("C4");
            set
            {
                if (this.Job.WindRate.WindRate == "C4")
                {
                    this.SetProperty(ref this.windC4, true);
                }
                else
                {
                    this.SetProperty(ref this.windC4, false);
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
        /// Gets or sets a value indicating whether h 2 treated.
        /// </summary>
        public bool H2STreated
        {
            get => this.CheckTreatment("H2S");
            set
            {
                if (this.Job.Treatment == "H2S")
                {
                    this.SetProperty(ref this.h2STreated, true);
                }
                else
                {
                    this.SetProperty(ref this.h2STreated, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether h 2 treated.
        /// </summary>
        public bool T2Blue
        {
            get => this.CheckTreatment("T2 Blue");
            set
            {
                if (this.Job.Treatment == "T2 Blue")
                {
                    this.SetProperty(ref this.t2Blue, true);
                }
                else
                {
                    this.SetProperty(ref this.t2Blue, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether h 2 treated.
        /// </summary>
        public bool T2Red
        {
            get => this.CheckTreatment("T2 Red");
            set
            {
                if (this.Job.Treatment == "T2 Red")
                {
                    this.SetProperty(ref this.t2Red, true);
                }
                else
                {
                    this.SetProperty(ref this.t2Red, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether h 2 treated.
        /// </summary>
        public bool T3Green
        {
            get => this.CheckTreatment("T3 Green");
            set
            {
                if (this.Job.Treatment == "T3 Green")
                {
                    this.SetProperty(ref this.t3Green, true);
                }
                else
                {
                    this.SetProperty(ref this.t3Green, false);
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
                        this.H2STreated = false;
                        this.T2Blue = false;
                        this.T2Red = false;
                        this.T3Green = false;
                        break;
                    case "Untreated":
                        this.H2Treated = false;
                        this.UnTreated = true;
                        this.H2STreated = false;
                        this.T2Blue = false;
                        this.T2Red = false;
                        this.T3Green = false;
                        break;
                    case "H2S":
                        this.H2Treated = false;
                        this.UnTreated = false;
                        this.H2STreated = true;
                        this.T2Blue = false;
                        this.T2Red = false;
                        this.T3Green = false;
                        break;
                    case "T2 Blue":
                        this.H2Treated = false;
                        this.UnTreated = false;
                        this.H2STreated = false;
                        this.T2Blue = true;
                        this.T2Red = false;
                        this.T3Green = false;
                        break;
                    case "T2 Red":
                        this.H2Treated = false;
                        this.UnTreated = false;
                        this.H2STreated = false;
                        this.T2Blue = false;
                        this.T2Red = true;
                        this.T3Green = false;
                        break;
                    case "T3 Green":
                        this.H2Treated = false;
                        this.UnTreated = false;
                        this.H2STreated = false;
                        this.T2Blue = false;
                        this.T2Red = false;
                        this.T3Green = true;
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
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "N2":
                        this.WindN1 = false;
                        this.WindN2 = true;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "N3":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = true;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "N4":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = true;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "N5":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = true;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "N6":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = true;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "C1":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = true;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "C2":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = true;
                        this.WindC3 = false;
                        this.WindC4 = false;
                        break;
                    case "C3":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = true;
                        this.WindC4 = false;
                        break;
                    case "C4":
                        this.WindN1 = false;
                        this.WindN2 = false;
                        this.WindN3 = false;
                        this.WindN4 = false;
                        this.WindN5 = false;
                        this.WindN6 = false;
                        this.WindC1 = false;
                        this.WindC2 = false;
                        this.WindC3 = false;
                        this.WindC4 = true;
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
            this.RaisePropertyChanged("WindN5");
            this.RaisePropertyChanged("WindN6");
            this.RaisePropertyChanged("WindC1");
            this.RaisePropertyChanged("WindC2");
            this.RaisePropertyChanged("WindC3");
            this.RaisePropertyChanged("WindC4");
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
            this.RaisePropertyChanged(nameof(H2STreated));
            this.RaisePropertyChanged(nameof(T2Blue));
            this.RaisePropertyChanged(nameof(T2Red));
            this.RaisePropertyChanged(nameof(T3Green));
        }
    }
}
