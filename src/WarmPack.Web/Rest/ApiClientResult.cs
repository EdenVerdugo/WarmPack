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
            Data = JObject.Parse(data);
        }
        public HttpResponseMessage Response { get; set; }
        public JObject Data { get; set; }
    }
}
