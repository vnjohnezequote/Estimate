using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.Windows.Input;
using ApplicationInterfaceCore.Enums;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Geometry;

namespace ApplicationInterfaceCore
{
    public interface IDrawInteractive : INotifyPropertyChanged
    {
        //DrawInteractiveDelegate DrawInteractiveHandler { get; }
        //event EventHandler ToolMessageChanged;
        IEntitiesManager EntitiesManager { get; }
        ILayerManager LayerManager { get; }
        string ActiveLevel { get; set; }
        string ToolName { get; }
        string ToolMessage { get; }
        bool IsSnapEnable { get; }
        Point3D BasePoint { get; }
        Point3D ReferencePoint { get; }
        bool IsUsingOrthorMode { get; }
        bool IsUsingLengthTextBox { get; }
        bool IsUsingWidthTextBox { get; }
        bool IsUsingHeightTextBox { get; }
        bool IsUsingAngleTextBox { get; }
        bool IsUsingTextStringTextBox { get; }
        bool IsUsingTextStringHeightTextBox { get; }
        bool IsUsingTextStringAngleTextBox { get; }
        bool IsUsingLeaderSegmentTextBox { get; }
        bool IsUsingArrowHeadSizeTextBox { get; }
        bool IsUsingScaleFactorTextBox { get; }
        bool IsUsingMultilineTextBox { get; }
        bool IsUsingOffsetDistance { get; }
        bool IsUsingSwitchMode { get; }
        bool IsUsingInsideLength { get; }
        double CurrentWidth { get; set; }
        double CurrentHeight { get; set; }
        double CurrentAngle { get; set; }
        double ScaleFactor { get; set; }
        int InsideLengthDistance { get; }
        IJob JobModel { get; set; }
        UnderMouseDrawingType EntityUnderMouseDrawingType { get; }
        FocusType DefaultDynamicInputTextBoxToFocus { get; }
        IDynamicInputView DynamicInput { get;}
        void NotifyMouseMove(object sender, MouseEventArgs e);
        void NotifyMouseDown(object sender, MouseButtonEventArgs e);
        void NotifyMouseUp(object sender, MouseButtonEventArgs e);
        void OnJigging(object sender, DrawInteractiveArgs e);
        void NotifyPreviewKeyDown(object sender, KeyEventArgs e);
        void SetDynamicInput(IDynamicInputView dynamicInput);
        void SetLayersManager(ILayerManager layerManager);
        void SetEntitiesManager(IEntitiesManager entitiesManager);
    }
}