using System;
using System.Runtime.InteropServices;

namespace Nvidia.Data
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct BoardInfo
    {
        public uint version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] BoardNum;
    }
}
