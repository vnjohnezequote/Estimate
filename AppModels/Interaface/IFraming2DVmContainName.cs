using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.Interaface
{
    public interface IFraming2DVmContainName
    {
        string Name { get; set; }
        int Index { get; set; }
        int SubFixIndex { get; }
    }
}
