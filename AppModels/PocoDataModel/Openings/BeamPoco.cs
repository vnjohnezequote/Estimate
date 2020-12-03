using System;
using System.Collections.Generic;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel.Framings.Beams;
using AppModels.ResponsiveData.Openings;

namespace AppModels.PocoDataModel.Openings
{
    public class BeamPoco: BeamBasePoco
    {
        public BeamPoco() 
        {

        }
        public BeamPoco(Beam beamInfo): base(beamInfo)
        {

        }
        
    }
}
