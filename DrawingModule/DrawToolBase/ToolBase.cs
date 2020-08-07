using System;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using AppModels.EventArg;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.Interface;
using DrawingModule.Views;
using Prism.Mvvm;

namespace DrawingModule.DrawToolBase
{
    public abstract class ToolBase :BindableBase, IDrawInteractive
    {
        private string _toolMessage;
        private IDynamicInputView _dynamicInput;
        public event EventHandler ToolMessageChanged;
        public string ActiveLevel { get; set; }
        public IEntitiesManager EntitiesManager { get; private set; }
        public ILayerManager LayerManager { get; private set; }
        public virtual string ToolName { get; }
        public virtual double CurrentWidth { get; set; }
        public virtual double CurrentHeight { get; set; }
        public virtual double CurrentAngle { get; set; }
        public virtual double ScaleFactor { get; set; }
        protected bool _isInit = false;
        public virtual string ToolMessage
        {
            get=>_toolMessage;
            set
            {
                SetProperty(ref _toolMessage, value);
                ToolMessageChanged?.Invoke(this,System.EventArgs.Empty);
            } 
        }
        public virtual bool IsSnapEnable { get; protected set; }
        public abstract Point3D BasePoint { get; protected set; }
        public Point3D ReferencePoint { get; protected set; }
        public bool IsUsingOrthorMode { get; protected set; }
        public bool IsUsingLengthTextBox { get; protected set; }
        public bool IsUsingWidthTextBox { get; protected set; }
        public bool IsUsingHeightTextBox { get; protected set; }
        public bool IsUsingAngleTextBox { get; protected set; }
        public bool IsUsingTextStringTextBox { get; protected set; }
        public bool IsUsingTextStringHeightTextBox { get; protected set; }
        public bool IsUsingTextStringAngleTextBox { get; protected set; }
        public bool IsUsingLeaderSegmentTextBox { get; protected set; }
        public bool IsUsingArrowHeadSizeTextBox { get; protected set; }
        public bool IsUsingScaleFactorTextBox { get; protected set; }
        public bool IsUsingMultilineTextBox { get; protected set; }
        public bool IsUsingOffsetDistance { get; protected set; }
        public bool IsUsingSwitchMode { get; protected set; }
        public UnderMouseDrawingType EntityUnderMouseDrawingType { get; protected set; }
        public FocusType DefaultDynamicInputTextBoxToFocus { get; protected set; }
        
        // Error here if click by mouse
        public IDynamicInputView DynamicInput
        {
            get
            {
                if (_dynamicInput == null)
                {
                    return null;
                }

                return _dynamicInput;
            }
        } 

        protected ToolBase()
        {
            InitForToolBase();
            _isInit = true;
        }

        private void InitForToolBase()
        {
            IsSnapEnable = true;
            IsUsingOrthorMode = true;
            IsUsingLengthTextBox = false;
            IsUsingWidthTextBox = false;
            IsUsingHeightTextBox = false;
            IsUsingAngleTextBox = false;
            IsUsingTextStringTextBox = false;
            IsUsingTextStringHeightTextBox = false;
            IsUsingTextStringAngleTextBox = false;
            IsUsingLeaderSegmentTextBox = false;
            IsUsingArrowHeadSizeTextBox = false;
            IsUsingScaleFactorTextBox = false;
            IsUsingSwitchMode = false;
            IsUsingOffsetDistance = false;
            DefaultDynamicInputTextBoxToFocus = FocusType.Length;
            ScaleFactor = 1;
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
        }
        public virtual void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public virtual void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        public virtual void NotifyMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        public virtual void OnJigging(object sender, DrawInteractiveArgs e)
        {
            
        }

        public virtual void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    OnMoveNextTab();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        protected virtual void OnMoveNextTab()
        {

        }

        public virtual void SetDynamicInput(IDynamicInputView dynamicInput)
        {
            this._dynamicInput = dynamicInput;
        }

        public void SetLayersManager(ILayerManager layerManager)
        {
            this.LayerManager = layerManager;
        }

        public void SetEntitiesManager(IEntitiesManager entitiesManager)
        {
            this.EntitiesManager = entitiesManager;
        }
    }
}