using Nvidia.Data;
using Nvidia.Interop;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Nvidia.Interop
{
    public static partial class Nvapi
    {
        #region Defines
        public const uint NvMaxHeads = 4;
        public const uint NvMaxVidProfiles = 4;
        public const uint NvMaxVidStreams = 4;
        public const uint NvAdvancedDisplayHeads = 4;
        public const uint NvGenericStringMax = 4096;
        public const uint NvLongStringMax = 256;
        public const uint NvMaxAcpiIds = 16;
        public const uint NvMaxAudioDevices = 16;
        public const uint NvMaxAvailableCPUTopologies = 256;
        public const uint NvMaxAvailableSLIGroups = 256;
        public const uint NvMaxDisplayHeads = 2;
        public const uint NvMaxDisplays = NvPhysicalGPUs * NvAdvancedDisplayHeads;
        public const uint NvMaxGPUPerTopology = 8;
        public const uint NvMaxGPUTopologies = NvMaxPhysicalGPUs;
        public const uint NvMaxHeadsPerGPU = 32;
        public const uint NvMaxLogicalGPUs = 64;
        public const uint NvMaxPhysicalBridges = 100;
        public const uint NvMaxPhysicalGPUs = 64;
        public const uint NvMaxViewModes = 8;
        public const uint NvPhysicalGPUs = 32;
        public const uint NvShortStringMax = 64;
        public const uint NvSystemHWBCInvalidId = 0xffffffff;
        public const uint NvSystemMaxDisplays = NvMaxPhysicalGPUs * NvMaxHeads;
        public const uint NvSystemMaxHWBCs = 128;

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayHandle
        {
            private readonly IntPtr ptr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UnAttachedDisplayHandle
        {
            public readonly IntPtr ptr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PhysicalGpuHandle
        {
            private readonly IntPtr ptr;
        }
        #endregion

        #region Function IDs
        private const uint NvId_Initialize                = 0x0150E828;
        private const uint NvId_Unload                    = 0xD22BDD7E;
        private const uint NvId_GetErrorMessage           = 0x6C2D048C;
        private const uint NvId_GetInterfaceVersionString = 0x01053FA5;
        #endregion

        #region General NVAPI Functions
        // QueryInterface
        private delegate IntPtr QueryInterfaceDelegate(uint id);
        private static readonly QueryInterfaceDelegate QueryInterface;

        // Initialize
        private delegate Status InitializeDelegate();
        private static readonly InitializeDelegate InitializeInternal;

        // Unload
        private delegate Status UnloadDelegate();
        private static readonly UnloadDelegate UnloadInternal;

        // GetErrorMessage
        private delegate Status GetErrorMessageDelegate(Status nr, StringBuilder szDesc);
        private static readonly GetErrorMessageDelegate GetErrorMessageInternal;

        /// <summary>
        /// This function converts an NvAPI error code into a null terminated string.
        /// </summary>
        /// <param name="nr">The error code to convert</param>
        /// <param name="szDesc">The string corresponding to the error code</param>
        /// <returns></returns>
        public static Status GetErrorMessage(Status nr, out string szDesc)
        {
            StringBuilder builder = new StringBuilder((int)NvShortStringMax);

            Status status;
            if (GetErrorMessageInternal != null) { status = GetErrorMessageInternal(nr, builder); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            szDesc = builder.ToString();

            return status;
        }

        // GetInterfaceVersionString
        private delegate Status GetInterfaceVersionStringDelegate(StringBuilder szDesc);
        private static readonly GetInterfaceVersionStringDelegate GetInterfaceVersionStringInternal;

        /// <summary>
        /// This function returns a string describing the version of the NvAPI library. The contents of the string are human readable. Do not assume a fixed format.
        /// </summary>
        /// <param name="szDesc">User readable string giving NvAPI version information</param>
        /// <returns></returns>
        public static Status GetInterfaceVersionString(out string szDesc)
        {
            StringBuilder builder = new StringBuilder((int)NvShortStringMax);

            Status status;
            if (GetErrorMessageInternal != null) { status = GetInterfaceVersionStringInternal(builder); }
            else { status = Status.NVAPI_FUNCTION_NOT_FOUND; }
            szDesc = builder.ToString();

            return status;
        }
        #endregion

        #region Initialization code
        private static bool available = false;

        public static bool IsAvailable() { return Nvapi.available; }

        static Nvapi()
        {
            DllImportAttribute attribute = new DllImportAttribute(GetDllName());
            attribute.CallingConvention = CallingConvention.Cdecl;
            attribute.PreserveSig = true;
            attribute.EntryPoint = "nvapi_QueryInterface";
            PInvokeDelegateFactory.CreateDelegate(attribute, out QueryInterface);

            try
            {
                GetDelegate(NvId_Initialize, out UnloadInternal);
            }
            catch (DllNotFoundException) { return; }
            catch (EntryPointNotFoundException) { return; }
            catch (ArgumentNullException) { return; }

            if (UnloadInternal() == Status.NVAPI_OK)
            {
                GetDelegate(NvId_Unload, out UnloadInternal);
                GetDelegate(NvId_GetInterfaceVersionString, out GetInterfaceVersionStringInternal);
                GetDelegate(NvId_GetErrorMessage, out GetErrorMessageInternal);

                // Display
                GetDelegate(NvId_EnumNvidiaDisplayHandle, out EnumNvidiaDisplayHandleInternal);
                GetDelegate(NvId_EnumNvidiaUnAttachedDisplayHandle, out EnumNvidiaUnAttachedDisplayHandleInternal);
                GetDelegate(NvId_GetAssociatedNvidiaDisplayHandle, out GetAssociatedNvidiaDisplayHandleInternal);
                GetDelegate(NvId_GetAssociatedUnAttachedNvidiaDisplayHandle, out GetAssociatedUnAttachedNvidiaDisplayHandleInternal);

                available = true;
            }

            AppDomain.CurrentDomain.ProcessExit += Nvapi.OnExit;
        }

        private static void GetDelegate<T>(uint id, out T newDelegate) where T : class
        {
            IntPtr ptr = QueryInterface(id);
            if (ptr != IntPtr.Zero)
            {
                newDelegate = Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
            }
            else
            {
                newDelegate = null;
            }
        }

        private static string GetDllName()
        {
            if (IntPtr.Size == 4)
            {
                return "nvapi.dll";
            }
            else
            {
                return "nvapi64.dll";
            }
        }

        private static void OnExit(object sender, EventArgs e)
        {
            available = false;

            if (Nvapi.UnloadInternal != null) { Nvapi.UnloadInternal(); }
        }
        #endregion
    }
}
