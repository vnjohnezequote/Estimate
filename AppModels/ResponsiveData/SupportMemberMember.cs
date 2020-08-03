using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData
{
    public class SupportMemberMember : ISupportMember
    {
        public string Id { get; }
        public SupportType PointSupportType { get; }
        public ISupportInfo SupportInfo { get; }
    }
}
