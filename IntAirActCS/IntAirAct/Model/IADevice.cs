using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace IntAirAct
{
    public class IADevice
    {
        [JsonProperty("name")]
        public String Name { get; private set; }

        [JsonProperty("host")]
        public String Host { get; private set; }

        [JsonProperty("port")]
        public ushort Port { get; private set; }

        [JsonProperty("supportedRoutes")]
        public HashSet<IARoute> SupportedRoutes { get; private set; }

        public IADevice(string name, string host, ushort port, HashSet<IARoute> supportedRoutes)
        {
            this.Name = name;
            this.Host = host;
            this.Port = port;
            this.SupportedRoutes = supportedRoutes;
        }

        public override string ToString()
        {
            return String.Format("IADevice[Name: {0}, Host: {1}, Port: {2}, SupportedRoutes: {3}]", this.Name, this.Host, this.Port, this.SupportedRoutes);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IADevice) == null)
            {
                return false;
            }

            IADevice device = (IADevice) obj;
            return this.Name != null && this.Name.Equals(device.Name);
        }

        public override int GetHashCode()
        {
            int hash = 87;
            hash = hash * 31 + (this.Name == null ? 0 : this.Name.GetHashCode());
            return hash;
        }
    }
}
