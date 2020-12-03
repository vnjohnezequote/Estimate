using System;
using AppModels.Enums;
using AppModels.ResponsiveData.Support;

namespace AppModels.PocoDataModel
{
    public class SupportPointPoco
    {
        public LoadPointLocation PointLocation { get; set; }
        public Guid EngineerReferenceId { get; set; }
        public SupportType? PointSupportType { get; set; }

        public SupportPointPoco()
        {

        }
        public SupportPointPoco( SupportPoint supportPoint)
        {
            PointLocation = supportPoint.PointLocation;
            if (supportPoint.EngineerMemberInfo!=null)
            {
                EngineerReferenceId = supportPoint.EngineerMemberInfo.Id;
            }
            PointSupportType = supportPoint.PointSupportType;
        }
    }
}
