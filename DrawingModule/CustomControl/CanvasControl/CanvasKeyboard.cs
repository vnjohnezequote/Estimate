using System;
using System.Linq;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;

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
                        if (this.CurrentTool!= null && this.CurrentTool.IsUsingMultilineTextBox)
                        {
                            if (DynamicInput !=null && DynamicInput.PreviusDynamicInputFocus == FocusType.TextContent)
                            {
                                if (this.CurrentTool is ITextTool textTool)
                                {
                                    textTool.TextInput += Environment.NewLine;
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            this.Focus();
                            if (this._waitingForSelection && this.EntitiesManager.SelectedEntities.Count > 0)
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
                            //this.Dispatcher.Invoke((Action)(() =>
                            //{//this refer to form in WPF application 

                            DynamicInput?.FocusDynamicInputTextBox(CurrentTool.DefaultDynamicInputTextBoxToFocus);
                            //}));
                        }


                    }
                    return false;
                case Key.Space:
                    if (!this.IsProcessingTool) return true;
                    var drawInteractive = this.CurrentTool;
                    if (drawInteractive == null || !drawInteractive.IsUsingSwitchMode) return true;
                    this.PromptStatus = PromptStatus.SwitchMode;
                    IsUserInteraction = true;
                    this._waitingForPickSelection = false;
                    return true;
                case Key.Delete:
                    if (this.EntitiesManager.SelectedEntities!=null && this.EntitiesManager.SelectedEntities.Count!=0)
                    {
                        var undoList = new UndoList() {ActionType = ActionTypes.Remove};
                        foreach (var entitiesManagerSelectedEntity in EntitiesManager.SelectedEntities)
                        {
                            var backup = BackupEntitiesFactory.CreateBackup(entitiesManagerSelectedEntity, undoList,EntitiesManager);
                            backup?.Backup();
                            if (entitiesManagerSelectedEntity is BeamEntity beam)
                            {
                                var curentlevel = beam.FramingReference.Level;
                                
                                   if (curentlevel!=null)
                                   {
                                       if (curentlevel.RoofBeams.Contains(beam.FramingReference))
                                       {
                                           var index = curentlevel.RoofBeams.IndexOf(beam.FramingReference);
                                           curentlevel.RoofBeams.Remove(beam.FramingReference);
                                           for (int i = index; i < curentlevel.RoofBeams.Count; i++)
                                           {
                                               curentlevel.RoofBeams[i].Index = i + 1;
                                           }

                                    }
                                       
                                   }
                                   this.Blocks.Remove(beam.BeamBlock);
                            }
                            if (entitiesManagerSelectedEntity is DoorCountEntity doorCount)
                            {
                                if (JobModel != null)
                                {
                                    LevelWall level = null;
                                    foreach (var jobModelLevel in JobModel.Levels)
                                    {
                                        if (jobModelLevel.LevelName == doorCount.LevelName)
                                        {
                                            level = jobModelLevel;
                                        }
                                    }

                                    if (level != null)
                                    {
                                        if (level.Openings.Contains(doorCount.DoorReference))
                                        {
                                            level.Openings.Remove(doorCount.DoorReference);
                                        }
                                        var i = 1;
                                        foreach (var door in level.Openings)
                                        {
                                            door.Id = i;
                                            i++;
                                        }
                                    }

                                }
                            }
                            if (entitiesManagerSelectedEntity is Wall2D wall)
                            {
                                this.EntitiesManager.Walls.Remove(wall);
                            }
                            if (entitiesManagerSelectedEntity is FramingRectangleContainHangerAndOutTrigger framing)
                            {
                                if (framing.FramingName!=null)
                                {
                                    EntitiesManager.Entities.Remove(framing.FramingName);
                                }

                                var hangerController = new HangerControler(EntitiesManager, framing);
                                hangerController.RemoveHangerA();
                                hangerController.RemoveHangerB();
                                var outriggerController = new OutTriggerController(EntitiesManager, framing);
                                outriggerController.RemoveOutTriggerA();
                                outriggerController.RemoveOutTriggerB();
                            }
                            if (entitiesManagerSelectedEntity is Joist2D joist)
                            {
                                var activesheet = joist.FramingReference.FramingSheet;
                                if (activesheet.Joists.Contains(joist.FramingReference))
                                        activesheet.Joists.Remove(joist.FramingReference);
                                AppModels.Helper.RegenerationFramingName(activesheet.Joists.ToList());
                            }
                            if (entitiesManagerSelectedEntity is Beam2D beam2D)
                            {
                                if(beam2D.FramingReference!=null)
                                {
                                    var activesheet = beam2D.FramingReference.FramingSheet;
                                    if (activesheet.Beams.Contains(beam2D.FramingReference))
                                        activesheet.Beams.Remove(beam2D.FramingReference);
                                    AppModels.Helper.RegenerationFramingName(activesheet.Beams.ToList());
                                }
                                
                            }
                            if (entitiesManagerSelectedEntity is Hanger2D hanger2D)
                            {
                                var hangerController = new HangerControler(EntitiesManager, hanger2D.Framing2D);
                                if (hanger2D.Framing2D.HangerA == hanger2D)
                                {
                                    hangerController.RemoveHangerA();
                                }

                                if (hanger2D.Framing2D.HangerB == hanger2D)
                                {
                                    hangerController.RemoveHangerB();
                                }
                            }
                            if (entitiesManagerSelectedEntity is OutTrigger2D outTrigger2D)
                            {
                                var outTriggerController =
                                    new OutTriggerController(EntitiesManager, outTrigger2D.Framing2D);
                                if (outTrigger2D.Framing2D.OutTriggerA == outTrigger2D)
                                {
                                    outTriggerController.RemoveOutTriggerA();
                                }

                                if (outTrigger2D.Framing2D.OutTriggerB == outTrigger2D)
                                {
                                    outTriggerController.RemoveOutTriggerB();
                                }
                            }
                            if (entitiesManagerSelectedEntity is Blocking2D blocking2D)
                            {
                                var activeSheet = blocking2D.FramingReference.FramingSheet;
                                activeSheet.Blockings.Remove(blocking2D.FramingReference);
                                AppModels.Helper.RegenerationFramingName(activeSheet.Blockings.ToList());
                            }
                            if (entitiesManagerSelectedEntity is FramingNameEntity framingName)
                            {
                                JoistArrowEntity dependentJoistArrow = null;
                                foreach (var entity in EntitiesManager.Entities)
                                {
                                    if (entity is IFraming2DContaintHangerAndOutTrigger framingContainName)
                                    {
                                        if (framingContainName.FramingName == framingName)
                                        {
                                            framingContainName.FramingNameId = null;
                                            framingContainName.FramingName = null;
                                            framingContainName.IsShowFramingName = false;
                                            break;
                                        }
                                    }
                                    else if (entity is JoistArrowEntity joistArrow)
                                    {
                                        if (joistArrow.FramingName == framingName)
                                        {
                                            dependentJoistArrow =joistArrow ;
                                            break;
                                        }
                                    }
                                }
                                if (dependentJoistArrow != null)
                                {
                                    EntitiesManager.Entities.Remove(dependentJoistArrow);
                                }


                            }
                        }
                        UndoEngineer.SaveSnapshot(undoList);
                        EntitiesManager.SelectedEntities.Clear();
                        EntitiesManager.SelectedEntity = null;
                    }
                    return true;
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return true;
                case Key.Z:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        this.ProcessCommand("Undo");
                        return true;
                    }

                    return false;
                case Key.V:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        this.ProcessCommand("Img");
                        return true;
                    }
                    return false;
                case Key.Escape:
                    this.Focus();
                    this.ProcessCancelTool();
                    return false;
                //case Key.Tab:
                //    if (this.CurrentTool!=null)
                //    {
                //        return true;
                //    }

                //    if (this.CurrentTool == null && this.IsFocused)
                //    {
                //        return false;
                //    }

                //    return true;
                default:
                    return this.IsProcessingTool;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (this.EntitiesManager!=null)
            {
                this.EntitiesManager.NotifyEntitiesListChanged();
            }
        }
    }
}
