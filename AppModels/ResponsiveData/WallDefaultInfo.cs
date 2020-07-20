using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class WallDefaultInfo : BindableBase
    {
        #region Field

        private int _wallSpacing;
        private int _wallThickness;
        private int _materialDepth;
        private string _material;
        public WallMemberBase _ribbonPlate { get; set; }
        public WallMemberBase _topPlate { get; set; }
        public WallStud _stud { get; set; }
        public WallMemberBase _bottomPlate { get; set; }
        #endregion

        #region Property
        public int WallSpacing { get=>_wallSpacing; set=>SetProperty(ref _wallSpacing,value); }
        public int WallThickness { get=>_wallThickness; set=>SetProperty(ref _wallThickness,value); }
        public int MaterialDepth { get=>_materialDepth; set=>SetProperty(ref _materialDepth,value); }
        public string Material { get=>_material; set=>SetProperty(ref _material,value); }
        /// <summary>
        /// Gets or sets the ribbon plate.
        /// </summary>
        public WallMemberBase RibbonPlate { get; set; }
        public WallMemberBase TopPlate { get; set; }
        public WallStud Stud { get; set; }
        public WallMemberBase BottomPlate { get; set; }
        public WallMemberBase Nogging { get; set; }
        public WallMemberBase Trimmer { get; set; }
        //public TimberBase 
        
        #endregion

        public WallDefaultInfo()
        {

        }
    }
}
