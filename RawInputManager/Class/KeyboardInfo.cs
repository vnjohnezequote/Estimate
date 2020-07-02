using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RawInputManager.Class
{
    public class KeyboardInfo : DeviceInfo
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.RawInput.KeyboardInfo" /> class.
		/// </summary>
		// Token: 0x0600004C RID: 76 RVA: 0x000027D9 File Offset: 0x000009D9
		public KeyboardInfo()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.RawInput.KeyboardInfo" /> class.
		/// </summary>
		/// <param name="rawDeviceInfo">The raw device info.</param>
		/// <param name="deviceName">Name of the device.</param>
		/// <param name="deviceHandle">The device handle.</param>
		// Token: 0x0600004D RID: 77 RVA: 0x00002990 File Offset: 0x00000B90
		internal KeyboardInfo(ref Structure.DeviceInfo rawDeviceInfo, string deviceName, IntPtr deviceHandle) : base(ref rawDeviceInfo, deviceName, deviceHandle)
		{
			this.KeyboardType = (int)rawDeviceInfo.KeyboardInfo.Type;
			this.Subtype = (int)rawDeviceInfo.KeyboardInfo.SubType;
			this.KeyboardMode = (int)rawDeviceInfo.KeyboardInfo.KeyboardMode;
			this.FunctionKeyCount = (int)rawDeviceInfo.KeyboardInfo.NumberOfFunctionKeys;
			this.IndicatorCount = (int)rawDeviceInfo.KeyboardInfo.NumberOfIndicators;
			this.TotalKeyCount = (int)rawDeviceInfo.KeyboardInfo.NumberOfKeysTotal;
		}

		/// <summary>
		/// Gets or sets the type of the keyboard.
		/// </summary>
		/// <value>
		/// The type of the keyboard.
		/// </value>
		/// <unmanaged>unsigned int dwType</unmanaged>	
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002A0C File Offset: 0x00000C0C
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002A14 File Offset: 0x00000C14
		public int KeyboardType { get; set; }

		/// <summary>
		/// Gets or sets the subtype.
		/// </summary>
		/// <value>
		/// The subtype.
		/// </value>
		/// <unmanaged>unsigned int dwSubType</unmanaged>	
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002A1D File Offset: 0x00000C1D
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002A25 File Offset: 0x00000C25
		public int Subtype { get; set; }

		/// <summary>
		/// Gets or sets the keyboard mode.
		/// </summary>
		/// <value>
		/// The keyboard mode.
		/// </value>
		/// <unmanaged>unsigned int dwKeyboardMode</unmanaged>	
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002A2E File Offset: 0x00000C2E
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002A36 File Offset: 0x00000C36
		public int KeyboardMode { get; set; }

		/// <summary>
		/// Gets or sets the function key count.
		/// </summary>
		/// <value>
		/// The function key count.
		/// </value>
		/// <unmanaged>unsigned int dwNumberOfFunctionKeys</unmanaged>	
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002A3F File Offset: 0x00000C3F
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002A47 File Offset: 0x00000C47
		public int FunctionKeyCount { get; set; }

		/// <summary>
		/// Gets or sets the indicator count.
		/// </summary>
		/// <value>
		/// The indicator count.
		/// </value>
		/// <unmanaged>unsigned int dwNumberOfIndicators</unmanaged>	
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002A50 File Offset: 0x00000C50
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002A58 File Offset: 0x00000C58
		public int IndicatorCount { get; set; }

		/// <summary>
		/// Gets or sets the total key count.
		/// </summary>
		/// <value>
		/// The total key count.
		/// </value>
		/// <unmanaged>unsigned int dwNumberOfKeysTotal</unmanaged>	
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002A61 File Offset: 0x00000C61
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002A69 File Offset: 0x00000C69
		public int TotalKeyCount { get; set; }
    }
}
