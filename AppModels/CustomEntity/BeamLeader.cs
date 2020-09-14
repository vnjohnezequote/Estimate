using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using AppModels.Enums;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class BeamLeader
    {
        public BeamType BeamType;
        public Beam BeamReference { get; set; }


    }
}
