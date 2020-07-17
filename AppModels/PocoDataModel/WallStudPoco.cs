using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.PocoDataModel
{
    public class WallStudPoco
    {
        public int Id { get; set; }
        public TimberBasePoco TimberInfo { get; set; }
        public int Height { get; set; }
        public bool IsDefault { get; set; }
    }
}
