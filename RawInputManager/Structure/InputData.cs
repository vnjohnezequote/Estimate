using System.Runtime.InteropServices;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct InputData
    {
        
        public RawInputHeader header; // 64 bit header size is 24  32 bit the header size is 16
        public RawData data; // Creating the rest in a struct allows the header size to align correctly for 32 or 64 bit
    
    }
}