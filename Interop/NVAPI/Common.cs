using Nvidia.Data;
using Nvidia.Interop;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Nvidia.Interop
{
    public partial class Nvapi
    {
        #region Defines
        protected const uint NvMaxHeads = 4;
        protected const uint NvMaxVidProfiles = 4;
        protected const uint NvMaxVidStreams = 4;
        protected const uint NvAdvancedDisplayHeads = 4;
        protected const uint NvGenericStringMax = 4096;
        protected const uint NvLongStringMax = 256;
        protected const uint NvMaxAcpiIds = 16;
        protected const uint NvMaxAudioDevices = 16;
        protected const uint NvMaxAvailableCPUTopologies = 256;
        protected const uint NvMaxAvailableSLIGroups = 256;
        protected const uint NvMaxDisplayHeads = 2;
        protected const uint NvMaxDisplays = NvPhysicalGPUs * NvAdvancedDisplayHeads;
        protected const uint NvMaxGPUPerTopology = 8;
        protected const uint NvMaxGPUTopologies = NvMaxPhysicalGPUs;
        protected const uint NvMaxHeadsPerGPU = 32;
        protected const uint NvMaxLogicalGPUs = 64;
        protected const uint NvMaxPhysicalBridges = 100;
        public const uint NvMaxPhysicalGPUs = 64;
        protected const uint NvMaxViewModes = 8;
        protected const uint NvPhysicalGPUs = 32;
        protected const uint NvShortStringMax = 64;
        protected const uint NvSystemHWBCInvalidId = 0xffffffff;
        protected const uint NvSystemMaxDisplays = NvMaxPhysicalGPUs * NvMaxHeads;
        protected const uint NvSystemMaxHWBCs = 128;
        #endregion

        #region Function IDs
        protected const uint NvId_Initialize                = 0x0150E828;
        protected const uint NvId_Unload                    = 0xD22BDD7E;
        protected const uint NvId_GetErrorMessage           = 0x6C2D048C;
        protected const uint NvId_GetInterfaceVersionString = 0x01053FA5;
        #endregion

        #region General NVAPI Functions
        // QueryInterface
        protected delegate IntPtr QueryInterfaceDelegate(uint id);
        protected static readonly QueryInterfaceDelegate QueryInterface;

        // Initialize
        protected delegate Status InitializeDelegate();
        protected static readonly InitializeDelegate InitializeInternal;

        // Unload
        protected delegate Status UnloadDelegate();
        protected static readonly UnloadDelegate UnloadInternal;

        // GetErrorMessage
        protected delegate Status GetErrorMessageDelegate(Status nr, StringBuilder szDesc);
        protected static readonly GetErrorMessageDelegate GetErrorMessageInternal;

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
        protected delegate Status GetInterfaceVersionStringDelegate(StringBuilder szDesc);
        protected static readonly GetInterfaceVersionStringDelegate GetInterfaceVersionStringInternal;

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
        protected static bool available = false;

        public bool IsAvailable() { return Nvapi.available; }

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

        protected static void GetDelegate<T>(uint id, out T newDelegate) where T : class
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

        protected static string GetDllName()
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

        protected static void OnExit(object sender, EventArgs e)
        {
            available = false;

            if (Nvapi.UnloadInternal != null) { Nvapi.UnloadInternal(); }
        }
        #endregion
    }
}
