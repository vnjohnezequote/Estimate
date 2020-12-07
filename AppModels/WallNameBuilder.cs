
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using Attribute = devDept.Eyeshot.Entities.Attribute;

namespace AppModels
{
    public enum QueueType
    {

    }
    public class WallNameBuilder: WorkUnit, IDisposable
    {
        private Drawings _drawings;
        private IJob _jobModel;
        private BackgroundWorker _backgroundWorker;
        private QueueType _queueType;
        public ViewBuilder.operatingType OperatingMode { get; set; }

        private Dictionary<Sheet, WallName> _wallNamesDict;
        private Dictionary<WallName, Block> _wallNameBlockDict;
        public WallNameBuilder(Drawings drawings,IJob jobModel)
        {
            _drawings = drawings;
            _jobModel = jobModel;
            //OperatingMode = queOperatingType;
            _wallNamesDict = InitWallNameListDict(drawings);

        }

        private static Dictionary<Sheet,WallName> InitWallNameListDict(Drawings drawings)
        {
            var wallNamesDict= new Dictionary<Sheet, WallName>();
            foreach (var sheet in drawings.Sheets)
            {
                foreach (var sheetEntity in sheet.Entities)
                {
                    var wallName = sheetEntity as WallName;
                    if (wallName != null)
                    {
                        wallNamesDict.Add(sheet,wallName);
                    }
                }
            }

            return wallNamesDict;

        }

        protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs doWorkEventArgs)
        {
            _wallNameBlockDict = new Dictionary<WallName, Block>();
            _backgroundWorker = worker;
            if (_backgroundWorker!=null)
            {
                _queueType =(QueueType)1;
            }
            else
            {
                _queueType = (QueueType) 0;
            }

            foreach (var wallName in _wallNamesDict)
            {
                var wallRef = wallName.Value;
                var wallSheet = wallName.Key;
                var wallBlock =GeneralWallList(wallRef,wallSheet,worker);
                if (wallBlock!=null)
                {
                    _wallNameBlockDict.Add(wallRef, wallBlock);
                }
                
                if (base.Cancelled(worker,null))
                {
                    this._queueType = (QueueType) 2;
                    break;
                }

                if (worker!=null && this.OperatingMode == ViewBuilder.operatingType.Queue)
                {
                    break;
                }
            }

        }

        private Block GeneralWallList(WallName wallName, Sheet wallSheet,BackgroundWorker worker)
        {
            Block block;
            block = base.Cancelled(worker, null) ? null : GeneralWallListStart(wallName, wallSheet, worker);
            return block;
        }

        private Block GeneralWallListStart(WallName wallName, Sheet wallSheet, BackgroundWorker worker)
        {
            Block block = new Block(wallName.BlockName);
            if (this._jobModel==null|| _jobModel.Levels==null )
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

            if (level!=null)
            {
                if (level.WallLayers==null|| level.WallLayers.Count==0)
                {
                    return null;
                }
                
                var lineSpace = 0d;
                var wallFloorName = new Attribute(0,lineSpace,0,"Title","",3.5);
                //var wallFloorName = new Text(0,lineSpace,level.LevelName + ":",3.5);
                lineSpace -= 6;
                wallFloorName.ColorMethod = colorMethodType.byEntity;
                wallFloorName.Color = Color.Black;
                var wallBoxMax = wallFloorName.BoxMax;
                var wallBoxMin = wallFloorName.BoxMin;
                if (!_drawings.TextStyles.Contains("FloorHeader"))
                {
                    _drawings.TextStyles.Add(new TextStyle("FloorHeader","Areal",FontStyle.Underline));
                    
                }
                //_drawings.TextStyles["Default"].Style = FontStyle.Underline;
                wallFloorName.StyleName = "FloorHeader";
                block.Entities.Add(wallFloorName);
                foreach (var levelWallLayer in level.WallLayers)
                {
                    var wallText = new Text(0,lineSpace,0,levelWallLayer.WallName,3.5);
                    wallText.ColorMethod = colorMethodType.byEntity;
                    //var line = new Line(new Point3D(0,lineSpace),new Point3D(100,lineSpace));
                    //line.ColorMethod = colorMethodType.byEntity;
                    //line.LayerName = "WiresLayerName";
                    //line.ColorMethod = colorMethodType.byEntity;
                    if (levelWallLayer.WallColorLayer!=null)
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

            if (base.Cancelled(worker,null))
            {
                return null;
            }


            return block;
        }
        public void Dispose()
        {
            _wallNameBlockDict.Clear();
            _wallNameBlockDict = null;
            _wallNamesDict.Clear();
            _wallNamesDict = null;
        }

        public void AddToDrawings()
        {
            _drawings.renderContext.MakeCurrent();
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
    }
}
