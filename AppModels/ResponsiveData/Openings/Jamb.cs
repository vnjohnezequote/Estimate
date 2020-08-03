using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.WallMemberData;

namespace AppModels.ResponsiveData.Openings
{
    public class Jamb : WallMemberBase
    {
        private int _height;
        public int Height
        {
            get
            {
                if (_height != 0)
                {
                    return _height;
                }

                if (WallInfo != null)
                {
                    return WallInfo.StudHeight;
                }

                switch (BaseMaterialInfo)
                {
                    case null:
                        return 0;
                    case WallStud wallStud:
                        return wallStud.Height;
                    default:
                        return 0;
                }
            }
            set
            {
                if(WallInfo!=null && value == WallInfo.StudHeight)
                {
                    value = 0;
                }
                if (BaseMaterialInfo != null && BaseMaterialInfo is WallStud wallStud && value == wallStud.Height)
                {
                    value = 0;
                }

                SetProperty(ref _height, value);
            }
        }
        public Jamb(IWallInfo wallInfo) : base(wallInfo)
        {
        }
        public override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo { get; }
        public override int Thickness
        {
            get
            {
                if (_thickness != 0)
                {
                    return _thickness;
                }

                if(BaseMaterialInfo!=null)
                {
                    return BaseMaterialInfo.Thickness;
                }

                return WallInfo?.WallThickness ?? 0;
            } 
            set
            {
                    if (BaseMaterialInfo!=null)
                    {
                        if (value == BaseMaterialInfo.Thickness)
                        {
                            value = 0;
                        }
                    } else if (WallInfo.WallThickness == value)
                    {
                        value = 0;
                    }
                    SetProperty(ref _thickness, value);
            }
        }
    }
}
