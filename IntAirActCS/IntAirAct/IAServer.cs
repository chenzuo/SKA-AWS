using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirAct
{
    public interface IAServer
    {
        ushort Port { get; set; }

        void Start();

        void Stop();

        void Route(IARoute route, Action<IARequest, IAResponse> action);
    }
}
