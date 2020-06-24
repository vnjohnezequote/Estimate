using System.Windows;
using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationService;
using devDept.Eyeshot;
using devDept.Graphics;
using DrawingModule.CustomControl.PaperSpaceControl;
using DrawingModule.DrawToolBase;
using DrawingModule.Interface;
using DrawingModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;
using Unity;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;
using Grid = System.Windows.Controls.Grid;

namespace DrawingModule.ViewModels
{
    public class CanvasDrawingViewModel : BaseViewModel
    {
        #region Field

        private Grid _canvasGrid;
        private bool _isCanvasMouseOn;
        private readonly ObservableAsPropertyHelper<Visibility> _dynamicInputVisibility;
        private bool _hintViewrVisible;
        private CanvasDrawing _canvasDrawing;
        private PaperSpaceDrawing _paperSpace;
        private CanvasDrawingView _canvasDrawingView;
        
        private DynamicInputViewModel  _dynamciInputViewModel;
        #endregion
        #region Properties

        public bool IsCanvasMouseOn
        {
            get => this._isCanvasMouseOn;
            set
            {
                this.SetProperty(ref _isCanvasMouseOn, value);
                this.EventAggre.GetEvent<DynamicInputViewEvent>().Publish(this._isCanvasMouseOn);
            } 
        }

        //public Visibility DynamicInputVisibility => IsCanvasMouseOn ? Visibility.Visible : Visibility.Collapsed;
        //public Visibility DynamicInputVisibility => this.IsCanvasMouseOn ? Visibility.Visible : Visibility.Collapsed;
        

        public bool HintViewerVisible
        {
            get => this._hintViewrVisible;
            set => this.SetProperty(ref _hintViewrVisible, value);
        }
        #endregion
        #region Command

        public  ICommand CanvasDrawingLoadedCommand { get; private set; }
        public ICommand CanvasDrawingMouseLeave { get; private set; }
        
        public ICommand CanvasDrawingMouseEnter { get; private set; }
        
        #endregion
        #region Constructor
        public CanvasDrawingViewModel() :base()
        {}
        public CanvasDrawingViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            //CanvasDrawingLoadedCommand = ReactiveCommand.Create<Grid,Grid >(canvasGrid =>this._canvasGrid = canvasGrid );
            //CanvasDrawingLoadedCommand = new DelegateCommand<CanvasDrawing>(OnCanvasDrawingLoaded);
            CanvasDrawingLoadedCommand = new DelegateCommand<CanvasDrawingView>(OnCanvasDrawingLoaded);
            CanvasDrawingMouseLeave = new DelegateCommand(OnCanvasDrawingMouseLeave);
            CanvasDrawingMouseEnter = new DelegateCommand(OnCanvasDrawingMouseEnter);
            this.EventAggre.GetEvent<CommandExcuteStringEvent>().Subscribe(ExcuteCommand);
        }

        

        #endregion
        #region Private Method

        //private void SubscriberPromptStatus(PromptStatus promptStatus)
        //{
        //    if (promptStatus == PromptStatus.Cancel)
        //    {
        //        this._canvasDrawing.ProcessCancelTool(promptStatus);
        //    }
        //}
        private void ExcuteCommand(string commandExcuteString)
        {
            if (this._canvasDrawingView.TabDrawing.IsSelected)
            {
                _canvasDrawing?.ProcessCommand(commandExcuteString);
            }
            
            if (this._canvasDrawingView.TabPaperSpace.IsSelected)
            {
                _paperSpace?.ProcessCommand(commandExcuteString);
            }
        }
        #endregion

        #region Public Method



        #endregion

        #region Command Method

        //private void OnCanvasDrawingLoaded(CanvasDrawing canvasDrawing)
        //{
        //    canvasDrawing.SetView(viewType.Top);
        //    canvasDrawing.Camera.ProjectionMode = projectionType.Orthographic;
        //}
        private void OnCanvasDrawingMouseLeave()
        {
            //if (HintViewerVisible)
            //{
            //    this.IsCanvasMouseOn = true;
            //    return;
            //}
            this.IsCanvasMouseOn = false;
        }
        private void OnCanvasDrawingMouseEnter()
        {
            this.IsCanvasMouseOn = true;
            this._canvasDrawing.Focus();
        }

        private void OnCanvasDrawingLoaded(CanvasDrawingView canvasDrawingView)
        {
            if (canvasDrawingView.CanvasDrawing == null)
            {
                return;
            }
            canvasDrawingView.CanvasDrawing.SetView(viewType.Top);
            canvasDrawingView.CanvasDrawing.Camera.ProjectionMode = projectionType.Orthographic;
            this._canvasDrawing = canvasDrawingView.CanvasDrawing;
            _canvasDrawing.SetDynamicInput(canvasDrawingView.DynamicInput);
            _paperSpace = canvasDrawingView.PaperSpace;
            this._canvasDrawingView = canvasDrawingView;
            this._dynamciInputViewModel= canvasDrawingView.DynamicInput.DataContext as DynamicInputViewModel;
            _canvasDrawing.ToolChanged += OnToolChanged;
            _canvasDrawing.UserInteraction += OnUserInteraction;
            _paperSpace.ToolChanged += OnToolChanged;
            //_paperSpace.UserInteraction += OnUserInteraction;
            Application.Application.DocumentManager.MdiActiveDocument.Editor.InitializingEditor(_canvasDrawing,_dynamciInputViewModel,_paperSpace,canvasDrawingView,canvasDrawingView.DynamicInput);
        }

        private void OnUserInteraction()
        {
            this._dynamciInputViewModel.NotifyToolChanged();
        }

        private void OnToolChanged(ICadDrawAble cadDraw, ToolChangedArgs toolArgs)
        {
            _dynamciInputViewModel?.SetCurrentTool(toolArgs.CurrentTool);
        }


        #endregion
    }
}
