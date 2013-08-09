using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.IO;
using System.IO;

namespace IntAirAct
{
    public static class NancyRequestExtensions
    {
        public static string BodyAsString(this Request request)
        {
            RequestStream bodyStream = request.Body;
            bodyStream.Position = 0;
            string bodyText;
            using (var bodyReader = new StreamReader(bodyStream))
            {
                bodyText = bodyReader.ReadToEnd();
            }
            return bodyText;
        }

        public static byte[] BodyAsByte(this Request request)
        {
            return ReadFully(request.Body);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
