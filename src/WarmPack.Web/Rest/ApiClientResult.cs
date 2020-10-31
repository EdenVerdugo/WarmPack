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
                Data = JObject.Parse(data);
            }
            catch(Exception)
            {
                Data = JObject.Parse("{ content : 'No se pudo generar el JObject => checar en el Response.Content.ReadAsStringAsync()'}");                
            }
            
        }
        public HttpResponseMessage Response { get; set; }
        public JObject Data { get; set; }
    }
}
