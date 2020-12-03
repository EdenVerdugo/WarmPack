using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Rest
{
    public class ApiClientResult
    {
        public ApiClientResult()
        {

        }

        public ApiClientResult(HttpResponseMessage response, string data)
        {
            Response = response;

            try
            {
                Data = JToken.Parse(data);
            }
            catch(Exception)
            {
                Data = JObject.Parse("{ content : 'No se pudo generar el JObject => checar en el Response.Content.ReadAsStringAsync()'}");                
            }
            
        }
        public HttpResponseMessage Response { get; set; }
        public JToken Data { get; set; }
    }

    public class ApiClientResult<T>
    {
        public ApiClientResult()
        {

        }

        public ApiClientResult(HttpResponseMessage response, string data)
        {
            Response = response;

            if (response.IsSuccessStatusCode)
            {
                Data = JsonConvert.DeserializeObject<T>(data);
            }
        }
        public HttpResponseMessage Response { get; set; }
        public T Data { get; set; }
    }
}
