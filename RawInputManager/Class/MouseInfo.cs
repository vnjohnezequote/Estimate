using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawInputManager.Class
{
    public class MouseInfo : DeviceInfo
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.RawInput.MouseInfo" /> class.
		/// </summary>
		// Token: 0x06000066 RID: 102 RVA: 0x000027D9 File Offset: 0x000009D9
		public MouseInfo()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.RawInput.MouseInfo" /> class.
		/// </summary>
		/// <param name="rawDeviceInfo">The raw device info.</param>
		/// <param name="deviceName">Name of the device.</param>
		/// <param name="deviceHandle">The device handle.</param>
		// Token: 0x06000067 RID: 103 RVA: 0x00002B4C File Offset: 0x00000D4C
		internal MouseInfo(ref Structure.DeviceInfo rawDeviceInfo, string deviceName, IntPtr deviceHandle) : base(ref rawDeviceInfo, deviceName, deviceHandle)
		{
			this.Id = (int)rawDeviceInfo.MouseInfo.Id;
			this.ButtonCount = (int)rawDeviceInfo.MouseInfo.NumberOfButtons;
			this.SampleRate = (int)rawDeviceInfo.MouseInfo.SampleRate;
			this.HasHorizontalWheel = rawDeviceInfo.MouseInfo.HasHorizontalWheel;
		}

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		/// <unmanaged>unsigned int dwId</unmanaged>	
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002BAB File Offset: 0x00000DAB
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002BB3 File Offset: 0x00000DB3
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the button count.
		/// </summary>
		/// <value>
		/// The button count.
		/// </value>
		/// <unmanaged>unsigned int dwNumberOfButtons</unmanaged>	
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002BBC File Offset: 0x00000DBC
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public int ButtonCount { get; set; }

		/// <summary>
		/// Gets or sets the sample rate.
		/// </summary>
		/// <value>
		/// The sample rate.
		/// </value>
		/// <unmanaged>unsigned int dwSampleRate</unmanaged>	
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002BCD File Offset: 0x00000DCD
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002BD5 File Offset: 0x00000DD5
		public int SampleRate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance has horizontal wheel.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has horizontal wheel; otherwise, <c>false</c>.
		/// </value>
		/// <unmanaged>BOOL fHasHorizontalWheel</unmanaged>	
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002BDE File Offset: 0x00000DDE
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002BE6 File Offset: 0x00000DE6
		public bool HasHorizontalWheel { get; set; }
    }
}
