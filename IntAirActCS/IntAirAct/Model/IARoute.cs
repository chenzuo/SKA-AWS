using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace IntAirAct
{
    public class IARoute
    {
        public static IARoute Put(string resource)
        {
            return new IARoute("PUT", resource);
        }

        public static IARoute Post(string resource)
        {
            return new IARoute("POST", resource);
        }

        public static IARoute Get(string resource)
        {
            return new IARoute("GET", resource);
        }

        public static IARoute Delete(string resource)
        {
            return new IARoute("DELETE", resource);
        }

        [JsonProperty("action")]
        public String Action { get; private set; }

        [JsonProperty("resource")]
        public String Resource { get; private set; }

        public IARoute(string action, string resource)
        {
            this.Action = action;
            this.Resource = resource;
        }

        public override string ToString()
        {
            return String.Format("IARoute[Action: {0}, Resource: {1}]", Action, Resource);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IARoute) == null)
            {
                return false;
            }

            IARoute route = (IARoute)obj;
            return (this.Action != null && this.Action.Equals(route.Action))
                && (this.Resource != null && this.Resource.Equals(route.Resource));
        }

        public override int GetHashCode()
        {
            int hash = 49;
            hash = hash * 31 + (this.Action == null ? 0 : this.Action.GetHashCode());
            hash = hash * 31 + (this.Resource == null ? 0 : this.Resource.GetHashCode());
            return hash;
        }
    }
}
