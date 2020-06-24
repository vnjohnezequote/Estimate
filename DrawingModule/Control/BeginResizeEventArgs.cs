using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawingModule.Control
{
    class BeginResizeEventArgs :RoutedEventArgs
    {
        public BeginResizeEventArgs(Direction direction) : base(ResizeThumb.BeginResizeEvent)
        {
            this.ResizeDirection = direction;
        }

        // Token: 0x17000419 RID: 1049
        // (get) Token: 0x06001538 RID: 5432 RVA: 0x0004B0A9 File Offset: 0x0004A0A9
        // (set) Token: 0x06001539 RID: 5433 RVA: 0x0004B0B1 File Offset: 0x0004A0B1
        public Direction ResizeDirection { get; private set; }
    }
}
