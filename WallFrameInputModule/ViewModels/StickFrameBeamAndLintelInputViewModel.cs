using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace WallFrameInputModule.ViewModels
{
    public class StickFrameBeamAndLintelInputViewModel : BaseViewModel
    {
        #region Field

        private int _startItemId;
        private Beam _selectedBeam;
        #endregion

        #region Properties
        public LevelWall LevelInfo { get; set; }
        public ObservableCollection<Beam> Beams { get; } = new ObservableCollection<Beam>();
        public Beam SelectedBeam { get=>_selectedBeam; set=>SetProperty(ref _selectedBeam,value); }
        #endregion
        #region Command
        public ICommand CreateNewBeamCommand { get;private set; }
        public ICommand AutoMathBeamWithEngineerBeamList { get; private set; }
        public ICommand AddSupportToBeam { get; private set; }


        #endregion
        public StickFrameBeamAndLintelInputViewModel(){}

        public StickFrameBeamAndLintelInputViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {

            CreateNewBeamCommand = new DelegateCommand(OnCreateBeamCommand);
            AutoMathBeamWithEngineerBeamList = new DelegateCommand(OnAutoMathBeamList);
            AddSupportToBeam = new DelegateCommand(OnAddSupportToBeam);
            JobModel.EngineerMemberList.CollectionChanged += EngineerMemberList_CollectionChanged;
        }

        private void EngineerMemberList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
            foreach (var beam in Beams)
            {
                beam.NotifyPropertyChanged();
            }
        }

        private void OnCreateBeamCommand()
        {
            this._startItemId = Beams.Count + 1;
            var beam = new Beam(BeamType.RoofBeam,this.LevelInfo.LevelInfo){Id = _startItemId};
            Beams.Add(beam);
        }

        private void OnAddSupportToBeam()
        {
            SelectedBeam.AddSupport();
        }

        private void OnAutoMathBeamList()
        {
            foreach (var beam in Beams)
            {
                foreach (var engineerBeam in JobModel.EngineerMemberList)
                {
                    if (beam.SpanLength<=0)
                    {
                        continue;
                    }

                    if (beam.SpanLength>engineerBeam.MinSpan && beam.SpanLength<=engineerBeam.MaxSpan)
                    {
                        beam.EngineerMemberInfo = engineerBeam;
                    }
                }
            }
        }
    }
}
