using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Web.Nancy.Models.Security;

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

        public static UserModel BindUser(this NancyModule nancy)
        {
            if (nancy.Context.CurrentUser == null) return new UserModel(nancy);

            var usuario = new UserModel(nancy)
            {
                Id = Convert.ToInt32(nancy.Context.CurrentUser.FindFirst("idUsuario").Value),
                User = nancy.Context.CurrentUser.FindFirst("usuario").Value,
                Name = nancy.Context.CurrentUser.FindFirst("nombre").Value,                
            };

            return usuario;
        }
    }
}
