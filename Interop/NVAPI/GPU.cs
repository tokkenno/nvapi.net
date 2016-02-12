using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nvidia.Interop
{
    public partial class Nvapi
    {
        #region Function IDs
        protected const uint NvId_GPU_GetThermalSettings = 0xE3640A56;
        protected const uint NvId_GPU_GetFullName = 0xCEEE8E9F;
        protected const uint NvId_GPU_GetTachReading = 0x5F608315;
        protected const uint NvId_GPU_GetAllClocks = 0x1BD69F49;
        protected const uint NvId_GPU_GetPStates = 0x60DED2ED;
        protected const uint NvId_GPU_GetUsages = 0x189A1FDF;
        protected const uint NvId_GPU_GetCoolerSettings = 0xDA141340;
        protected const uint NvId_GPU_SetCoolerLevels = 0x891FA0AE;
        protected const uint NvId_GPU_GetMemoryInfo = 0x774AA982;
        protected const uint NvId_GPU_GetPCIIdentifiers = 0x2DDFB66E;
        #endregion
    }
}
