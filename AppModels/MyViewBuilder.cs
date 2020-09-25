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

            //var line = new Line(new Point3D(0, 0, 0), new Point3D(10000, 0, 0));
            //line.Color = Color.Blue;
            //line.ColorMethod = colorMethodType.byEntity;
            //line.LineWeight = 0.5f;
            //line.LineWeightMethod = colorMethodType.byEntity;
            //drawings.Blocks[2].Entities.Add(line);
            //var line = new Line(new Point3D(0,0,0),new Point3D(0,10,100));
            //line.Color = Color.Blue;
            //line.ColorMethod = colorMethodType.byEntity;
            //line.LineWeight = 0.1f;
            //line.LineWeightMethod = colorMethodType.byEntity;
            //drawings.Blocks[2].Entities.Add(line);
            var numberOfFloor = drawings.Blocks.Count;
            List<Block> floorBlocks = new List<Block>();
            switch (numberOfFloor)
            {
                case 3:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    break;
                case 5:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    floorBlocks.Add(drawings.Blocks[4]);
                    break;
                case 7:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    floorBlocks.Add(drawings.Blocks[4]);
                    floorBlocks.Add(drawings.Blocks[6]);
                    break;
                case 9:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    floorBlocks.Add(drawings.Blocks[4]);
                    floorBlocks.Add(drawings.Blocks[6]);
                    floorBlocks.Add(drawings.Blocks[8]);
                    break;
                case 11:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    floorBlocks.Add(drawings.Blocks[4]);
                    floorBlocks.Add(drawings.Blocks[6]);
                    floorBlocks.Add(drawings.Blocks[8]);
                    floorBlocks.Add(drawings.Blocks[10]);
                    break;
                case 13:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    floorBlocks.Add(drawings.Blocks[4]);
                    floorBlocks.Add(drawings.Blocks[6]);
                    floorBlocks.Add(drawings.Blocks[8]);
                    floorBlocks.Add(drawings.Blocks[10]);
                    floorBlocks.Add(drawings.Blocks[12]);
                    break;
                case 15:
                    floorBlocks.Clear();
                    floorBlocks.Add(drawings.Blocks[2]);
                    floorBlocks.Add(drawings.Blocks[4]);
                    floorBlocks.Add(drawings.Blocks[6]);
                    floorBlocks.Add(drawings.Blocks[8]);
                    floorBlocks.Add(drawings.Blocks[10]);
                    floorBlocks.Add(drawings.Blocks[12]);
                    floorBlocks.Add(drawings.Blocks[14]);
                    break;
                default:
                    break;
                
            }

            foreach (var floorBlock in floorBlocks)
            {
                floorBlock.Entities.Clear();
                //var listPicEntities = new List<PictureEntity>();
                //foreach (var modelEntity in _model.Entities)
                //{
                //    if (modelEntity is PictureEntity pictureEntity)
                //    {
                //        listPicEntities.Add(pictureEntity);
                //        //_model.Entities.Remove(modelEntity);
                //    }
                //}

                //foreach (var pictureEntity in listPicEntities)
                //{
                //    _model.Entities.Remove(pictureEntity);
                //}

                //_model.Entities.AddRange(listPicEntities);
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
                                beamLine.LineTypeScale = 1;
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
                                var newBeamText = new Text(Plane.XY, beamText.InsertionPoint, beamEntity.Attributes["Name"].Value,beamText.Height,beamText.Alignment);
                                newBeamText.Color = beamText.Color;
                                newBeamText.ColorMethod = colorMethodType.byEntity;
                                //var beamTextMesh = newBeamText.ConvertToMesh(_drawings);
                                floorBlock.Entities.Add(newBeamText);
                                //floorBlock.Entities.AddRange(beamTextMesh);
                            }

                            continue;
                        }
                        case BlockReference _:
                            continue;
                    }

                    var cloneEntitiy = modelEntity.Clone() as Entity;
                    //if (cloneEntitiy is PictureEntity pictureEntity)
                    //{
                    //    listPicEntities.Add(pictureEntity);
                    //    continue;
                    //}
                    cloneEntitiy.LineWeight = 0.8f;
                    cloneEntitiy.LineWeightMethod = colorMethodType.byEntity;
                    floorBlock.Entities.Add(cloneEntitiy);
                    floorBlock.Entities.Regen();
                }

                //foreach (var pictureEntity in listPicEntities)
                //{
                //    floorBlock.Entities.AddRange(listPicEntities);
                //}
            }

            //if (drawings.Blocks[2]!=null)
            //{
            //   //drawings.Blocks[2].Entities.Clear();
            //    //foreach (var modelEntity in _model.Entities)
            //    //{
            //    //    var layoutEntity = modelEntity.Clone() as Entity;
            //    //    //layoutEntity.LayerName = "";
            //    //    //layoutEntity.Color = modelEntity.Color;
            //    //    //layoutEntity.ColorMethod = colorMethodType.byEntity;
            //    //    //layoutEntity.LineWeight = 0.8f;
            //    //    //layoutEntity.LineWeightMethod = colorMethodType.byEntity;
            //    //    drawings.Blocks[2].Entities.Add(layoutEntity);
            //    //}
            //    //foreach (var entity in drawings.Blocks[2].Entities)
            //    //{
            //    //    entity.Color= Color.Blue;
            //    //    entity.ColorMethod = colorMethodType.byEntity;
            //    //}
            //}

        }
    }
}
