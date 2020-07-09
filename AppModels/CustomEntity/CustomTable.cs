using System;
using System.Runtime.Serialization;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
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
