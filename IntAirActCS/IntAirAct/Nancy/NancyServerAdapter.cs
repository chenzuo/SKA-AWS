using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyIoC;
using Nancy.Routing;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;

namespace IntAirAct
{
    public class NancyServerAdapter : IAServer
    {
        public Dictionary<IARoute, Action<IARequest, IAResponse>> Routes { get; set; }
        public Owin.AppDelegate App { get; set; }
        public ushort Port { get; set; }
        private IDisposable server;
        private NancyRebuildableCache cache;

        public NancyServerAdapter()
        {
            this.Routes = new Dictionary<IARoute, Action<IARequest, IAResponse>>();
            this.Port = 0;
        }

        private NancyRebuildableCache resolveCache()
        {
            if (cache == null)
            {
                cache = TinyIoCContainer.Current.Resolve<IRouteCache>() as NancyRebuildableCache;
            }
            return cache;
        }

        public void Start()
        {
            if (Port == 0)
            {
                Port = TcpPort.FindNextAvailablePort(12345);
            }

            server = new Gate.Hosts.Firefly.ServerFactory().Create(App, Port);
        }

        public void Stop()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        public void Route(IARoute route, Action<IARequest, IAResponse> action)
        {
            Routes.Add(route, action);
            resolveCache().RebuildCache();
        }

        public static class TcpPort
        {
            private const string PortReleaseGuid =
                "8875BD8E-4D5B-11DE-B2F4-691756D89593";

            /// <summary>
            /// Check if startPort is available, incrementing and
            /// checking again if it's in use until a free port is found
            /// </summary>
            /// <param name="startPort">The first port to check</param>
            /// <returns>The first available port</returns>
            public static ushort FindNextAvailablePort(ushort startPort)
            {
                ushort port = startPort;
                bool isAvailable = true;

                var mutex = new Mutex(false,
                    string.Concat("Global/", PortReleaseGuid));
                mutex.WaitOne();
                try
                {
                    IPGlobalProperties ipGlobalProperties =
                        IPGlobalProperties.GetIPGlobalProperties();
                    IPEndPoint[] endPoints =
                        ipGlobalProperties.GetActiveTcpListeners();

                    do
                    {
                        if (!isAvailable)
                        {
                            port++;
                            isAvailable = true;
                        }

                        foreach (IPEndPoint endPoint in endPoints)
                        {
                            if (endPoint.Port != port) continue;
                            isAvailable = false;
                            break;
                        }

                    } while (!isAvailable && port < IPEndPoint.MaxPort);

                    if (!isAvailable)
                        throw new Exception();

                    return port;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}
