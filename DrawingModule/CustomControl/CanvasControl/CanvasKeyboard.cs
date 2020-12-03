using System;
using System.Collections.Generic;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.ViewModelEntity;
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

                            if (entitiesManagerSelectedEntity is Joist2D joist)
                            {
                                // Cho nay can viet lai de tim dung floor sheet cua joist de xoa trong truong hop active floor sheet khong phai laf floor sheet cua joist chon xoa
                                var activesheet = joist.FramingReference.FramingSheet;
                                var joistVm = (Joist2dVm)joist.CreateEntityVm(EntitiesManager);
                                    joistVm.IsHangerA = false;
                                    joistVm.IsHangerB = false;
                                    joistVm.IsOutriggerA = false;
                                    joistVm.IsOutriggerB = false;
                                    if(activesheet.Joists.Contains(joist.FramingReference))
                                        activesheet.Joists.Remove(joist.FramingReference);
                            }

                            if (entitiesManagerSelectedEntity is Beam2D beam2D)
                            {
                                var activesheet = beam2D.FramingReference.FramingSheet;
                                if (activesheet.Beams.Contains(beam2D.FramingReference))
                                    activesheet.Beams.Remove(beam2D.FramingReference);
                                var beamVm = (Beam2DVm) beam2D.CreateEntityVm(EntitiesManager);
                                beamVm.IsHangerA = false;
                                beamVm.IsHangerB = false;
                                beamVm.IsOutriggerA = false;
                                beamVm.IsOutriggerB = false;
                            }

                            if (entitiesManagerSelectedEntity is Hanger2D hangert)
                            {
                                Joist2D joistHangerBelongTo=null;
                                Beam2D beamHangerBelongTo = null;
                                foreach ( var entity in EntitiesManager.Entities)
                                {
                                    if (entity is Joist2D joistt && (joistt.HangerAId == hangert.Id||joistt.HangerBId== hangert.Id))
                                    {
                                        joistHangerBelongTo = joistt;
                                    }
                                    if (entity is Beam2D beamtt && (beamtt.HangerAId == hangert.Id || beamtt.HangerBId == hangert.Id))
                                    {
                                        beamHangerBelongTo = beamtt;
                                    }
                                }
                                if (joistHangerBelongTo!=null)
                                {
                                    var joistVm = (Joist2dVm)joistHangerBelongTo.CreateEntityVm(EntitiesManager);
                                    if (joistVm.HangerA == hangert)
                                    {
                                        joistVm.IsHangerA = false;
                                    }
                                    else if(joistVm.HangerB == hangert)
                                    {
                                        joistVm.IsHangerB = false;
                                    }
                                }

                                if (beamHangerBelongTo!=null)
                                {
                                    var beamVm = (Beam2DVm)beamHangerBelongTo.CreateEntityVm(EntitiesManager);
                                    if (beamVm.HangerA == hangert)
                                    {
                                        beamVm.IsHangerA = false;
                                    }
                                    else if (beamVm.HangerB == hangert)
                                    {
                                        beamVm.IsHangerB = false;
                                    }
                                }
                            }

                            if (entitiesManagerSelectedEntity is OutTrigger2D outTrigger2D)
                            {
                                Joist2D joistOutTriggerBelongTo = null;
                                Beam2D beamOutTriggerBelongTo = null;
                                foreach (var entity in EntitiesManager.Entities)
                                {
                                    if (entity is Joist2D joistt && (joistt.OutTriggerA == outTrigger2D|| joistt.OutTriggerB ==outTrigger2D))
                                    {
                                        joistOutTriggerBelongTo = joistt;
                                    }
                                    if (entity is Beam2D beamtt && (beamtt.OutTriggerA == outTrigger2D || beamtt.OutTriggerB == outTrigger2D))
                                    {
                                        beamOutTriggerBelongTo = beamtt;
                                    }

                                }

                                if (joistOutTriggerBelongTo != null)
                                {
                                    var joistVm = (Joist2dVm)joistOutTriggerBelongTo.CreateEntityVm(EntitiesManager);
                                    if (joistVm.OutTriggerA == outTrigger2D)
                                    {
                                        joistVm.IsOutriggerA = false;
                                    }
                                    else if (joistVm.OutTriggerB == outTrigger2D)
                                    {
                                        joistVm.IsOutriggerB = false;
                                    }
                                }

                                if (beamOutTriggerBelongTo != null)
                                {
                                    var beamVm = (Beam2DVm)beamOutTriggerBelongTo.CreateEntityVm(EntitiesManager);
                                    if (beamVm.OutTriggerA == outTrigger2D)
                                    {
                                        beamVm.IsOutriggerA = false;
                                    }
                                    else if ((beamVm.OutTriggerB == outTrigger2D))
                                    {
                                        beamVm.IsOutriggerB = false;
                                    }
                                }
                            }

                            if (entitiesManagerSelectedEntity is Blocking2D)
                            {
                                foreach (var entity in EntitiesManager.SelectedEntities)
                                {
                                    if (entity is Blocking2D blocking)
                                    {
                                        blocking.FramingReference.FramingSheet.Blockings.Remove((Blocking)blocking
                                            .FramingReference);
                                    }
                                }

                            }
                        }
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
