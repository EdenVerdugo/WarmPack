using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Classes;

namespace WarmPack.Web.Nancy.Models.Security
{
    public class UserModel
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Name { get; set; }

        private ClaimsPrincipal _claims;

        public UserModel(NancyModule nancy)
        {
            _claims = nancy.Context.CurrentUser;
        }

        public UserModel(string id, string user, string name)
        {
            Id = id;
            User = user;
            Name = name;            
        }

        public Castable Claim(string name)
        {
            return new Castable(_claims.FindFirst(name).Value);
        }


    }
}
