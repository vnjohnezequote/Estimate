// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinCategoryViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WinCategoryViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppDataBase.DataBase;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace JobInfoModule.ViewModels
{
    using ApplicationCore.BaseModule;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The win category view model.
    /// </summary>
    public class WindCategoryViewModel : BaseJobInForViewModel
    {
        #region Private Region
        /// <summary>
        /// The prenai wind.
        /// </summary>
        //private PrenailWindCategoryView _prenaiWind;
        private ObservableCollection<string> _windRates;
        private ObservableCollection<string> _treatments;
        #endregion
        #region Property

        public ObservableCollection<string> WindRates
        {
            get => _windRates;
            set => SetProperty(ref _windRates, value);
        }
        public ObservableCollection<string> Treatments
        {
            get => _treatments;
            set => SetProperty(ref _treatments, value);
        }
        #endregion

        //public WindCategoryViewModel()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="WindCategoryViewModel"/> class.
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
        public WindCategoryViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator,ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            //this.RegionManager = this.RegionManager.CreateRegionManager();
        }


        #region Command

        #endregion
        #region Private Funtion

        protected override void Initilazied()
        {
            base.Initilazied();
            WindRatesReseive(SelectedClient.WinRates);
            TreatmentsReseive(SelectedClient.Treatments);
            JobInfo.WindRate = WindRates.FirstOrDefault();
        }

        /// <summary>
        /// The set clientPoco.
        /// </summary>
        /// <param name="clientName">
        /// The clientPoco name.
        /// </param>
        protected override void SelectedClientReceive(ClientPoco selectedClient)
        {
            base.SelectedClientReceive(selectedClient);
            if (selectedClient == null)
            {
                return;
            }
            this.SelectedClient = selectedClient;
            WindRatesReseive(selectedClient.WinRates);
            TreatmentsReseive(selectedClient.Treatments);
        }
        private void WindRatesReseive(List<string> windRates)

        {
            this.WindRates = new ObservableCollection<string>(windRates);
        }

        private void TreatmentsReseive(List<string> treatMents)
        {
            this.Treatments = new ObservableCollection<string>(treatMents);
        }

        #endregion
    }
}

