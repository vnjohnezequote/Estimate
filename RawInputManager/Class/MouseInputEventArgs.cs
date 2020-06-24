using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawInputManager.Enum;
using RawInputManager.Structure;

namespace RawInputManager.Class
{
    public class MouseInputEventArgs:RawInputEventArgs
    {
        public MouseInputEventArgs()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.RawInput.MouseInputEventArgs" /> class.
        /// </summary>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="hwnd">The handle of the window that received the RawInput mesage.</param>
        // Token: 0x06000071 RID: 113 RVA: 0x00002BF0 File Offset: 0x00000DF0
        internal MouseInputEventArgs(ref InputData rawInput, IntPtr hwnd) : base(ref rawInput, hwnd)
        {
            this.MouseFlags = rawInput.data.mouse.Flags;
            this.ButtonFlags = rawInput.data.mouse.ButtonFlags;
            this.WheelDelta = rawInput.data.mouse.ButtonData;
            this.Buttons = (int)rawInput.data.mouse.RawButtons;
            this.X = rawInput.data.mouse.LastX;
            this.Y = rawInput.data.mouse.LastY;
            this.ExtraInformation = (int)rawInput.data.mouse.ExtraInformation;
        }
        /// <summary>
		/// Gets or sets the mode.
		/// </summary>
		/// <value>
		/// The mode.
		/// </value>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002CA9 File Offset: 0x00000EA9
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002CB1 File Offset: 0x00000EB1
		public RawMouseFlags MouseFlags { get; set; }

		/// <summary>
		/// Gets or sets the button flags.
		/// </summary>
		/// <value>
		/// The button flags.
		/// </value>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002CBA File Offset: 0x00000EBA
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002CC2 File Offset: 0x00000EC2
		public RawMouseButtons ButtonFlags { get; set; }

		/// <summary>
		/// Gets or sets the extra information.
		/// </summary>
		/// <value>
		/// The extra information.
		/// </value>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002CCB File Offset: 0x00000ECB
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002CD3 File Offset: 0x00000ED3
		public int ExtraInformation { get; set; }

		/// <summary>
		/// Gets or sets the raw buttons.
		/// </summary>
		/// <value>
		/// The raw buttons.
		/// </value>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002CDC File Offset: 0x00000EDC
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002CE4 File Offset: 0x00000EE4
		public int Buttons { get; set; }

		/// <summary>
		/// Gets or sets the wheel delta.
		/// </summary>
		/// <value>
		/// The wheel delta.
		/// </value>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002CED File Offset: 0x00000EED
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002CF5 File Offset: 0x00000EF5
		public int WheelDelta { get; set; }

		/// <summary>
		/// Gets or sets the X.
		/// </summary>
		/// <value>
		/// The X.
		/// </value>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002CFE File Offset: 0x00000EFE
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002D06 File Offset: 0x00000F06
		public int X { get; set; }

		/// <summary>
		/// Gets or sets the Y.
		/// </summary>
		/// <value>
		/// The Y.
		/// </value>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002D0F File Offset: 0x00000F0F
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002D17 File Offset: 0x00000F17
		public int Y { get; set; }
    }
}
