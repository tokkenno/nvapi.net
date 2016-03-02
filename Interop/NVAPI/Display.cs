using Nvidia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Nvidia.Interop
{
    public static partial class Nvapi
    {
        #region Function IDs
        private const uint NvId_EnumNvidiaDisplayHandle = 0x9ABDD40D;
        private const uint NvId_EnumNvidiaUnAttachedDisplayHandle = 0x20DE9260;
        private const uint NvId_GetAssociatedNvidiaDisplayHandle = 0x35C29134;
        private const uint NvId_GetAssociatedUnAttachedNvidiaDisplayHandle = 0xA70503B2;
        #endregion

        #region Display NVAPI Functions
        // EnumNvidiaDisplayHandle
        private delegate Status EnumNvidiaDisplayHandleDelegate(int thisEnum, ref DisplayHandle displayHandle);
        private static readonly EnumNvidiaDisplayHandleDelegate EnumNvidiaDisplayHandleInternal;

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
        private delegate Status EnumNvidiaUnAttachedDisplayHandleDelegate(int thisEnum, ref UnAttachedDisplayHandle pNvDispHandle);
        private static readonly EnumNvidiaUnAttachedDisplayHandleDelegate EnumNvidiaUnAttachedDisplayHandleInternal;

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
        private delegate Status GetAssociatedNvidiaDisplayHandleDelegate(StringBuilder szDisplayName, ref DisplayHandle pNvDispHandle);
        private static readonly GetAssociatedNvidiaDisplayHandleDelegate GetAssociatedNvidiaDisplayHandleInternal;

        /// <summary>
        /// This function returns the handle of the NVIDIA display that is associated with the given display "name" (such as "\\.\DISPLAY1").
        /// </summary>
        /// <param name="szDisplayName"></param>
        /// <param name="pNvDispHandle"></param>
        /// <returns></returns>
        public static Status GetAssociatedNvidiaDisplayHandle(string szDisplayName, ref DisplayHandle pNvDispHandle)
        {
            StringBuilder builder = new StringBuilder((int)NvShortStringMax);
            builder.Append(szDisplayName);

            Status status;
            if (GetAssociatedNvidiaDisplayHandleInternal != null) { status = GetAssociatedNvidiaDisplayHandleInternal(builder, ref pNvDispHandle); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            szDisplayName = builder.ToString();

            return status;
        }

        // GetAssociatedUnAttachedNvidiaDisplayHandle
        private delegate Status GetAssociatedUnAttachedNvidiaDisplayHandleDelegate(StringBuilder szDisplayName, ref UnAttachedDisplayHandle pNvUnAttachedDispHandle);
        private static readonly GetAssociatedUnAttachedNvidiaDisplayHandleDelegate GetAssociatedUnAttachedNvidiaDisplayHandleInternal;

        /// <summary>
        /// This function returns the handle of an unattached NVIDIA display that is associated with the given display name (such as "\\DISPLAY1").
        /// </summary>
        /// <param name="szDisplayName"></param>
        /// <param name="pNvUnAttachedDispHandle"></param>
        /// <returns></returns>
        public static Status GetAssociatedUnAttachedNvidiaDisplayHandle(string szDisplayName, ref UnAttachedDisplayHandle pNvUnAttachedDispHandle)
        {
            StringBuilder builder = new StringBuilder((int)NvShortStringMax);
            builder.Append(szDisplayName);

            Status status;
            if (GetAssociatedUnAttachedNvidiaDisplayHandleInternal != null) { status = GetAssociatedUnAttachedNvidiaDisplayHandleInternal(builder, ref pNvUnAttachedDispHandle); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            szDisplayName = builder.ToString();

            Console.WriteLine(pNvUnAttachedDispHandle.ptr);

            return status;
        }
        #endregion
    }
}
