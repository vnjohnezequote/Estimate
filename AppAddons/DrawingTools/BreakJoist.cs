using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class BreakJoist : ToolBase
    {
        private bool _watingForSelection;
        private List<FramingRectangle2D> _framingList = new List<FramingRectangle2D>();
        private Beam2D _breakFraming;
        private Line _lineBreak;
        public override string ToolName => "Break Joist";
        public override Point3D BasePoint { get; protected set; }

        public BreakJoist()
        {
            _watingForSelection = true;
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }

        [CommandMethod("BreakJ")]
        public void BreakJ()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (true)
            {
                ToolMessage = "Please select Joist To Break";
                var promptEntities = new PromptEntityOptions();
                var result = acDoc.Editor.GetEntities(promptEntities,true);
                if (result.Status == PromptStatus.OK)
                {
                    foreach (var resultEntity in result.Entities)
                    {
                        if (resultEntity is FramingRectangle2D framingRect)
                        {
                            if (!(resultEntity is OutTrigger2D))
                            {
                                _framingList.Add(framingRect);
                            }
                        }
                    }

                    if (_framingList.Count==0)
                    {
                        continue;
                    }
                }

                while (true)
                {
                    ToolMessage = "Please select Line/Beam to break";
                    var promtSelect = new PromptSelectionOptions();
                    var resultEnt = acDoc.Editor.GetSelection(promtSelect);
                    if (resultEnt.Status == PromptStatus.OK)
                    {
                        if (resultEnt.Value is Line line)
                        {
                            _lineBreak = line;
                            break;
                        }
                        else if (resultEnt.Value is Beam2D beam)
                        {
                            _breakFraming = beam;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                

                
            }
        }

    }
}
