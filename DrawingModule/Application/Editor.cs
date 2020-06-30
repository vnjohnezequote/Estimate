using System.Windows;
using DrawingModule.CustomControl;
using DrawingModule.CustomControl.PaperSpaceControl;
using DrawingModule.Interface;
using DrawingModule.UserInteractive;
using DrawingModule.ViewModels;
using DrawingModule.Views;
using Syncfusion.Windows.PdfViewer;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.Application
{
    public sealed class Editor
    {
        #region Field
        private CanvasDrawing _canvasDrawing;
        private DynamicInputView _dynamicInput;
        private PaperSpaceDrawing _paperSpace;
        private CanvasDrawingView _rootCanvas;
        private DynamicInputViewModel _dynamicInputViewModel;
        #endregion

        #region properties
        public CanvasDrawing CanvasDrawing => _canvasDrawing;
        public DynamicInputView DynamicInput => _dynamicInput;
        public CanvasDrawingView RootCanvas => _rootCanvas;
        #endregion
        #region Constructor
        #endregion
        #region public method

        public void FocusToLengthTextBox()
        {
            if (this._dynamicInput == null) return;
            if (_dynamicInput.TextLength.Visibility == Visibility.Visible)
            {
               // _dynamicInput.FocusTextLength();
            }
        }
        public PromptPointResult GetPoint(PromptPointOptions options)
        {
            return (PromptPointResult)DoPrompt(options);
        }

        public PromptEntityResult GetEntities(PromptEntityOptions options)
        {
            return (PromptEntityResult) DoPrompt(options);
        }

        public PromptSelectionResult GetSelection(PromptSelectionOptions options)
        {
            return (PromptSelectionResult) DoPrompt(options);
        }

        public void ResetProcessTool()
        {
            this._dynamicInputViewModel.ResetProcesscingTool();
            this._canvasDrawing.ResetProcessingTool();
        }

        public void InitializeProcessTool()
        {

        }
        public void InitializingEditor(CanvasDrawing canvas, DynamicInputViewModel dynamicInputViewModel, PaperSpaceDrawing paperSpace, CanvasDrawingView rootView, DynamicInputView dynamicInputView)
        {
            this._canvasDrawing = canvas;
            this._paperSpace = paperSpace;
            this._dynamicInputViewModel = dynamicInputViewModel;
            this._dynamicInput = dynamicInputView;
            this._rootCanvas = rootView;
        }
        //public void RegisterDrawInteractive(IDrawInteractive drawInteractiveObject)
        //{
        //    this.CanvasDrawing.RegisterDrawInteractive(drawInteractiveObject);
        //}
        //public void UnRegisterDrawInteractive(IDrawInteractive drawInteractiveObject)
        //{
        //    this.CanvasDrawing.UnRegisterDrawInteractive(drawInteractiveObject);
        //}
        #endregion


        //public PromptResult GetString(string message)
        //{
        //    //return GetString(new PromptStringOptions(message));
        //}
        //public PromptResult GetString(PromptStringOptions options)
        //{
        //    return DoPrompt(options);
        //}


        public PromptResult DoPrompt(PromptOptions opt)
        {
            PromptResult result;
            result = opt.DoIt(this._canvasDrawing, this._dynamicInputViewModel);
            return result;
        }

        public void AddControlToUi(FrameworkElement control)
        {

        }

        public void WriteMessage(string message)
        {
            //this.Document.CommandEditor.PromptAndInput.Prompt = message;
        }
    }
}
