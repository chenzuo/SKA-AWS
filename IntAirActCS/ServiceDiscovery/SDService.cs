using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceDiscovery
{

    /// <summary>
    /// Represents a service found on the network.
    /// </summary>
    public class SDService
    {
        /// <summary>
        /// The name of the service.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// The hostname of the service.
        /// </summary>
        public String Hostname { get; private set; }

        /// <summary>
        /// The port of the service.
        /// </summary>
        public ushort Port { get; private set; }

        /// <summary>
        /// The type of the service.
        /// </summary>
        public String Type { get; private set; }

        /// <summary>
        /// The TXT record of the service.
        /// </summary>
        public Dictionary<String, String> TXTRecord { get; private set; }

        /// <summary>
        /// Construct a new service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="hostname">The hostname of the service.</param>
        /// <param name="port">The name of the service.</param>
        /// <param name="type">The type of the service.</param>
        /// <param name="TXTRecord">The TXT record of the service.</param>
        /// <returns>The new service.</returns>
        public SDService(String name, String hostname, ushort port, String type, Dictionary<String, String> TXTRecord)
        {
            this.Name = name;
            this.Hostname = hostname;
            this.Port = port;
            this.Type = type;
            this.TXTRecord = TXTRecord;
        }

        public override string ToString()
        {
            return String.Format("SDService[Name: {0}, Hostname: {1}, Port: {2}, Type: {3}, TXTRecord: {4}]", Name, Hostname, Port, Type, TXTRecord);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as SDService) == null)
            {
                return false;
            }

            SDService response = (SDService)obj;
            return (this.Name == response.Name || (this.Name != null && this.Name.Equals(response.Name)));
        }

        public override int GetHashCode()
        {
            int hash = 77;
            hash = hash * 31 + (this.Name == null ? 0 : this.Name.GetHashCode());
            return hash;
        }

    }
}
