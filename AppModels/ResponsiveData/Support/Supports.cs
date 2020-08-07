using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class Supports
    {
        private string LevelName { get; set; }
        public SupportType LoadSupportType { get; set; }
        public ObservableCollection<ISupportMember> SupportMemberList = new ObservableCollection<ISupportMember>();

        public Supports(string levelName)
        {

        }
    }
}
