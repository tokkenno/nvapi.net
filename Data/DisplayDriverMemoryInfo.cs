using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Nvidia.Data
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal class DisplayDriverMemoryInfoV1
    {
        /// <summary>
        /// Version info.
        /// </summary>
        public uint version;
        /// <summary>
        /// Size(in kb) of the physical framebuffer.
        /// </summary>
        public uint dedicatedVideoMemory;
        /// <summary>
        /// Size(in kb) of the available physical framebuffer for allocating video memory surfaces.
        /// </summary>
        public uint availableDedicatedVideoMemory;
        /// <summary>
        /// Size(in kb) of shared system memory that driver is allowed to commit for surfaces across all allocations.
        /// </summary>
        public uint systemVideoMemory;
        /// <summary>
        /// Size(in kb) of system memory the driver allocates at load time.
        /// </summary>
        public uint sharedSystemMemory;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal class DisplayDriverMemoryInfoV2 : DisplayDriverMemoryInfoV1
    {
        /// <summary>
        /// Size(in kb) of the current available physical framebuffer for allocating video memory surfaces.
        /// </summary>
        public uint curAvailableDedicatedVideoMemory;
    }
}
