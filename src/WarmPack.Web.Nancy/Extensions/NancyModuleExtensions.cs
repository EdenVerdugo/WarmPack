﻿using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Web.Nancy.Models.Security;

namespace WarmPack.Web.Nancy
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

        public static string Json(this NancyModule context)
        {
            var reader = new StreamReader(context.Request.Body);
            string json = reader.ReadToEnd();

            return json;
        }

        public static T BindWithJsonConvert<T>(this NancyModule context)
        {
            string json = Json(context);

            var r = JsonConvert.DeserializeObject<T>(json);

            return r;
        }

        public static UserModel BindUser(this NancyModule nancy)
        {
            if (nancy.Context.CurrentUser == null) return new UserModel(nancy);

            var usuario = new UserModel(nancy)
            {
                Id = nancy.Context.CurrentUser.FindFirst("id").Value,
                User = nancy.Context.CurrentUser.FindFirst("user").Value,
                Name = nancy.Context.CurrentUser.FindFirst("name").Value,                
            };

            return usuario;
        }

        public static byte[] GetFileBytes(this Request request)
        {
            return GetFileBytes(request, "");
        }

        public static byte[] GetFileBytes(this Request request, string key = "")
        {
            var file = key == "" ? request.Files.FirstOrDefault() : request.Files.FirstOrDefault(f => f.Key == key);

            byte[] buffer = null;

            using(var ms  = new MemoryStream())
            {
                file.Value.CopyTo(ms);
                ms.Position = 0;
                buffer = ms.ToArray();
            }

            return buffer;
        }
    }
}
