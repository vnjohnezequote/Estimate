﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using devDept.Geometry;

namespace DrawingModule.ViewModels
{
    public sealed class PromptPointResult : PromptResult
    {
        #region Field

        private Point3D m_value;


        #endregion

        #region properties

        public Point3D Value => m_value;
        
        

        #endregion
        #region Constructor
        internal PromptPointResult(PromptStatus promptStatus, string stringResult, Point3D pt)
            : base(promptStatus, stringResult)
        {
            if (promptStatus == PromptStatus.OK)
            {
                m_value = new Point3D(pt.X, pt.Y, pt.Z);
            }
        }


        #endregion
    }
}
