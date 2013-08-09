using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntAirAct
{
    public class IADeSerialization
    {
        public byte[] Body { get; set; }
        public string ContentType { get; set; }

        public IADeSerialization()
        {
            this.Body = new byte[0];
            this.ContentType = "text/plain";
        }

        public IADeSerialization(byte[] body, string contentType)
        {
            this.Body = body;
            this.ContentType = contentType;
        }

        public dynamic BodyAs<T>()
        {
            Type type = typeof(T);
            if (type.IsAssignableFrom(typeof(string)))
            {
                return BodyAsString();
            }
            else if (type.IsAssignableFrom(typeof(int)))
            {
                return int.Parse(BodyAsString());
            }
            else
            {
                try
                {
                    object data = JsonConvert.DeserializeObject(BodyAsString());
                    return Deserialize<T>(data);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
        }

        private dynamic Deserialize<T>(object data)
        {
            if (data == null)
            {
                return null;
            }
            if (data.GetType().IsAssignableFrom(typeof(JValue)))
            {
                data = ((JValue)data).Value;
            }
            Type type = typeof(T);
            if (data.GetType().IsAssignableFrom(typeof(string)))
            {
                string value = (string)data;
                if (type.IsAssignableFrom(typeof(string)))
                {
                    return value;
                }
                else if (type.IsAssignableFrom(typeof(int)))
                {
                    return int.Parse(value);
                }
                else
                {
                    return null;
                }
            }
            else if (data.GetType().IsAssignableFrom(typeof(int)))
            {
                int value = (int)data;
                if (type.IsAssignableFrom(typeof(int)))
                {
                    return value;
                }
                else if (type.IsAssignableFrom(typeof(string)))
                {
                    return "" + value;
                }
                else
                {
                    return null;
                }
            }
            else if (data.GetType().IsAssignableFrom(typeof(long)))
            {
                long value = (long)data;
                if (type.IsAssignableFrom(typeof(int)))
                {
                    try
                    {
                        int result = Convert.ToInt32(value);
                        return result;
                    }
                    catch (OverflowException)
                    {
                        return value;
                    }
                }
                else if (type.IsAssignableFrom(typeof(string)))
                {
                    return "" + data;
                }
                else
                {
                    return null;
                }
            }
            else if (data.GetType().IsAssignableFrom(typeof(JArray)))
            {
                if (type.IsAssignableFrom(typeof(string[])))
                {
                    List<string> result = new List<string>();
                    foreach (object item in (JArray)data)
                    {
                        result.Add(Deserialize<string>(item));
                    }
                    return result.ToArray();
                }
                else if (type.IsAssignableFrom(typeof(int[])))
                {
                    List<int> result = new List<int>();
                    foreach (object item in (JArray)data)
                    {
                        int value = Deserialize<int>(item);
                        result.Add(value);
                    }
                    return result.ToArray();
                }
                else
                {
                    List<T> result = new List<T>();
                    foreach (object item in (JArray)data)
                    {
                        T val = Deserialize<T>(item);
                        result.Add(val);
                    }
                    return result;
                }
            }
            else if (data.GetType().IsAssignableFrom(typeof(JObject)))
            {
                JObject jObject = (JObject)data;
                return jObject.ToObject<T>();
            }
            return null;
        }

        public string BodyAsString()
        {
            return System.Text.Encoding.UTF8.GetString(this.Body);
        }

        public void SetBodyWith(Object data)
        {
            if (data == null)
            {
                return;
            }
            else if (data.GetType() == typeof(string))
            {
                SetBodyWithString((string)data);
            }
            else
            {
                String json = JsonConvert.SerializeObject(data);

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                this.Body = encoding.GetBytes(json);

                this.ContentType = "application/json";
            }
        }

        public void SetBodyWithString(string body)
        {
            this.Body = System.Text.Encoding.UTF8.GetBytes(body);
        }
    }
}
