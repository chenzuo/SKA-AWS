using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Routing;
using System.IO;
using Nancy.Helpers;

namespace IntAirAct
{
    public class NancyRouteModule : NancyModule
    {
        public NancyRouteModule(NancyServerAdapter adapter)
        {
            foreach (KeyValuePair<IARoute, Action<IARequest, IAResponse>> kvp in adapter.Routes)
            {
                IARoute route = kvp.Key;
                Action<IARequest, IAResponse> action = kvp.Value;
                RouteBuilder rb = new RouteBuilder(route.Action, this);
                rb[route.Resource] = nancyDynamicDictionary =>
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();

                    // get parameters out of path
                    foreach (string key in nancyDynamicDictionary)
                    {
                        
                        DynamicDictionaryValue value = nancyDynamicDictionary[key];
                        string urldecoded = HttpUtility.UrlDecode(value.ToString());
                        parameters.Add(key, urldecoded);
                    }

                    // get parameters out of query string
                    foreach (string key in Request.Query)
                    {
                        DynamicDictionaryValue value = Request.Query[key];
                        parameters.Add(key, "" + value.Value);
                    }
                    string contentType = Request.Headers.ContentType;

                    IADevice origin = null;
                    if (Request.Headers.Keys.Contains("X-IA-Origin"))
                    {
                        IAIntAirAct intAirAct = TinyIoC.TinyIoCContainer.Current.Resolve<IAIntAirAct>();
                        if (intAirAct != null)
                        {
                            origin = intAirAct.DeviceWithName(Request.Headers["X-IA-Origin"].First());
                        }
                    }

                    Dictionary<string, string> metadata = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, IEnumerable<string>> header in Request.Headers)
                    {
                        var value = header.Value.First();
                        metadata[header.Key] = value;
                    }

                    IARequest iaRequest = new IARequest(route, metadata, parameters, origin, Request.BodyAsByte(), contentType);
                    IAResponse iaResponse = new IAResponse();
                    action(iaRequest, iaResponse);
                    Response response = new Response();
                    response.StatusCode = (HttpStatusCode)iaResponse.StatusCode;
                    response.Contents = stream =>
                    {
                        var writer = new BinaryWriter(stream);
                        writer.Write(iaResponse.Body);
                        writer.Flush();
                    };
                    response.Headers = iaResponse.Metadata;

                    response.ContentType = iaResponse.ContentType;

                    return response;
                };
            }
        }
    }
}
