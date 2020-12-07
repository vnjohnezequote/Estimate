using System;
using System.Collections.Generic;
using System.ComponentModel;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using Block = devDept.Eyeshot.Block;


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
                    floorBlocks.Add(drawingsBlock);
                    ReCalculatorFloorBlockColor(drawingsBlock);
                }
            }
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

                            foreach (var beamEntityAttribute in beamEntity.Attributes)
                            {
                                //var beamText = new Text(Plane.XY, beamEntityAttribute.Value.InsertionPoint, beamEntityAttribute.Value, beamText.Height, beamEntity.Attributes["Name"].Alignment);
                                var beamText = new Text(Plane.XY, beamEntityAttribute.Value.InsertionPoint,
                                    beamEntityAttribute.Value.Value, beamEntityAttribute.Value.Height,
                                    beamEntityAttribute.Value.Alignment);
                                beamText.Color = beamEntityAttribute.Value.Color;
                                beamText.ColorMethod = colorMethodType.byEntity;
                                floorBlock.Entities.Add(beamText);
                            }
                            //if (beamEntity.BeamNameAttribute is Text beamText)
                            //{
                            //    var newBeamText = new Text(Plane.XY, beamText.InsertionPoint, beamEntity.Attributes["Name"].Value, beamText.Height, beamEntity.Attributes["Name"].Alignment);
                            //    newBeamText.Color = beamText.Color;
                            //    newBeamText.ColorMethod = colorMethodType.byEntity;
                            //    floorBlock.Entities.Add(newBeamText);
                            //}

                            continue;
                        }
                    case BlockReference _:
                        break;
                    case Dimension _:
                    case DoorCountEntity _:
                        continue;
                    case Beam2D beam:
                        var outerList = new List<Point2D>()
                            {beam.OuterStartPoint, beam.OuterEndPoint, beam.InnerEndPoint, beam.InnerStartPoint,beam.OuterStartPoint};
                        var beamQuad = Mesh.CreatePlanar(Plane.XY, outerList, Mesh.natureType.Plain);
                        beamQuad.Color = beam.Color;
                        beamQuad.ColorMethod = colorMethodType.byEntity;
                        floorBlock.Entities.Add(beamQuad);
                        continue;
                    case IRectangleSolid rectangle:
                        var outerList2 = new List<Point2D>()
                            {rectangle.OuterStartPoint, rectangle.OuterEndPoint, rectangle.InnerEndPoint, rectangle.InnerStartPoint,rectangle.OuterStartPoint};
                        //var quad = new Quad(Plane.XY, rectangle.OuterStartPoint, rectangle.OuterEndPoint, rectangle.InnerEndPoint,
                        //    rectangle.InnerStartPoint);
                        var quad = Mesh.CreatePlanar(Plane.XY, outerList2, Mesh.natureType.Plain);
                        quad.Color = rectangle.Color;
                        quad.ColorMethod = colorMethodType.byEntity;
                        floorBlock.Entities.Add(quad);
                        continue;
                    case Hanger2D hanger:
                        var hangerText = new Text(Plane.XY, hanger.InsertionPoint, hanger.TextString, hanger.Height,
                            hanger.Alignment);
                        hangerText.Color = hanger.Color;
                        hangerText.ColorMethod = colorMethodType.byEntity;
                        var hangerCirCle = new Circle(Plane.XY, hanger.InsertionPoint, hangerText.Height);
                        hangerCirCle.Color = hanger.Color;
                        hangerCirCle.ColorMethod = colorMethodType.byEntity;
                        floorBlock.Entities.Add(hangerText);
                        floorBlock.Entities.Add(hangerCirCle);
                        continue;
                }
                var cloneEntitiy = modelEntity.Clone() as Entity;
                if (cloneEntitiy != null)
                {
                    cloneEntitiy.Color = modelEntity.Color;
                    cloneEntitiy.ColorMethod = colorMethodType.byEntity;
                    cloneEntitiy.LineWeight = 0.6f;
                    cloneEntitiy.LineWeightMethod = colorMethodType.byEntity;
                    floorBlock.Entities.Add(cloneEntitiy);
                }

                floorBlock.Entities.Regen();
            }
        }
    }
}
