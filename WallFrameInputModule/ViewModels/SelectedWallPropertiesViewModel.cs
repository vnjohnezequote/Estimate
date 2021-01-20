using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace WallFrameInputModule.ViewModels
{
    public class SelectedWallPropertiesViewModel: BaseJobInForViewModel
    {
        #region Field

        private WallBase _selectedWall;
        private List<string> _ribbonPlates;
        private List<string> _topPlates;
        private List<string> _bottomPlates;
        private List<string> _studs;
        #endregion

        #region Properties

        public List<string> RibbonPlates { get=>_ribbonPlates; set=>SetProperty(ref _ribbonPlates,value); } 
        public List<string> TopPlates { get=>_topPlates; set=>SetProperty(ref _topPlates,value); }
        public List<string>BottomPlates { get=>_bottomPlates; set=>SetProperty(ref _bottomPlates,value); }
        public List<string> Studs { get=>_studs; set=>SetProperty(ref _studs,value); }

        public WallBase SelectedWall
        {
            get=>_selectedWall;
            set
            {
                if (value !=null)
                {
                    value.PropertyChanged += WallBasePropertiesChanged;
                }

                if (SelectedWall!=null)
                {
                    SelectedWall.PropertyChanged -= WallBasePropertiesChanged;
                }
                SetProperty(ref _selectedWall,value);
            }
        }

       

        public List<int> WallThicknessList { get; set; } = new List<int>(){90,70};
        public List<int> WallSpacingList { get; set; } = new List<int>(){300,350,400,450,600};

        #endregion

        public SelectedWallPropertiesViewModel()
        { }    
        public SelectedWallPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {
            RibbonPlates = new List<string>();
            TopPlates = new List<string>();
            BottomPlates = new List<string>();
            Studs = new List<string>();
            PropertyChanged += SelectedWallPropertiesViewModel_PropertyChanged;
            this.EventAggregator.GetEvent<WallEventService>().Subscribe(OnWallSelectedReceives);
        }

        private void WallBasePropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedWall.WallThickness):
                    var tempSelectedWall = SelectedWall;
                    if (SelectedClient!=null)
                    {
                        RibbonPlates = GetMaterialList(SelectedClient.RibbonPlates);
                        TopPlates = GetMaterialList(SelectedClient.TopPlates);
                        BottomPlates = GetMaterialList(SelectedClient.BottomPlates);
                        Studs = GetStudList(SelectedClient.Studs);
                    }

                    SelectedWall = null;
                    SelectedWall = tempSelectedWall;
                    break;
            }
        }
        private void SelectedWallPropertiesViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedWall):
                case nameof(SelectedClient):
                    if (SelectedClient!=null)
                    {
                        RibbonPlates = GetMaterialList(SelectedClient.RibbonPlates);
                        TopPlates = GetMaterialList(SelectedClient.TopPlates);
                        BottomPlates = GetMaterialList(SelectedClient.BottomPlates);
                        Studs = GetStudList(SelectedClient.Studs);
                    }
                    break;
            }
        }

        private void OnWallSelectedReceives(WallBase selectedWallBase)
        {
            SelectedWall = selectedWallBase;
        }

        private List<string> GetMaterialList(Dictionary<string,List<TimberBase>> timberMaterialsDict)
        {
            var result = new List<string>();
            if (this.SelectedWall== null || SelectedClient == null)
            {
                return result;
            }

            if (timberMaterialsDict.ContainsKey(SelectedWall.WallThickness.ToString()))
            {
                
                timberMaterialsDict.TryGetValue(SelectedWall.WallThickness.ToString(), out var timberPlates);
                if (timberPlates!=null)
                {
                 result.AddRange(timberPlates.Select(timberBase=>timberBase.SizeGrade));
                 return result;
                }
            }

            return result;
        }

        private List<string> GetStudList(Dictionary<string, List<TimberBase>> studsDict)
        {
            var result = new List<string>();
            if (this.SelectedWall == null || SelectedClient == null)
            {
                return result;
            }

            var wallKey = "LBW";
            if (SelectedWall.WallType!=null && !SelectedWall.WallType.IsLoadBearingWall)
            {
                wallKey = "NONLBW";
            }
            if (SelectedWall.GlobalWallInfo.GlobalInfo.Client.Name == "Warnervale")
            {
                wallKey = SelectedWall.WallThickness.ToString();
            }

            if (studsDict.ContainsKey(wallKey))
            {
                studsDict.TryGetValue(wallKey, out var studs);
                if (studs!=null)
                {
                    var selectedStud = studs.FindAll(x => x.Thickness == SelectedWall.WallThickness);
                    result.AddRange(selectedStud.Select(stud=>stud.SizeGrade));
                    return result;
                }
            }

            return result;
        }
        

    }
}
