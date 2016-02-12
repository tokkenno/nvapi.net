using Nvidia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Nvidia.Interop
{
    public partial class Nvapi
    {
        #region Defines
        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayHandle
        {
            private readonly IntPtr ptr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UnAttachedDisplayHandle
        {
            private readonly IntPtr ptr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PhysicalGpuHandle
        {
            private readonly IntPtr ptr;
        }
        #endregion

        #region Function IDs
        protected const uint NvId_EnumNvidiaDisplayHandle = 0x9ABDD40D;
        protected const uint NvId_EnumNvidiaUnAttachedDisplayHandle = 0x20DE9260;
        protected const uint NvId_GetAssociatedNvidiaDisplayHandle = 0x35C29134;
        protected const uint NvId_GetAssociatedUnAttachedNvidiaDisplayHandle = 0xA70503B2;
        #endregion

        #region Display NVAPI Functions
        // EnumNvidiaDisplayHandle
        protected delegate Status EnumNvidiaDisplayHandleDelegate(int thisEnum, ref DisplayHandle displayHandle);
        protected static readonly EnumNvidiaDisplayHandleDelegate EnumNvidiaDisplayHandleInternal;

        /// <summary>
        /// This function returns the handle of the NVIDIA display specified by the enum index (thisEnum). The client should keep enumerating until it returns NVAPI_END_ENUMERATION.
        /// Note: Display handles can get invalidated on a modeset, so the calling applications need to renum the handles after every modeset.
        /// </summary>
        /// <param name="thisEnum">The index of the NVIDIA display.</param>
        /// <param name="displayHandle">Pointer to the NVIDIA display handle.</param>
        /// <returns></returns>
        public static Status EnumNvidiaDisplayHandle(int thisEnum, ref DisplayHandle displayHandle)
        {
            Status status;
            if (EnumNvidiaDisplayHandleInternal != null) { status = EnumNvidiaDisplayHandleInternal(thisEnum, ref displayHandle); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            return status;
        }

        // EnumNvidiaUnAttachedDisplayHandle
        protected delegate Status EnumNvidiaUnAttachedDisplayHandleDelegate(int thisEnum, ref UnAttachedDisplayHandle pNvDispHandle);
        protected static readonly EnumNvidiaUnAttachedDisplayHandleDelegate EnumNvidiaUnAttachedDisplayHandleInternal;

        /// <summary>
        /// This function returns the handle of the NVIDIA unattached display specified by the enum index (thisEnum). The client should keep enumerating until it returns error. Note: Display handles can get invalidated on a modeset, so the calling applications need to renum the handles after every modeset.
        /// </summary>
        /// <param name="thisEnum">The index of the NVIDIA display.</param>
        /// <param name="pNvDispHandle">Pointer to the NVIDIA display handle of the unattached display.</param>
        /// <returns></returns>
        public static Status EnumNvidiaUnAttachedDisplayHandle(int thisEnum, ref UnAttachedDisplayHandle pNvDispHandle)
        {
            Status status;
            if (EnumNvidiaUnAttachedDisplayHandleInternal != null) { status = EnumNvidiaUnAttachedDisplayHandleInternal(thisEnum, ref pNvDispHandle); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            return status;
        }

        // GetAssociatedUnAttachedNvidiaDisplayHandle
        protected delegate Status GetAssociatedNvidiaDisplayHandleDelegate(StringBuilder szDisplayName, ref DisplayHandle pNvDispHandle);
        protected static readonly GetAssociatedNvidiaDisplayHandleDelegate GetAssociatedNvidiaDisplayHandleInternal;

        /// <summary>
        /// This function returns the handle of the NVIDIA display that is associated with the given display "name" (such as "\\.\DISPLAY1").
        /// </summary>
        /// <param name="szDisplayName"></param>
        /// <param name="pNvDispHandle"></param>
        /// <returns></returns>
        public static Status GetAssociatedNvidiaDisplayHandle(out string szDisplayName, ref DisplayHandle pNvDispHandle)
        {
            StringBuilder builder = new StringBuilder((int)NvShortStringMax);

            Status status;
            if (GetAssociatedNvidiaDisplayHandleInternal != null) { status = GetAssociatedNvidiaDisplayHandleInternal(builder, ref pNvDispHandle); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            szDisplayName = builder.ToString();

            return status;
        }

        // GetAssociatedUnAttachedNvidiaDisplayHandle
        protected delegate Status GetAssociatedUnAttachedNvidiaDisplayHandleDelegate(StringBuilder szDisplayName, ref UnAttachedDisplayHandle pNvUnAttachedDispHandle);
        protected static readonly GetAssociatedUnAttachedNvidiaDisplayHandleDelegate GetAssociatedUnAttachedNvidiaDisplayHandleInternal;

        /// <summary>
        /// This function returns the handle of an unattached NVIDIA display that is associated with the given display name (such as "\\DISPLAY1").
        /// </summary>
        /// <param name="szDisplayName"></param>
        /// <param name="pNvUnAttachedDispHandle"></param>
        /// <returns></returns>
        public static Status GetAssociatedUnAttachedNvidiaDisplayHandle(out string szDisplayName, ref UnAttachedDisplayHandle pNvUnAttachedDispHandle)
        {
            StringBuilder builder = new StringBuilder((int)NvShortStringMax);

            Status status;
            if (GetAssociatedUnAttachedNvidiaDisplayHandleInternal != null) { status = GetAssociatedUnAttachedNvidiaDisplayHandleInternal(builder, ref pNvUnAttachedDispHandle); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            szDisplayName = builder.ToString();

            return status;
        }
        #endregion
    }
}
