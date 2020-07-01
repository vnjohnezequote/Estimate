

using System.Windows.Forms;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace DrawingModule.EditingTools
{
    public class ImportImageTool: ToolBase
    {
        public override string ToolName => "Import Image";
        public override Point3D BasePoint { get; protected set; }
        private System.Drawing.Image _insertImage;
        //private Picture _importPicture;

        public ImportImageTool()
        {
            if (!Clipboard.ContainsImage()) return;
            _insertImage = Clipboard.GetImage();
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
                var inserPoint = resutl.Value;
                var insertPic = new Picture(Plane.XY, inserPoint,100,100,_insertImage);
                
                EntitiesManager.AddAndRefresh(insertPic,LayerManager.SelectedLayer.Name);
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

        }
    }
}
