using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Documents;
using System.Linq;
using AppModels.CustomEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using Block = devDept.Eyeshot.Block;
using Environment = devDept.Eyeshot.Environment;
using ProgressChangedEventArgs = devDept.Eyeshot.ProgressChangedEventArgs;


namespace AppModels
{
    public class MyViewBuilder: ViewBuilder, IDisposable
    {
        private Model _model;
        private Drawings _drawings;
        public MyViewBuilder(Model model, Drawings drawings, bool dirtyOnly = false, operatingType operatingType = operatingType.Standard) : base(model, drawings, dirtyOnly, operatingType)
        {
            _model = model;
            _drawings = drawings;
        }

        protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs doWorkEventArgs)
        {
            foreach (var modelLineType in _model.LineTypes)
            {
                if (_drawings.LineTypes.Contains(modelLineType))
                {
                    continue;
                }

                _drawings.LineTypes.Add(modelLineType);
            }

            foreach (var modelLayer in _model.Layers)
            {
                if (_drawings.Layers.Contains(modelLayer))
                {
                    continue;
                }

                _drawings.Layers.Add(modelLayer);
            }
            base.DoWork(worker, doWorkEventArgs);
        }

        public new void AddToDrawings(Drawings drawings)
        {
            base.AddToDrawings(drawings);
            var numberOfFloor = drawings.Blocks.Count;
            List<Block> floorBlocks = new List<Block>();
            foreach (var drawingsBlock in drawings.Blocks)
            {
                if (drawingsBlock.Name.Contains("View"))
                {
                    //floorBlocks.Add(drawingsBlock);
                    ReCalculatorFloorBlockColor(drawingsBlock);
                }
            }
            //foreach (var floorBlock in floorBlocks)
            //{
               
            //}
        }

        private void ReCalculatorFloorBlockColor(Block floorBlock)
        {
            floorBlock.Entities.Clear();
            foreach (var modelEntity in _model.Entities)
            {
                switch (modelEntity)
                {
                    case BeamEntity beamEntity:
                        {
                            var beamLine = beamEntity.BeamLine.Clone() as Entity;
                            if (beamLine != null)
                            {
                                beamLine.LineWeight = 0.7f;
                                beamLine.LineWeightMethod = colorMethodType.byEntity;
                                beamLine.LineTypeScale = 0.5f;
                                floorBlock.Entities.Add(beamLine);
                            }

                            var beamLeader = beamEntity.BeamLeader.Clone() as Entity;
                            if (beamLeader != null)
                            {
                                beamLeader.LineWeight = 0.3f;
                                beamLeader.LineWeightMethod = colorMethodType.byEntity;
                                floorBlock.Entities.Add(beamLeader);
                            }

                            if (beamEntity.BeamNameAttribute is Text beamText)
                            {
                                var newBeamText = new Text(Plane.XY, beamText.InsertionPoint, beamEntity.Attributes["Name"].Value, beamText.Height, beamEntity.Attributes["Name"].Alignment);
                                newBeamText.Color = beamText.Color;
                                newBeamText.ColorMethod = colorMethodType.byEntity;
                                floorBlock.Entities.Add(newBeamText);
                            }

                            continue;
                        }
                    case BlockReference _:
                        continue;
                }

                var cloneEntitiy = modelEntity.Clone() as Entity;
                if (cloneEntitiy != null)
                {
                    cloneEntitiy.LineWeight = 0.6f;
                    cloneEntitiy.LineWeightMethod = colorMethodType.byEntity;
                    floorBlock.Entities.Add(cloneEntitiy);
                }

                floorBlock.Entities.Regen();
            }
        }
    }
}
