using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using AppModels.CustomEntity;
using AppModels.ExportData.FrameData;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using Attribute = devDept.Eyeshot.Entities.Attribute;
using Block = devDept.Eyeshot.Block;


namespace AppModels
{
    public class MyViewBuilder: ViewBuilder
    {
        private Model _model;
        private Drawings _drawings;
        private BackgroundWorker _backgroundWorker;
        private IJob _jobModel;
        private Dictionary<Sheet, WallName> _wallNamesDict;
        private Dictionary<WallName, Block> _wallNameBlockDict;
        private Dictionary<Sheet, FloorQuantity> _floorQuantitiesDict;
        private Dictionary<FloorQuantity, Block> _floorQuantitiesBlockDict;
        
        //public MyViewBuilder(Model model, Drawings drawings, bool dirtyOnly = false, operatingType operatingType = operatingType.Standard) : base(model, drawings, dirtyOnly, operatingType)
        //{
        //    _model = model;
        //    _drawings = drawings;
        //}

        public MyViewBuilder(Model model, Drawings drawing, View view, Sheet sheet,IJob jobModel) : base(model, drawing, view, sheet)
        {
            _model = model;
            _drawings = drawing;
            _jobModel = jobModel;
            _wallNamesDict = InitWallNameListDict(drawing);
            _floorQuantitiesDict = InitFloorQuantitiesListDict(drawing);

        }

        private static Dictionary<Sheet, FloorQuantity> InitFloorQuantitiesListDict(Drawings drawings)
        {
            var floorQuantitiesDict = new Dictionary<Sheet, FloorQuantity>();
            foreach (var sheet in drawings.Sheets)
            {
                foreach (var sheetEntity in sheet.Entities)
                {
                    var floorQuantity = sheetEntity as FloorQuantity;
                    if (floorQuantity!=null)
                    {
                        if(!floorQuantitiesDict.ContainsKey(sheet))
                        floorQuantitiesDict.Add(sheet,floorQuantity);
                    }
                }
            }
            return floorQuantitiesDict;
        }
        
        private static Dictionary<Sheet, WallName> InitWallNameListDict(Drawings drawings)
        {
            var wallNamesDict = new Dictionary<Sheet, WallName>();
            foreach (var sheet in drawings.Sheets)
            {
                foreach (var sheetEntity in sheet.Entities)
                {
                    var wallName = sheetEntity as WallName;
                    if (wallName != null)
                    {
                        if(!wallNamesDict.ContainsKey(sheet))
                        wallNamesDict.Add(sheet, wallName);
                    }
                }
            }

            return wallNamesDict;

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
            _wallNameBlockDict = new Dictionary<WallName, Block>();
            _backgroundWorker = worker;
            foreach (var wallName in _wallNamesDict)
            {
                var wallRef = wallName.Value;
                var wallSheet = wallName.Key;
                var wallBlock = GeneralWallList(wallRef, wallSheet, worker);
                if (wallBlock != null)
                {
                    _wallNameBlockDict.Add(wallRef, wallBlock);
                }

                if (base.Cancelled(worker, null))
                {
                    break;
                }
            }

            _floorQuantitiesBlockDict = new Dictionary<FloorQuantity, Block>();
            foreach (var floorQuantity in _floorQuantitiesDict)
            {
                var floorQtyRef = floorQuantity.Value;
                var floorSheet = floorQuantity.Key;
                var floorQtyBlock = GeneralFloorQtyBlock(floorQtyRef, floorSheet, worker);
                if (floorQtyBlock!=null)
                {
                    _floorQuantitiesBlockDict.Add(floorQtyRef,floorQtyBlock);
                }

                if (base.Cancelled(worker,null))
                {
                    break;
                }
            }
            
            base.DoWork(worker, doWorkEventArgs);

        }

        private Block GeneralFloorQtyBlock(FloorQuantity floorQty, Sheet floorSheet, BackgroundWorker worker)
        {
            if (!_drawings.TextStyles.Contains("TotalHeader"))
            {
                _drawings.TextStyles.Add(new TextStyle("TotalHeader", "Areal", FontStyle.Bold));

            }
            Block floorQTyBlock;
            floorQTyBlock = base.Cancelled(worker, null) ? null : GeneralFloorQtyList(floorQty, floorSheet,worker);
            return floorQTyBlock;
        }
        private Block GeneralWallList(WallName wallName, Sheet wallSheet, BackgroundWorker worker)
        {
            Block block;
            block = base.Cancelled(worker, null) ? null : GeneralWallListStart(wallName, wallSheet, worker);
            return block;
        }

        private Block GeneralFloorQtyList(FloorQuantity floorQty,Sheet floorSheet,BackgroundWorker worker)
        {
            Block block = new Block(floorQty.BlockName);
            if (this._jobModel == null || _jobModel.Levels==null )
            {
                return null;
            }

            FramingSheet framingSheet = null;
            foreach (var jobModelLevel in _jobModel.Levels)
            {
                foreach (var sheet in jobModelLevel.FramingSheets)
                {
                    if (floorQty.BlockName.Contains(sheet.FloorName))
                    {
                        framingSheet = sheet;
                    }
                }
            }

            if (framingSheet!=null)
            {
                var frameJob = new FrameJob(_jobModel, framingSheet);
                frameJob.CreateTable(block);
            }
            if (base.Cancelled(worker, null))
            {
                return null;
            }

            return block;

        }
        private Block GeneralWallListStart(WallName wallName, Sheet wallSheet, BackgroundWorker worker)
        {
            Block block = new Block(wallName.BlockName);
            if (this._jobModel == null || _jobModel.Levels == null)
            {
                return null;
            }

            LevelWall level = null;
            foreach (var jobModelLevel in _jobModel.Levels)
            {
                if (wallName.BlockName.Contains(jobModelLevel.LevelName))
                {
                    level = jobModelLevel;
                }
            }

            if (level != null)
            {
                if (level.WallLayers == null || level.WallLayers.Count == 0)
                {
                    return null;
                }

                var lineSpace = 0d;
                var wallFloorName = new Attribute(0, lineSpace, 0, "Title", "", 3.5);
                //var wallFloorName = new Text(0,lineSpace,level.LevelName + ":",3.5);
                lineSpace -= 6;
                wallFloorName.ColorMethod = colorMethodType.byEntity;
                wallFloorName.Color = Color.Black;
                //var wallBoxMax = wallFloorName.BoxMax;
                //var wallBoxMin = wallFloorName.BoxMin;
                if (!_drawings.TextStyles.Contains("FloorHeader"))
                {
                    _drawings.TextStyles.Add(new TextStyle("FloorHeader", "Areal", FontStyle.Underline));

                }
                //_drawings.TextStyles["Default"].Style = FontStyle.Underline;
                wallFloorName.StyleName = "FloorHeader";
                block.Entities.Add(wallFloorName);
                foreach (var levelWallLayer in level.WallLayers)
                {
                    var wallText = new Text(0, lineSpace, 0, levelWallLayer.WallName, 3.5);
                    wallText.ColorMethod = colorMethodType.byEntity;
                    //var line = new Line(new Point3D(0,lineSpace),new Point3D(100,lineSpace));
                    //line.ColorMethod = colorMethodType.byEntity;
                    //line.LayerName = "WiresLayerName";
                    //line.ColorMethod = colorMethodType.byEntity;
                    if (levelWallLayer.WallColorLayer != null)
                    {
                        // line.Color = levelWallLayer.WallColorLayer.Color;
                        wallText.Color = levelWallLayer.WallColorLayer.Color;
                    }
                    else
                    {
                        wallText.Color = Color.Black;
                        //line.Color = Color.Black;
                    }

                    //block.Entities.Add(line);
                    block.Entities.Add(wallText);

                    lineSpace -= 6;
                }
            }

            if (base.Cancelled(worker, null))
            {
                return null;
            }


            return block;
        }
        public new void  Dispose()
        {
            base.Dispose();
            _wallNameBlockDict.Clear();
            _wallNameBlockDict = null;
            _wallNamesDict.Clear();
            _wallNamesDict = null;
        }

        public new void AddToDrawings(Drawings drawings)
        {
            base.AddToDrawings(drawings);
            AddWallNameToDrawing(drawings);
            AddFloorQuantitiesToDrawing(drawings);
            //var numberOfFloor = drawings.Blocks.Count;
            //List<Block> floorBlocks = new List<Block>();
            foreach (var drawingsBlock in drawings.Blocks)
            {
                if (drawingsBlock.Name.Contains("View"))
                {
                    //floorBlocks.Add(drawingsBlock);
                    ReCalculatorFloorBlockColor(drawingsBlock);
                    //drawingsBlock.Entities.Regen();
                }
            }
            drawings.Entities.Regen();
        }

        private void AddFloorQuantitiesToDrawing(Drawings drawings)
        {
            foreach (var drawingsBlock in drawings.Blocks)
            {
                foreach (var block in _floorQuantitiesBlockDict)
                {
                    if (block.Value==null)
                    {
                        continue;
                    }

                    if (drawingsBlock.Name == block.Value.Name)
                    {
                        drawingsBlock.Entities.Clear();
                        drawingsBlock.Entities.AddRange(block.Value.Entities);
                        drawingsBlock.Entities.Regen();
                    }
                }
            }
        }

        private void AddWallNameToDrawing(Drawings drawings)
        {
            foreach (var drawingsBlock in _drawings.Blocks)
            {
                foreach (var block in _wallNameBlockDict)
                {
                    if (block.Value == null)
                    {
                        continue;
                    }
                    if (drawingsBlock.Name == block.Value.Name)
                    {
                        drawingsBlock.Entities.Clear();
                        drawingsBlock.Entities.AddRange(block.Value.Entities);
                        drawingsBlock.Entities.Regen();
                    }
                }
            }
        }

        private void ReCalculatorFloorBlockColor(Block floorBlock)
        {
            floorBlock.Entities.Clear();
            _drawings.renderContext.MakeCurrent();
            List<Block> blocks = new List<Block>();
            List<Entity> entities = new List<Entity>();
            
            
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
                                //var beamMesh = beamText.ConvertToMesh(_model);
                                //foreach (var mesh in beamMesh)
                                //{
                                //    mesh.Color = beamText.Color;
                                //    mesh.ColorMethod = colorMethodType.byEntity;
                                //}
                                //floorBlock.Entities.AddRange(beamte);
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
                    //case Beam2D beam:
                    //    var outerList = new List<Point2D>()
                    //        {beam.OuterStartPoint, beam.OuterEndPoint, beam.InnerEndPoint, beam.InnerStartPoint,beam.OuterStartPoint};
                    //    var beamQuad = Mesh.CreatePlanar(Plane.XY, outerList, Mesh.natureType.Plain);
                    //    beamQuad.Color = beam.Color;
                    //    beamQuad.ColorMethod = colorMethodType.byEntity;
                    //    floorBlock.Entities.Add(beamQuad);
                    //    continue;
                    case IRectangleSolid rectangle:
                        var outerList2 = new List<Point3D>()
                            {rectangle.OuterStartPoint, rectangle.OuterEndPoint, rectangle.InnerEndPoint, rectangle.InnerStartPoint,rectangle.OuterStartPoint};
                        //var linearPath = new LinearPath(outerList2.ToArray());
                        //linearPath.Color = rectangle.Color;
                        //linearPath.ColorMethod = colorMethodType.byEntity;
                        //linearPath.LineWeight = 0.1f;
                        //linearPath.LineWeightMethod = colorMethodType.byEntity;
                        //floorBlock.Entities.Add(linearPath);
                        //var hatch = new Hatch(Hatch.SolidPatternName, linearPath);
                        //hatch.Color = rectangle.Color;
                        //hatch.ColorMethod = colorMethodType.byEntity;
                        //floorBlock.Entities.Add(hatch);
                        //var quad = new Quad(Plane.XY, rectangle.OuterStartPoint, rectangle.OuterEndPoint, rectangle.InnerEndPoint,
                        //    rectangle.InnerStartPoint);
                        //var region = new Region(linearPath);
                        //region.Color = rectangle.Color;
                        //region.ColorMethod = colorMethodType.byEntity;
                        //floorBlock.Entities.Add(region);
                        //var meshs = quad.GetPolygonMeshes();
                        var quad = Mesh.CreatePlanar(outerList2, Mesh.natureType.Smooth);
                        quad.PrintOrder = 1;
                        
                        //foreach (var quadVertex in quad.Vertices)
                        //{
                        //    quadVertex.Z = -10;
                        //}
                        //quad.TransformBy(transformation);
                        quad.Color = rectangle.Color;
                        quad.ColorMethod = colorMethodType.byEntity;
                        quad.LineWeight = 0.1f;
                        quad.LineWeightMethod = colorMethodType.byEntity;
                        //quad.EdgeStyle = Mesh.edgeStyleType.Sharp;


                        //foreach (var mesh in meshs)
                        //{
                        //    mesh.Color = rectangle.Color;
                        //    mesh.ColorMethod = colorMethodType.byEntity;
                        //}
                        floorBlock.Entities.Add(quad);
                        //floorBlock.Entities.AddRange(meshs);

                        continue;
                    case Hanger2D hanger:
                        var hangerInsertPoint = hanger.InsertionPoint;
                        //hangerInsertPoint.Z = 10;
                        var hangerText = new Text(Plane.XY, hangerInsertPoint, hanger.TextString, hanger.Height,
                            hanger.Alignment);
                        hangerText.Color = hanger.Color;
                        hangerText.ColorMethod = colorMethodType.byEntity;
                        var hangerCirCle = new Circle(Plane.XY, hangerInsertPoint, hangerText.Height);
                        hangerCirCle.Color = hanger.Color;
                        hangerCirCle.ColorMethod = colorMethodType.byEntity;
                        //var hangerTexrShape = hangerText.ConvertToMesh(_model);
                        //foreach (var mesh in hangerTexrShape)
                        //{
                        //    mesh.Color = hangerText.Color;
                        //    mesh.ColorMethod = colorMethodType.byEntity;
                        //}
                        //floorBlock.Entities.AddRange(hangerTexrShape);
                        //floorBlock.Entities.Insert(0,hangerText);
                        //floorBlock.Entities.Insert(0,hangerCirCle);
                        hangerText.PrintOrder = 2;
                        hangerCirCle.PrintOrder = 2;
                        floorBlock.Entities.Add(hangerText);
                        floorBlock.Entities.Add(hangerCirCle);
                        continue;
                    case JoistArrowEntity joistArrow:
                        var points = new List<Point3D>()
                            {joistArrow.StartArrow, joistArrow.StartPoint, joistArrow.EndPoint, joistArrow.EndArrow};
                        var joistArrowPath = new LinearPath(points);
                        joistArrowPath.Color = Color.Black;
                        joistArrowPath.ColorMethod = colorMethodType.byEntity;
                        joistArrowPath.LineWeight = 0.1f;
                        joistArrowPath.LineWeightMethod = colorMethodType.byEntity;
                        floorBlock.Entities.Add(joistArrowPath);
                        continue;
                    case Line wallLine:
                        var line = (Line)wallLine.Clone();

                        if (line.Color == Color.White|| line.Color == Color.Snow || line.LayerName == "LBW")
                        {
                            line.Color = Color.Black;
                            line.ColorMethod = colorMethodType.byEntity;
                            line.LineWeight = 0.1f;
                            line.LineWeightMethod = colorMethodType.byEntity;
                            floorBlock.Entities.Add(line);
                            continue;

                        }
                        if (line.LayerName == "NLBW")
                        {
                            line.Color = Color.Black;
                            line.ColorMethod = colorMethodType.byEntity;
                            line.LineWeight = 0.1f;
                            line.LineWeightMethod = colorMethodType.byEntity;
                            line.LineTypeName = "Dash Space";
                            line.LineTypeMethod = colorMethodType.byEntity;
                            line.LineTypeScale = 0.5f;
                            floorBlock.Entities.Add(line);
                            continue;
                        }
                        break;
                    case Blocking2D blocking:
                        var cloneBlocking = blocking.Clone() as Entity;
                        if (cloneBlocking!=null)
                        {
                            cloneBlocking.PrintOrder = 2;
                            cloneBlocking.Color = blocking.Color;
                            cloneBlocking.ColorMethod = colorMethodType.byEntity;
                            cloneBlocking.LineWeight = 0.6f;
                            cloneBlocking.LineWeightMethod = colorMethodType.byEntity;
                            floorBlock.Entities.Add(cloneBlocking);
                        }
                        break;
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
