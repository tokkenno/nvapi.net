using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nvidia.Interop
{
    public static partial class Nvapi
    {
        #region Function IDs
        private const uint NvId_GPU_GetThermalSettings = 0xE3640A56;
        private const uint NvId_GPU_GetFullName = 0xCEEE8E9F;
        private const uint NvId_GPU_GetTachReading = 0x5F608315;
        private const uint NvId_GPU_GetAllClocks = 0x1BD69F49;
        private const uint NvId_GPU_GetPStates = 0x60DED2ED;
        private const uint NvId_GPU_GetUsages = 0x189A1FDF;
        private const uint NvId_GPU_GetCoolerSettings = 0xDA141340;
        private const uint NvId_GPU_SetCoolerLevels = 0x891FA0AE;
        private const uint NvId_GPU_GetMemoryInfo = 0x774AA982;
        private const uint NvId_GPU_GetPCIIdentifiers = 0x2DDFB66E;
        #endregion
    }
}
