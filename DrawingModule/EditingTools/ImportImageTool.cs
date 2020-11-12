

using System.Drawing;
using System.Reactive.Joins;
using System.Windows.Forms;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace DrawingModule.EditingTools
{
    public class ImportImageTool: ToolBase
    {
        public override string ToolName => "Import Image";
        public override Point3D BasePoint { get; protected set; }
        private System.Drawing.Image _insertImage;
        private double _imgWidth;
        private double _imgHeight;

        //private PictureEntity _insertPick;
        //private Picture _importPicture;

        public ImportImageTool()
        {
            if (!Clipboard.ContainsImage()) return;
            _insertImage = Clipboard.GetImage();
            _imgWidth = _insertImage.Width;
            _imgHeight = _insertImage.Height;
            //_importPicture = new Picture(Plane.XY,100,100,img);
            //_importPicture = new Picture();
        }

        [CommandMethod("Img")]
        public void ImportImage()
        {
            var acadEditor = Application.Application.DocumentManager.MdiActiveDocument.Editor;
            if (_insertImage==null)
            {
                return;
            }
            var prompPointOptions = new PromptPointOptions("Click to ImportImage");
            ToolMessage = "Click to ImportImage";
            var resutl = acadEditor.GetPoint(prompPointOptions);
            if (resutl.Status == PromptStatus.OK)
            {
                var inserPoint = new Point3D(resutl.Value.X,resutl.Value.Y,-0.10);
                var picPlan = Plane.XY.Offset(-10);
                var insertPick = new PictureEntity(picPlan, inserPoint, _imgWidth, _imgHeight, _insertImage);
                
                EntitiesManager.AddAndRefresh(insertPick, LayerManager.SelectedLayer.Name);
            }
            else
            {
                return;
            }
        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveImportImage((ICadDrawAble)sender,e);
        }

        private void DrawInteractiveImportImage(ICadDrawAble canvas,DrawInteractiveArgs e)
        {
            if (e.CurrentPoint!= null && this._insertImage!=null)
            {

                DrawInteractiveUntilities.DrawRectangle(e.CurrentPoint,_imgWidth,_imgHeight,canvas);
                //_insertPick = new PictureEntity(Plane.XY,e.CurrentPoint,100,100,_insertImage);
                //DrawInteractiveUntilities.DrawCurveOrBlockRef(_insertPick,canvas);
            }
        }
    }
}
