using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppModels.ExportData.FrameData
{
    public class FramingList
    {
        
        public ObservableCollection<FrameData> FrameList { get; } = new ObservableCollection<FrameData>();
        public FramingList()
        {
        
        }

        public void Add(FrameData frame)
        {
            foreach (var framing in FrameList)
            {
                if (framing.IsSameWith(frame))
                {
                    framing.Quantity += framing.Quantity;
                }
                else
                {
                    FrameList.Add(frame);
                }
            }
        }
    }
}
