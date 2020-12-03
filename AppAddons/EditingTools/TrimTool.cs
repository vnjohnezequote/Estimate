using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using GeometryGym.Ifc;

namespace AppAddons.EditingTools
{
    public class TrimTool : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Trim";
        private Entity _trimedEntity;
        private Point3D _trimPoint;
        private bool _processingTool;
        private Entity _firstSelectedEntity;
        private Entity _secondSelectedEntity;
        private int _selectedEntityIndex;
        private List<Entity> _selectedEntities;
        private List<Entity> _boundingEntities;
        private List<Point3D> _trimmedPoints;
        //private bool _waitingForSelection = true;
        public TrimTool()
        {
            _selectedEntities = new List<Entity>();
            _boundingEntities = new List<Entity>();
            _trimmedPoints = new List<Point3D>();
            _processingTool = true;
            //_waitingForSelection = true;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
            
            //_selectedEntityIndex = -1;
        }

        [CommandMethod("Trim")]
        public void Trim()
        {
            OnProcessCommand();
        }
        protected virtual void OnProcessCommand()
        {
            while (_processingTool)
            {

            }

        }

       public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveTrim((ICadDrawAble)sender, e);
        }

        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this._processingTool = false;
                e.Handled = true;
            }
        }

        private void DrawInteractiveTrim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            
            


        }
    }
}
