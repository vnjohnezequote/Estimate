using System;

namespace DrawingModule.Control
{
    [Flags]
    public enum Direction
    {
        None = 0,
        // Token: 0x04000649 RID: 1609
        Top = 1,
        // Token: 0x0400064A RID: 1610
        Left = 2,
        // Token: 0x0400064B RID: 1611
        Bottom = 4,
        // Token: 0x0400064C RID: 1612
        Right = 8,
        // Token: 0x0400064D RID: 1613
        TopLeft = 3,
        // Token: 0x0400064E RID: 1614
        TopRight = 9,
        // Token: 0x0400064F RID: 1615
        BottomLeft = 6,
        // Token: 0x04000650 RID: 1616
        BottomRight = 12
    }
}