using Nvidia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nvidia.Exceptions
{
    public class NvidiaApiException : Exception
    {
        private Status _status;

        public NvidiaApiException(Status stat) : base(GetErrorMessage(stat))
        {
            this._status = stat;
        }

        private static String GetErrorMessage(Status stat)
        {
            String msg;
            Status status = Interop.Nvapi.GetErrorMessage(stat, out msg);

            if (status == Status.NVAPI_OK)
            {
                return Enum.GetName(typeof(Status), stat) + " => " + msg;
            }
            else
            {
                return Enum.GetName(typeof(Status), stat);
            }
        }

        public Status Status { get { return this._status; } }
    }
}
