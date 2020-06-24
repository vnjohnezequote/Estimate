using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using devDept.Serialization;
using Environment = System.Environment;

namespace DrawingModule.Entities
{
	[Serializable]
	public class CustomTable : Table
	{
		public CustomTable(Plane tablePlane, int rows, int columns, double rowsHeight, double columnsWidth, double textHeight, flowDirection direction = flowDirection.Down) : base(tablePlane, rows, columns, rowsHeight, columnsWidth, textHeight, direction)
		{
		}

		public CustomTable(Plane tablePlane, int rows, int columns, double[] rowsHeights, double[] columnsWidths, double textHeight, flowDirection direction = flowDirection.Down) : base(tablePlane, rows, columns, rowsHeights, columnsWidths, textHeight, direction)
		{
		}

		protected CustomTable(Table another) : base(another)
		{
		}

		public CustomTable(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
    }
}
