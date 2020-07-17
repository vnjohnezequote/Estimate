using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class BeamLeader: Leader
    {
        public BeamType BeamType;

        public BeamLeader(Plane pln, ICollection<Point3D> points) : base(pln, points)
        {
        }

        protected BeamLeader(Leader another) : base(another)
        {
        }
    }
}
