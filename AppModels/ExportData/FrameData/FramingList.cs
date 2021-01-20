using System.Collections.ObjectModel;

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
            if (FrameList.Count == 0)
            {
                FrameList.Add(frame);
            }
            else
            {
                bool checkifExisting = false;
                for (int i = 0; i < FrameList.Count; i++)
                {
                    if (FrameList[i].IsSameWith(frame))
                    {
                        FrameList[i].Quantity += frame.Quantity;
                        checkifExisting = true;
                    }
                }

                if (!checkifExisting)
                {
                    FrameList.Add(frame);
                }

            }
            
        }
    }
}
