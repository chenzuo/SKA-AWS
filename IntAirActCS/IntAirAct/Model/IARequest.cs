using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monads;

namespace IntAirAct
{
    public class IARequest : IADeSerialization
    {
        public IARoute Route { get; set; }
        public Dictionary<String, String> Metadata { get; set; }
        public Dictionary<String, String> Parameters { get; set; }
        public Option<IADevice> Origin { get; set; }

        public IARequest(IARoute route) : base()
        {
            this.Route = route;
            this.Metadata = new Dictionary<string,string>();
            this.Parameters = new Dictionary<string,string>();
            this.Origin = new None<IADevice>();
        }

        public IARequest(IARoute route, Dictionary<String, String> metadata, Dictionary<String, String> parameters, IADevice origin, byte[] body, string contentType)
            : base(body, contentType)
        {
            this.Route = route;
            this.Metadata = metadata;
            this.Parameters = parameters;
            this.Origin = origin.AsOption();
        }

        public override string ToString()
        {
            return String.Format("IARequest[Route: {0}, Metadata: {1}, Parameters: {2}, Body: {3}]", Route, Metadata, Parameters, Body);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IARequest) == null)
            {
                return false;
            }

            IARequest request = (IARequest)obj;
            return (this.Route != null && this.Route.Equals(request.Route))
                && (this.Metadata != null && this.Metadata.Equals(request.Metadata))
                && (this.Parameters != null && this.Parameters.Equals(request.Parameters))
                && (this.Origin.Equals(request.Origin))
                && (this.Body == request.Body);
        }

        public override int GetHashCode()
        {
            int hash = 89;
            hash = hash * 31 + (this.Route == null ? 0 : this.Route.GetHashCode());
            hash = hash * 31 + (this.Metadata == null ? 0 : this.Metadata.GetHashCode());
            hash = hash * 31 + (this.Parameters == null ? 0 : this.Parameters.GetHashCode());
            hash = hash * 31 + (this.Origin.Select(x => x.GetHashCode()).GetOrElse(()=>0));
            hash = hash * 31 + (this.Body == null ? 0 : this.Body.GetHashCode());
            return hash;
        }
    }
}
