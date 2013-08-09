using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Routing;
using Nancy.Bootstrapper;
using Nancy;

namespace IntAirAct
{
    public class NancyRebuildableCache : RouteCache
    {

        private readonly IModuleKeyGenerator moduleKeyGenerator;
        //private readonly IRouteSegmentExtractor routeSegmentExtractor;
        //private readonly IRouteDescriptionProvider routeDescriptionProvider;
        private readonly INancyContextFactory contextFactory;
        private readonly INancyModuleCatalog moduleCatalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="NancyRebuildableCache"/> class.
        /// </summary>
        /// <param name="moduleCatalog">The <see cref="INancyModuleCatalog"/> that should be used by the cache.</param>
        /// <param name="moduleKeyGenerator">The <see cref="IModuleKeyGenerator"/> used to generate module keys.</param>
        /// <param name="contextFactory">The <see cref="INancyContextFactory"/> that should be used to create a context instance.</param>
        /// <param name="routeSegmentExtractor"> </param>
        public NancyRebuildableCache(INancyModuleCatalog moduleCatalog, IModuleKeyGenerator moduleKeyGenerator, INancyContextFactory contextFactory) : base(moduleCatalog, moduleKeyGenerator, contextFactory)
        {
            this.moduleKeyGenerator = moduleKeyGenerator;
            //this.routeSegmentExtractor = routeSegmentExtractor;
            //this.routeDescriptionProvider = routeDescriptionProvider;
            this.contextFactory = contextFactory;
            this.moduleCatalog = moduleCatalog;
        }

        /// <summary>
        /// Gets a boolean value that indicates of the cache is empty or not.
        /// </summary>
        /// <returns><see langword="true"/> if the cache is empty, otherwise <see langword="false"/>.</returns>
        public new bool IsEmpty()
        {
            return !this.Values.SelectMany(r => r).Any();
        }

        public void RebuildCache()
        {
            this.Clear();
            using (var context = contextFactory.Create())
            {
                this.BuildCache(moduleCatalog.GetAllModules(context));
            }
        }

        private void BuildCache(IEnumerable<NancyModule> modules)
        {
            foreach (var module in modules)
            {
                var moduleType = module.GetType();
                var moduleKey = this.moduleKeyGenerator.GetKeyForModuleType(moduleType);

                var routes =
                    module.Routes.Select(r => r.Description);

                //foreach (var routeDescription in routes)
                //{
                //    routeDescription.Description = this.routeDescriptionProvider.GetDescription(module, routeDescription.Path);
                //    routeDescription.Segments = this.routeSegmentExtractor.Extract(routeDescription.Path);
                //}

                this.AddRoutesToCache(module.Routes.Select(r => r.Description), moduleKey);
            }
        }

        private void AddRoutesToCache(IEnumerable<RouteDescription> routes, string moduleKey)
        {
            if (!this.ContainsKey(moduleKey))
            {
                this[moduleKey] = new List<Tuple<int, RouteDescription>>();
            }

            this[moduleKey].AddRange(routes.Select((r, i) => new Tuple<int, RouteDescription>(i, r)));
        }
    }
}
