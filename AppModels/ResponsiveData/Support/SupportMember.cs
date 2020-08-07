using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.EngineerMember;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class SupportMember : BindableBase, ISupportMember
    {
        private EngineerMemberInfo _supportInfo;
        public string Id { get; set; }
        public SupportType PointSupportType { get; set; }
        public EngineerMemberInfo SupportInfo { get=>_supportInfo; set=>SetProperty(ref _supportInfo,value); }

        public SupportMember()
        {
        }

       
    }
}
