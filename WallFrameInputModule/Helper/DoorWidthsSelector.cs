using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData.Openings;
using Syncfusion.UI.Xaml.Grid;

namespace WallFrameInputModule.Helper
{
    public class DoorWidthsSelector : IItemsSourceSelector
    {
        public List<int> ExtDoorListWidths { get; set; } = new List<int> { 660, 960, 1260, 1560, 1860, 2160, 2460, 2760, 3060, 3360, 3660 };
        public List<int> IntDoorListhWidths { get; set; } = new List<int> { 875, 930, 1201, 1501, 1801, 2101, 2401, 2701, 3001, 3301, 3601, 4201 };
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            if (record == null)
            {
                return null;
            }

            var door = record as Opening;
            if (door.WallReference == null|| door.WallReference.WallType==null)
                return null;
            if (door.WallReference.WallType.IsLoadBearingWall)
                return ExtDoorListWidths;
            else return IntDoorListhWidths;

        }
    }
}
