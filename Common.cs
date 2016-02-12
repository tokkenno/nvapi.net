using Nvidia.Data;
using Nvidia.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nvidia
{
    public static class Common
    {
        private static String _interface_version = null;

        public static String InterfaceVersion {
            get {
                if (_interface_version == null)
                {
                    Status status = Interop.Nvapi.GetInterfaceVersionString(out _interface_version);
                    if (status != Status.NVAPI_OK) { throw new NvidiaApiException(status); }
                }

                return _interface_version;
            }
        }
    }
}
