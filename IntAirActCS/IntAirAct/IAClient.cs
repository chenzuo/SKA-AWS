using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirAct
{
    public interface IAClient
    {
        void SendRequest(IARequest request, IADevice device);

        void SendRequest(IARequest request, IADevice device, Action<IAResponse, Exception> action);
    }
}
