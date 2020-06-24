using System.Runtime.Serialization;
using devDept.Geometry;
using Syncfusion.XPS;
using Table = devDept.Eyeshot.Entities.Table;

namespace DrawingModule.Entities
{
    public class MyTable : Table
    {
        #region Constructor
        public MyTable(Plane tablePlane, int rows, int columns, double rowsHeight, double columnsWidth, double textHeight, flowDirection direction = flowDirection.Down) : base(tablePlane, rows, columns, rowsHeight, columnsWidth, textHeight, direction)
        {
        }

        public MyTable(Plane tablePlane, int rows, int columns, double[] rowsHeights, double[] columnsWidths, double textHeight, flowDirection direction = flowDirection.Down) : base(tablePlane, rows, columns, rowsHeights, columnsWidths, textHeight, direction)
        {
        }

        protected MyTable(Table another) : base(another)
        {
        }

        public MyTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion



    }
}