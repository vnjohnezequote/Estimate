using System;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.ResponsiveData;
using DrawingModule.Application;
using DrawingModule.EditingTools;

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
                        foreach (var entitiesManagerSelectedEntity in EntitiesManager.SelectedEntities)
                        {
                            if (entitiesManagerSelectedEntity is BeamEntity beam)
                            {
                                if (JobModel!=null)
                                {
                                    LevelWall level = null;
                                    foreach (var jobModelLevel in JobModel.Levels)
                                    {
                                        if (jobModelLevel.LevelName == beam.LevelName)
                                        {
                                            level = jobModelLevel;
                                        }
                                    }

                                    if (level!=null)
                                    {
                                        if (level.RoofBeams.Contains(beam.BeamReference))
                                        {
                                            level.RoofBeams.Remove(beam.BeamReference);
                                        }
                                        var i = 1;
                                        foreach (var levelRoofBeam in level.RoofBeams)
                                        {
                                            levelRoofBeam.Id = i;
                                            i++;
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
                        }
                    }
                    return true;
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return true;
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
