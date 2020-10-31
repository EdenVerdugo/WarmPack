using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Nancy
{
    public abstract class NancySecureModule : NancyModule
    {
        public NancySecureModule()
        {
            this.RequiresAuthentication();
        }

        public NancySecureModule(string modulePath): base(modulePath)
        {
            this.RequiresAuthentication();
        }
    }
}
