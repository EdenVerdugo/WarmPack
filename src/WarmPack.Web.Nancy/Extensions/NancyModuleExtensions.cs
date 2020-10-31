using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Nancy.Extensions
{
    public static class NancyModuleExtensions
    {
        public static dynamic BindModel(this NancyModule context)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(context.Request.Body))
            {
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize(jsonTextReader);
                }
            }
        }
    }
}
