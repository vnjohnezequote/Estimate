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

        public Point3D GetTopLeftCorner(int row, int col)
        {
            var bottomLeft = this.GetBottomLeftCorner(row, col);
            var topCorner = new Point3D(bottomLeft.X, bottomLeft.Y + this.GetRectHeight(row, col));
            return topCorner;
        }
		public Point3D GetTopRightCorner(int row, int col)
        {
            var bottomLeft = this.GetBottomLeftCorner(row, col);
            var topRight = new Point3D(bottomLeft.X + this.GetRectWidth(row, col),bottomLeft.Y+this.GetRectHeight(row,col));
            return topRight;
        }
        public Point3D GetBottomRightCorner(int row, int col)
        {
            var bottomLeft = this.GetBottomLeftCorner(row, col);
            
            var bottomRigth = new Point3D(bottomLeft.X + this.GetRectWidth(row, col), bottomLeft.Y);
            return bottomRigth;
        }
    }
}
