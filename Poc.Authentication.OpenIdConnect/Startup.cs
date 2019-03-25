using System;
using System.IO;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Poc.Authentication.OpenIdConnect.Users;

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
            UserStore userStore = new UserStore();
            userStore.Add(new User(Guid.Parse("F2845A10-5C9A-41EB-A91F-863BC0D2E716"), "dani"), "iszap");

            services.AddDataProtection()
                .SetApplicationName("poc-ropc-oauth")
                .PersistKeysToFileSystem(new DirectoryInfo("app-keys"));

            services.AddAuthentication(options => options.DefaultScheme = OpenIdConnectConstants.Schemes.Bearer)
                .AddOpenIdConnectServer(options =>
                {
                    // Create your own authorization provider by subclassing
                    // the OpenIdConnectServerProvider base class.
                    options.Provider = new AuthorizationProvider(userStore);
                    // Enable token endpoints.                    
                    options.TokenEndpointPath = "/auth/token";
                    // During development, you can set AllowInsecureHttp
                    // to true to disable the HTTPS requirement.
                    options.AllowInsecureHttp = true;
                    // issue an access token that expires in 24 hours
                    options.AccessTokenLifetime = TimeSpan.FromHours(24);
                    // issue a refresh token that expires in 14 days
                    options.RefreshTokenLifetime = TimeSpan.FromDays(14);
                })
                .AddOAuthValidation();

            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()))
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
