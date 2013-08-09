using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using TinyIoC;
using Nancy.Bootstrapper;
using Nancy.Routing;

namespace IntAirAct
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override TinyIoCContainer GetApplicationContainer()
        {
            return TinyIoCContainer.Current;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            this.RegisterTypes(container, new[] { new TypeRegistration(typeof(IRouteCache), typeof(NancyRebuildableCache)) });
            base.ApplicationStartup(container, pipelines);
        }
    }
}
