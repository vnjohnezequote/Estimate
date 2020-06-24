using System.Windows.Input;
using ApplicationService;
using DrawingModule.Application;

namespace DrawingModule.CustomControl.CanvasControl
{
    public partial class CanvasDrawing
    {
        private event KeyEventHandler PreviewKeybordDown_Drawing;
        public bool PreProcessKeyDownForCanvasView(KeyEventArgs e)
        {
            if (this.CurrentTool != null)
            {
                PreviewKeybordDown_Drawing?.Invoke(this,e);
            }

            if (e.Handled == true)
            {
                return true;
            }
            switch (e.Key)
            {
                case Key.Enter:
                    if (this.IsProcessingTool)
                    {
                        this.Focus();
                        if (this._waitingForSelection && this._selectTool.SelectedEntities.Count > 0)
                        {
                            this.IsUserInteraction = true;
                            this._waitingForSelection = false;
                            this.PromptStatus = PromptStatus.OK;
                        }
                        else if (!this._waitingForSelection)
                        {
                            this.PromptStatus = PromptStatus.OK;
                            this.IsUserInteraction = true;
                        }
                        _dynamicInput?.FocusDynamicInputTextBox(CurrentTool.DefaultDynamicInputTextBoxToFocus);
                    }
                    return false;
                case Key.Delete:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return true;
                case Key.Escape:
                    this.Focus();
                    this.ProcessCancelTool();
                    return false;
                default:
                    return this.IsProcessingTool;
            }
        }
    }
}
