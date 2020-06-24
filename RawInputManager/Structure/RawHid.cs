using System.Runtime.InteropServices;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawHid
    {
        public uint dwSizHid;
        public uint dwCount;
        public byte bRawData;

        public override string ToString()
        {
            return string.Format("Rawhib\n dwSizeHid : {0}\n dwCount : {1}\n bRawData : {2}\n", dwSizHid, dwCount,
                bRawData);
        }
    }
}