using Nvidia.Data;
using Nvidia.Exceptions;
using Nvidia.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nvidia
{
    public static class Display
    {
        public static IList<Nvapi.DisplayHandle> GetHandlers()
        {
            IList<Nvapi.DisplayHandle> displayHandlers = new List<Nvapi.DisplayHandle>();

            Status status = Status.NVAPI_OK;
            int index = 0;

            while (status != Status.NVAPI_END_ENUMERATION)
            {
                Nvapi.DisplayHandle displayHandle = new Nvapi.DisplayHandle();

                status = Nvapi.EnumNvidiaDisplayHandle(index, ref displayHandle);
                index++;

                if (status == Status.NVAPI_OK)
                {
                    displayHandlers.Add(displayHandle);
                }
                else if (index == 0 && status != Status.NVAPI_NVIDIA_DEVICE_NOT_FOUND) { return new List<Nvapi.DisplayHandle>(); }
                else if (status != Status.NVAPI_END_ENUMERATION) { throw new NvidiaApiException(status); }
            }

            return displayHandlers;
        }

        public static IList<Nvapi.UnAttachedDisplayHandle> GetUnAttachedHandlers()
        {
            IList<Nvapi.UnAttachedDisplayHandle> displayHandlers = new List<Nvapi.UnAttachedDisplayHandle>();

            Status status = Status.NVAPI_OK;
            int index = 0;

            while (status != Status.NVAPI_END_ENUMERATION)
            {
                Nvapi.UnAttachedDisplayHandle displayHandle = new Nvapi.UnAttachedDisplayHandle();

                status = Nvapi.EnumNvidiaUnAttachedDisplayHandle(index, ref displayHandle);
                index++;

                if (status == Status.NVAPI_OK)
                {
                    displayHandlers.Add(displayHandle);
                }
                else if (index == 0 && status != Status.NVAPI_NVIDIA_DEVICE_NOT_FOUND) { return new List<Nvapi.UnAttachedDisplayHandle>(); }
                else if (status != Status.NVAPI_END_ENUMERATION) { throw new NvidiaApiException(status); }
            }

            return displayHandlers;
        }

        public static String GetDisplayName(Nvapi.DisplayHandle ptr)
        {
            String name;
            Nvapi.GetAssociatedNvidiaDisplayHandle(out name, ref ptr);
            return name;
        }

        public static String GetUnAttachedDisplayName(Nvapi.UnAttachedDisplayHandle ptr)
        {
            String name;
            Nvapi.GetAssociatedUnAttachedNvidiaDisplayHandle(out name, ref ptr);
            return name;
        }
    }
}
