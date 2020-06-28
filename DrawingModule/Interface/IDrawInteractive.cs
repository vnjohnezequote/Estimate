using System;
using System.Web.UI;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.EventArgs;
using DrawingModule.Views;

namespace DrawingModule.Interface
{
    internal interface IDrawInteractive
    {
        //DrawInteractiveDelegate DrawInteractiveHandler { get; }
        //event EventHandler ToolMessageChanged;
        string ToolName { get; }
        string ToolMessage { get; }
        bool IsSnapEnable { get; }
        Point3D BasePoint { get; }
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
        FocusType DefaultDynamicInputTextBoxToFocus { get; }
        IDynamicInputView DynamicInput { get;}
        void NotifyMouseMove(object sender, MouseEventArgs e);
        void NotifyMouseDown(object sender, MouseButtonEventArgs e);
        void NotifyMouseUp(object sender, MouseButtonEventArgs e);
        void OnJigging(object sender, DrawInteractiveArgs e);
        void NotifyPreviewKeyDown(object sender, KeyEventArgs e);
        void SetDynamicInput(IDynamicInputView dynamicInput);
    }
}