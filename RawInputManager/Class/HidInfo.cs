using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawInputManager.Enum;

namespace RawInputManager.Class
{
    /// <summary>
	/// Defines the raw input data coming from the specified Human Interface Device (HID). 
	/// </summary>
	// Token: 0x0200000B RID: 11
	public class HidInfo : DeviceInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.RawInput.HidInfo" /> class.
		/// </summary>
		// Token: 0x06000038 RID: 56 RVA: 0x000027D9 File Offset: 0x000009D9
		public HidInfo()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.RawInput.HidInfo" /> class.
		/// </summary>
		/// <param name="rawDeviceInfo">The raw device info.</param>
		/// <param name="deviceName">Name of the device.</param>
		/// <param name="deviceHandle">The device handle.</param>
		// Token: 0x06000039 RID: 57 RVA: 0x000027E4 File Offset: 0x000009E4
		internal HidInfo(ref Structure.DeviceInfo rawDeviceInfo, string deviceName, IntPtr deviceHandle) : base(ref rawDeviceInfo, deviceName, deviceHandle)
		{
			this.VendorId = (int)rawDeviceInfo.HIDInfo.VendorID;
			this.ProductId = (int)rawDeviceInfo.HIDInfo.ProductID;
			this.VersionNumber = (int)rawDeviceInfo.HIDInfo.VersionNumber;
			this.UsagePage = rawDeviceInfo.HIDInfo.UsagePage;
			this.Usage = rawDeviceInfo.HIDInfo.Usage;
		}

		/// <summary>
		/// Gets or sets the vendor id.
		/// </summary>
		/// <value>
		/// The vendor id.
		/// </value>
		/// <unmanaged>unsigned int dwVendorId</unmanaged>
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000284F File Offset: 0x00000A4F
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002857 File Offset: 0x00000A57
		public int VendorId { get; set; }

		/// <summary>
		/// Gets or sets the product id.
		/// </summary>
		/// <value>
		/// The product id.
		/// </value>
		/// <unmanaged>unsigned int dwProductId</unmanaged>
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002860 File Offset: 0x00000A60
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002868 File Offset: 0x00000A68
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the version number.
		/// </summary>
		/// <value>
		/// The version number.
		/// </value>
		/// <unmanaged>unsigned int dwVersionNumber</unmanaged>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002871 File Offset: 0x00000A71
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002879 File Offset: 0x00000A79
		public int VersionNumber { get; set; }

		/// <summary>
		/// Gets or sets the usage page.
		/// </summary>
		/// <value>
		/// The usage page.
		/// </value>
		/// <unmanaged>HID_USAGE_PAGE usUsagePage</unmanaged>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002882 File Offset: 0x00000A82
		// (set) Token: 0x06000041 RID: 65 RVA: 0x0000288A File Offset: 0x00000A8A
		public HidUsagePage UsagePage { get; set; }

		/// <summary>
		/// Gets or sets the usage.
		/// </summary>
		/// <value>
		/// The usage.
		/// </value>
		/// <unmanaged>HID_USAGE_ID usUsage</unmanaged>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002893 File Offset: 0x00000A93
		// (set) Token: 0x06000043 RID: 67 RVA: 0x0000289B File Offset: 0x00000A9B
		public HidUsage Usage { get; set; }
	}
}
