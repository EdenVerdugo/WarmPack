using Microsoft.IdentityModel.Tokens;
using Nancy;
using Nancy.Authentication.JwtBearer;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Nancy
{
    public class WarmpackNancyBootstrapper : DefaultNancyBootstrapper
    {
        public override void Configure(INancyEnvironment environment)
        {
            environment.Tracing(false, true);

            base.Configure(environment);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Jwt.JwtManager.SigningKey(),

                ValidateIssuer = true,
                ValidIssuer = Jwt.JwtManager.ValidIssuer,

                ValidateAudience = true,
                ValidAudiences = Jwt.JwtManager.ValidAudiences,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var configuration = new JwtBearerAuthenticationConfiguration
            {
                TokenValidationParameters = tokenValidationParameters                
            };

            pipelines.EnableJwtBearerAuthentication(configuration);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                //Access-Control-Allow-Origin
                ctx.Response
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-Type, Authorization");
            });

            base.RequestStartup(container, pipelines, context);
        }
    }
}
