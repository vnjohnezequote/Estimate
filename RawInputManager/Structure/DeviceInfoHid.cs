﻿using System.Runtime.InteropServices;
using RawInputManager.Enum;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DeviceInfoHid
    {
        public uint VendorID; // Vendor identifier for the HID
        public uint ProductID; // Product identifier for the HID
        public uint VersionNumber; // Version number for the device
        public HidUsagePage UsagePage; // Top-level collection Usage page for the device
        public HidUsage Usage; // Top-level collection Usage for the device

        public override string ToString()
        {
            return
                string.Format(
                    "HidInfo\n VendorID: {0}\n ProductID: {1}\n VersionNumber: {2}\n UsagePage: {3}\n Usage: {4}\n",
                    VendorID, ProductID, VersionNumber, UsagePage, Usage);
        }
    }
}