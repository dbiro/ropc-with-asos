using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Poc.Authentication.OpenIdConnect
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = OpenIdConnectConstants.Schemes.Bearer;
                })
                .AddOpenIdConnectServer(options =>
                {
                    // Create your own authorization provider by subclassing
                    // the OpenIdConnectServerProvider base class.
                    options.Provider = new AuthorizationProvider();
                    // Enable token endpoints.                    
                    options.TokenEndpointPath = "/auth/token";
                    // During development, you can set AllowInsecureHttp
                    // to true to disable the HTTPS requirement.
                    options.AllowInsecureHttp = true;
                    // issue a token that never expires
                    options.AccessTokenLifetime = null;                                        
                })
                .AddOAuthValidation();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new AuthorizeFilter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();                        

            app.UseMvc();
        }
    }
}
