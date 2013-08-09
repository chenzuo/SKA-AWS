using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace IntAirAct
{

    public class RequestState
    {
        public HttpWebRequest WebRequest { get; set; }
        public IARequest IARequest { get; set; }
        public Action<IAResponse, Exception> Action { get; set; }
    }

    public class HttpWebRequestClient : IAClient
    {
        public void SendRequest(IARequest request, IADevice device)
        {
            this.SendRequest(request, device, null);
        }

        public void SendRequest(IARequest request, IADevice device, Action<IAResponse, Exception> action)
        {
            HttpWebRequest webRequest = CreateRequest(device, request);
            RequestState state = new RequestState
                {
                    WebRequest = webRequest,
                    Action = action,
                    IARequest = request
                };

            if (request.Route.Action == "GET" || request.Route.Action == "HEAD")
            {
                webRequest.BeginGetResponse(GetResponse, state);
            }
            else
            {
                webRequest.BeginGetRequestStream(GetResponseStream, state);
            }
        }

        private HttpWebRequest CreateRequest(IADevice device, IARequest iaRequest)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>(iaRequest.Parameters);
            string path = ReplaceParameters(iaRequest.Route.Resource, parameters);

            if (parameters.Count > 0)
            {
                path += "?";
            }

            foreach (KeyValuePair<string, string> parameter in parameters)
           { 
                string key = Nancy.Helpers.HttpUtility.UrlEncode(parameter.Key);
                string value = Nancy.Helpers.HttpUtility.UrlEncode(parameter.Value);
                path += String.Format("{0}={1}&", key, value);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://{0}:{1}{2}", device.Host, device.Port, path));
            request.ContentType = iaRequest.ContentType;
            request.Method = iaRequest.Route.Action;

            iaRequest.Origin.Foreach(x =>
            {
                request.Headers.Add(String.Format("X-IA-Origin: {0}", x.Name));
            });
            foreach (KeyValuePair<string, string> metaentry in iaRequest.Metadata)
            {
                request.Headers.Add(String.Format("{0}: {1}", metaentry.Key, metaentry.Value));
            }
            
            return request;
        }

        private static IAResponse IAResponseFromHttpWebResponse(HttpWebResponse webResponse)
        {
            IAResponse response = new IAResponse();

            response.StatusCode = (int)webResponse.StatusCode;
            response.ContentType = webResponse.ContentType;
            
            for(int i = 0; i < webResponse.Headers.Count; i++)
            {
                response.Metadata.Add(webResponse.Headers.Keys[i], webResponse.Headers[i]);   
            }

            Stream stream = webResponse.GetResponseStream();
            byte[] b;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                b = ms.ToArray();
            }
            response.Body = b;

            return response;
        }

        private static void GetResponse(IAsyncResult asyncResult)
        {
            IAResponse res = null;
            Exception e = null;
            RequestState state = null;
            try
            {
                state = (RequestState)asyncResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)state.WebRequest.EndGetResponse(asyncResult);
                res = IAResponseFromHttpWebResponse(response);
            }
            catch (Exception exception)
            {
                e = new Exception(exception.Message);
            }

            if (state != null && state.Action != null)
            {
                state.Action(res, e);
            }
        }

        private static void GetResponseStream(IAsyncResult asyncResult)
        {
            RequestState state = null;
            try
            {
                state = (RequestState)asyncResult.AsyncState;
                Stream postStream = state.WebRequest.EndGetRequestStream(asyncResult);
                postStream.Write(state.IARequest.Body, 0, state.IARequest.Body.Length);
                postStream.Close();

                state.WebRequest.BeginGetResponse(GetResponse, state);
            }
            catch (Exception e)
            {
                if (state != null && state.Action != null)
                {
                    state.Action(null, new Exception(e.Message));
                }
            }
        }

        private string ReplaceParameters(string path, Dictionary<string, string> parameters)
        {
            string result = Regex.Replace(path, @"{(\w+)}", delegate(Match match)
            {
                string value = match.ToString();
                string key = value.Substring(1, value.Length - 2);
                if (parameters.ContainsKey(key))
                {
                    string keyValue = parameters[key];
                    parameters.Remove(key);
                    return keyValue;
                }
                else
                {
                    return value;
                }
            });
            return result;
        }
    }
}
