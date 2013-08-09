using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TinyIoC;
using ServiceDiscovery;
using System.Net;
using System.ComponentModel;
using Monads;

namespace IntAirAct
{
    public delegate void DeviceFoundHandler(IADevice device, bool ownDevice);
    public delegate void DeviceLostHandler(IADevice device);

    public class IAIntAirAct : IDisposable
    {
        public bool IsRunning { get; private set; }
        public Option<IADevice> OwnDevice { get; private set; }
        public HashSet<IARoute> SupportedRoutes { get; set; }
        public event DeviceFoundHandler DeviceFound;
        public event DeviceLostHandler DeviceLost;

        private bool isDisposed = false;
        private Dictionary<string, Type> mappings = new Dictionary<string, Type>();
        private IAServer server;
        private IAClient client;
        private SDServiceDiscovery serviceDiscovery;
        private List<IADevice> devices = new List<IADevice>();

        #region Constructor, Deconstructor

        public static IAIntAirAct New()
        {
            // don't mess with the order here, TinyIoC is very picky about it
            TinyIoCContainer container = TinyIoCContainer.Current;
            NancyServerAdapter adapter = new NancyServerAdapter();
            Owin.AppDelegate app = Gate.Adapters.Nancy.NancyAdapter.App();
            adapter.App = app;
            IAClient client = new HttpWebRequestClient();


            // register the server adapter for the module serving the routes
            container.Register<NancyServerAdapter>(adapter);

            IAIntAirAct intAirAct = new IAIntAirAct(adapter, client);
            
            // register IntAirAct for getting the origin devices in NancyModule
            container.Register<IAIntAirAct>(intAirAct);

            return intAirAct;
        }

        public IAIntAirAct(IAServer server, IAClient client)
        {
            this.server = server;
            this.client = client;
            this.serviceDiscovery = new SDServiceDiscovery();
            this.IsRunning = false;
            this.SupportedRoutes = new HashSet<IARoute>();
            Port = 0;
            this.OwnDevice = new None<IADevice>();
            this.Setup();
        }

        ~IAIntAirAct()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                // Code to dispose the managed resources of the class
                Stop();
                if (serviceDiscovery != null)
                {
                    serviceDiscovery.Dispose();
                }
            }
            // Code to dispose the un-managed resources of the class
            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        #region Start, Stop, Setup

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            server.Start();

            serviceDiscovery.PublishService("_intairact._tcp.", Port);
            serviceDiscovery.SearchForServices("_intairact._tcp.");

            IsRunning = true;
        }

        public void Stop()
        {
            if (serviceDiscovery != null)
            {
                serviceDiscovery.Stop();
            }

            if (server != null)
            {
                server.Stop();
            }

            IsRunning = false;
        }

        private void Setup()
        {
            serviceDiscovery.ServiceFound += new ServiceFoundHandler(this.OnServiceFound);
            serviceDiscovery.ServiceLost += new ServiceLostHandler(this.OnServiceLost);
            serviceDiscovery.ServiceDiscoveryError += new ServiceDiscoveryErrorHandler(this.OnServiceDiscoveryError);

            this.Route(new IARoute("GET", "/routes"), delegate(IARequest request, IAResponse response)
            {
                response.SetBodyWith(this.SupportedRoutes);
            });
        }

        #endregion
        #region Properties

        public ushort Port
        {
            get
            {
                return this.server.Port;
            }

            set
            {
                this.server.Port = value;
            }
        }

        public List<IADevice> Devices
        {
            get
            {
                return new List<IADevice>(this.devices);
            }
        }

        #endregion
        #region Methods

        public IEnumerable<IADevice> DevicesSupportingRoute(IARoute route)
        {
            return this.Devices.FindAll(device => device.SupportedRoutes.Contains(route));
        }

        public IADevice DeviceWithName(string name)
        {
            return this.OwnDevice.Where(x => x.Name.Equals(name)).
              GetOrElse(() => this.Devices.Find(device => device.Name.Equals(name)));
        }

        public void Route(IARoute route, Action<IARequest, IAResponse> action)
        {
            SupportedRoutes.Add(route);
            server.Route(route, action);
        }

        public void SendRequest(IARequest request, IADevice device)
        {
            this.client.SendRequest(request, device);
        }

        public void SendRequest(IARequest request, IADevice device, Action<IAResponse, Exception> action)
        {
            this.client.SendRequest(request, device, action);
        }

        public Promise<Try<IAResponse>> Send(IARequest request, IADevice device)
        {
            Try<IAResponse> result = null;
            SendRequest(request, device, delegate(IAResponse response, Exception e)
            {
                result = MetaTry.Try<IAResponse>(() => {
                    if (e != null)
                    {
                        throw new Exception(e.Message, e);
                    }
                    else 
                    {
                        return response;
                    }
                });
            });
            return ((Func<Try<IAResponse>>)delegate()
            {
                while (result == null) ;
                return result;
            }).AsPromise();
        }

        #endregion
        #region Event Handling

        private void OnServiceFound(SDService service, bool ownService)
        {
            if (ownService)
            {
                IADevice device = new IADevice(service.Name, service.Hostname, service.Port, this.SupportedRoutes);
                this.OwnDevice = device.AsOption();
                OnDeviceFound(device, true);
            }
            else
            {
                IADevice device = new IADevice(service.Name, service.Hostname, service.Port, null);
                IARequest request = new IARequest(IARoute.Get("/routes"));
                SendRequest(request, device, delegate(IAResponse response, Exception error)
                {
                    if (error == null)
                    {
                        if (response.StatusCode == 200)
                        {
                            List<IARoute> supportedRoutes = response.BodyAs<IARoute>();
                            IADevice deviceWithRoutes = new IADevice(service.Name, service.Hostname, service.Port, new HashSet<IARoute>(supportedRoutes));
                            this.devices.Add(deviceWithRoutes);
                            OnDeviceFound(deviceWithRoutes, false);
                        }
                        else
                        {
                            Console.WriteLine(String.Format("An error ocurred trying to request routes from {0}", device));
                        }
                    }
                    else
                    {
                        Console.WriteLine(String.Format("An error ocurred: {0}", error));
                    }
                });
            }
        }

        private void OnServiceLost(SDService service)
        {
            IADevice device = new IADevice(service.Name, service.Hostname, service.Port, null);
            this.devices.Remove(device);
            OnDeviceLost(device);
        }

        private void OnServiceDiscoveryError(EventArgs eventArg)
        {
            Console.WriteLine(String.Format("An error ocurred: {0}", eventArg));
        }

        protected virtual void OnDeviceFound(IADevice device, bool ownDevice)
        {
            if (DeviceFound != null)
            {
                DeviceFound(device, ownDevice);
            }
        }

        protected virtual void OnDeviceLost(IADevice device)
        {
            if (DeviceLost != null)
            {
                DeviceLost(device);
            }
        }

        #endregion
    }
}
