using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class CreateJoistName: ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Create Joist Name";
        private bool _processingTool = true;
        public CreateJoistName()
        {
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }

        [CommandMethod("Create Joist Name")]
        public void CreateJName()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (_processingTool)
            {
                ToolMessage = "Please select Joist to Create Name";
                var promptEntities = new PromptEntityOptions();
                var resutl = acDoc.Editor.GetEntities(promptEntities, true);
                if (resutl.Status !=PromptStatus.OK) return;
                var sortedJoistList = SortJoist(resutl.Entities);
                var countLoop = 0;
                var endLoop = sortedJoistList.Count;
                Point3D startP = null;
                Point3D endP = null;
                int startIndex=0;
                int endIndex = 0;
                var countJoist = 0;
                while (countLoop<endLoop)
                {
                    if (startP == null)
                    {
                        countJoist = 1;
                        startIndex = countLoop;
                        startP = sortedJoistList[countLoop].MidPoint;
                        countLoop++;
                    }
                    else
                    {
                        if (sortedJoistList[countLoop].FramingReference.Name == sortedJoistList[countLoop-1].FramingReference.Name)
                        {
                            countJoist++;
                            endP = sortedJoistList[countLoop].MidPoint;
                            countLoop++;
                            if (countLoop == endLoop)
                            {
                                CreateJoistFramingName(ref countJoist,ref startIndex,ref endIndex,ref countLoop,ref startP,ref endP,sortedJoistList);
                            }
                            
                        }
                        else
                        {
                            CreateJoistFramingName(ref countJoist,ref startIndex,ref endIndex,ref countLoop,ref startP,ref endP,sortedJoistList);
                        }
                        
                    }
                }
                return;
            }
            
        }

        private void CreateJoistFramingName(ref int countJoist,ref int startIndex,ref int endIndex,ref int countLoop,ref Point3D startP,ref Point3D endP,List<Joist2D> sortedJoistList)
        { 
            if (countJoist<4)
            {
                for (var i = 0; i < countJoist; i++)
                {
                    if (sortedJoistList[startIndex].CreateEntityVm(EntitiesManager) is Joist2dVm joistVm)
                        joistVm.IsShowFramingName = true;
                    startIndex++;
                }

                if (startIndex== sortedJoistList.Count)
                {
                    return;
                }
                startP =sortedJoistList[startIndex].MidPoint;
                countJoist = 1;
                countLoop++;
            }
            else
            {
                endIndex = countLoop-1;
                var firstExtend =(double) sortedJoistList[startIndex].Thickness/2;
                var endExtend = (double)sortedJoistList[endIndex].Thickness / 2;
                var arrowSegment = new Segment2D(startP, endP);
                arrowSegment.ExtendBy(firstExtend,endExtend);
                startP = arrowSegment.P0.ConvertPoint2DtoPoint3D();
                endP = arrowSegment.P1.ConvertPoint2DtoPoint3D();
                var arrowLine = new JoistArrowEntity((Point3D)startP.Clone(), (Point3D)endP.Clone());
                arrowLine.LineTypeMethod = colorMethodType.byEntity;
                arrowLine.LineWeightMethod = colorMethodType.byEntity;
                arrowLine.LineWeight = 0.5f;
                 if (LayerManager.SelectedLayer.LineTypeName != "Continues")
                 { 
                     arrowLine.LineTypeName = LayerManager.SelectedLayer.LineTypeName;
                 }
                 arrowLine.Color = LayerManager.SelectedLayer.Color;
                GeneralFramingName(sortedJoistList[startIndex].FramingReference,arrowLine);
                EntitiesManager.AddAndRefresh(arrowLine,LayerManager.SelectedLayer.Name);
                if (countLoop == sortedJoistList.Count)
                {
                    return;
                }
                startP = sortedJoistList[countLoop].MidPoint;
                startIndex = countLoop;
                countJoist = 1;
                countLoop++;
            }

        }    

        private void GeneralFramingName(IFraming framingReference, JoistArrowEntity joistArrow)
        {
            if (framingReference == null) return;
            var p0 = joistArrow.StartPoint;
            var p1 = joistArrow.EndPoint;
            if (p0.X > p1.X)
            {
                Utility.Swap(ref p0, ref p1);
            }
            else if (Math.Abs(p0.X - p1.X) < 0.00001 && p0.Y > p1.Y)
            {
                Utility.Swap(ref p0, ref p1);
            }
            var framingBaseLine = new Segment2D(p0, p1);
            framingBaseLine = framingBaseLine.Offset(-100);
            var initPoint = framingBaseLine.MidPoint.ConvertPoint2DtoPoint3D();
            var framingName = new FramingNameEntity(initPoint, framingReference.Name, 200,
                Text.alignmentType.BaselineCenter, framingReference);
            joistArrow.FramingName = framingName;
            joistArrow.FramingNameId = framingName.Id;
            Vector2D v = new Vector2D(p0, p1);
            var radian = v.Angle;
            framingName.Rotate(radian, Vector3D.AxisZ, framingName.InsertionPoint);
            //framingName.Color =;
            framingName.ColorMethod = colorMethodType.byEntity;
            EntitiesManager.AddAndRefresh(framingName, this.LayerManager.SelectedLayer.Name);

        }

        private List<Joist2D> SortJoist(List<Entity> entities)
        {
            var i = 0;
            var returnList = new List<Joist2D>();
            //var ret = (from entity1 in entities
            //    where entity1 is Joist2D
            //    let joist2D = (Joist2D) entity1
            //    select joist2D).ToList();
            
            foreach (var entity in entities)
            {
                if(entity is Joist2D joist)
                    returnList.Add(joist);
            }

            if (returnList[0].FramingDirectionType == FramingRectangeDirectionType.Vertical )
            {
                returnList.Sort((x, y) =>
                {
                    if (x.MidPoint.Y.CompareTo(y.MidPoint.Y) < 0)
                    {
                        return 1;
                    }
                    else if(x.MidPoint.Y.CompareTo(y.MidPoint.Y) > 0)
                    {
                        return -1;
                    }

                    return 0;

                });

            }
            else
            {
                returnList.Sort((x, y) => x.MidPoint.X.CompareTo(y.MidPoint.X));
            }

            return returnList;



        }
        
        




        private void SortJoist(ref Joist2D joist1,ref Joist2D joist2)
        {
            if (joist1.FramingDirectionType == FramingRectangeDirectionType.Freedom || joist1.FramingDirectionType==FramingRectangeDirectionType.Horizontal)
            {
                if (!(joist1.StartCenterLinePoint.Y < joist2.StartCenterLinePoint.Y)) return;
                Utility.Swap(ref joist1,ref joist2);
            }
            else
            {
                if (joist1.StartCenterLinePoint.X> joist2.StartCenterLinePoint.X)
                {
                    Utility.Swap(ref joist1, ref joist2);
                }
                
            }
        }
        private void AddNewBlockJoist(Joist2D joist,Dictionary<string,List<Joist2D>> joistBlock,bool isCreateNew = false)
        {
            var blockName = joist.FramingReference.Name + " - Block ";
            var maxNumBlock = 1;
            if (isCreateNew)
            {
                maxNumBlock = GetMaxNumberBlockName(joistBlock,joist.FramingReference.Name)+1;
            }
            blockName += maxNumBlock;

            List<Joist2D> listJoist = null;

            if (joistBlock.ContainsKey(blockName))
            {
                joistBlock.TryGetValue(blockName, out listJoist);
            }
            else
            {
                listJoist = new List<Joist2D>();
            }

            if (listJoist == null) return;
            listJoist.Add(joist);
            joistBlock.Add(blockName, listJoist);
        }
        private int GetMaxNumberBlockName(Dictionary<string, List<Joist2D>> joistBlock,string blockName)
        {
            var maxNumber = 1;
            foreach (var joitBlockNameE in joistBlock.Keys)
            {
                if (joitBlockNameE.Contains(blockName))
                {
                    var number = GetStringNumberAfterKeywork(joitBlockNameE);
                    if (number > maxNumber)
                    {
                        maxNumber = number;
                    }
                }
            }
            return maxNumber;
        }
        public int GetStringNumberAfterKeywork(string source, string keyWork = "Block")
        {
            var index = source.IndexOf(keyWork, StringComparison.Ordinal);
            if (index == 1) return 1;
            var code = source.Substring(index, keyWork.Length + 1);
            return short.TryParse(code, out var resultCode) ? resultCode : 1;
        }
        private List<List<Joist2D>> GetBlockJoistInDic(Dictionary<string,List<Joist2D>> joistBlockDic,string joistName)
        {
            var listJoistBlock = new List<List<Joist2D>>();
            foreach (var joistBlock in joistBlockDic)
            {
                if (joistBlock.Key.Contains(joistName))
                {
                    listJoistBlock.Add(joistBlock.Value);
                }
            }
            return listJoistBlock;
        }

        private void CreateJoistArrow(KeyValuePair<string, List<Joist2D>> joistBlock)
        {
            if (joistBlock.Value.Count<4)
            {
                foreach (var joist in joistBlock.Value)
                {
                    if (joist.CreateEntityVm(EntitiesManager) is Joist2dVm joistVm) joistVm.IsShowFramingName = true;
                }
            }
            else
            {
                
            }
        }

        private Dictionary<int,List<Joist2D>> FindJoistParallel(List<Joist2D> joistList)
        {
            var joistDic = new Dictionary<int, List<Joist2D>>();
            var key = 0;
            while (joistList.Count>0)
            {
                
            }




            return joistDic;
        }
        

    }
}
