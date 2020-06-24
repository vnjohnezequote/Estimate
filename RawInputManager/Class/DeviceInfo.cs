using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawInputManager.Enum;

namespace RawInputManager.Class
{
    public class DeviceInfo
    {
        public string DeviceName { get; set; }
        public RawInputType DeviceType { get; set; }
        public IntPtr DeviceHandle { get; set; }
        public string Description { get; set; }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.RawInput.DeviceInfo" /> class.
        /// </summary>
        // Token: 0x0600002F RID: 47 RVA: 0x00002093 File Offset: 0x00000293
        public DeviceInfo()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.RawInput.DeviceInfo" /> class.
        /// </summary>
        /// <param name="rawDeviceInfo">The raw device info.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceHandle">The device handle.</param>
        // Token: 0x06000030 RID: 48 RVA: 0x0000270C File Offset: 0x0000090C
        internal DeviceInfo(ref Structure.DeviceInfo rawDeviceInfo, string deviceName, IntPtr deviceHandle)
        {
            this.DeviceName = deviceName;
            this.DeviceHandle = deviceHandle;
            this.DeviceType = rawDeviceInfo.Type;
        }
        internal static DeviceInfo Convert(ref Structure.DeviceInfo rawDeviceInfo, string deviceName, IntPtr deviceHandle)
        {
            DeviceInfo deviceInfo;
            switch (rawDeviceInfo.Type)
            {
                case RawInputType.Mouse:
                    deviceInfo = new MouseInfo(ref rawDeviceInfo, deviceName, deviceHandle);
                    break;
                case RawInputType.Keyboard:
                    deviceInfo = new KeyboardInfo(ref rawDeviceInfo, deviceName, deviceHandle);
                    break;
                case RawInputType.HID:
                    deviceInfo = new HidInfo(ref rawDeviceInfo, deviceName, deviceHandle);
                    break;
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unsupported Device Type [{0}]", new object[]
                    {
                        (int)rawDeviceInfo.Type
                    }));
            }
            return deviceInfo;
        }
    }
}
