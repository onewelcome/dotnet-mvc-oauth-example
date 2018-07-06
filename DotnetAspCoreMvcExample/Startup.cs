using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetAspCoreMvcExample.Data;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace DotnetAspCoreMvcExample
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddOnegini(o =>
            {
                o.ClientId = "8EC5395C7AD02E64AAAA88BAF2C5B34423A3D144F2008BA1F0C09859BBE4DAD9";
                o.ClientSecret = "065E8CB4F3573145189EA02739E642C5AE1BADEE3150078CAEB90B4F6DFC321D";
                o.AuthorizationEndpoint = "https://onegini-msp-snapshot.test.onegini.io/oauth/authorize";
                o.TokenEndpoint = "https://onegini-msp-snapshot.test.onegini.io/oauth/token";
                o.UserInformationEndpoint = "https://onegini-msp-snapshot.test.onegini.io/client/resource/profile";
            });

            /*
            services.AddAuthentication().AddOAuth("Onegini", c =>
            {
                c.ClientId = "8EC5395C7AD02E64AAAA88BAF2C5B34423A3D144F2008BA1F0C09859BBE4DAD9";
                c.ClientSecret = "065E8CB4F3573145189EA02739E642C5AE1BADEE3150078CAEB90B4F6DFC321D";
                c.Scope.Add("read");
                c.Scope.Add("profile");
                c.CallbackPath = "/signin-onegini";
                c.AuthorizationEndpoint = "https://onegini-msp-snapshot.test.onegini.io/oauth/authorize";
                c.TokenEndpoint = "https://onegini-msp-snapshot.test.onegini.io/oauth/token";
                c.UserInformationEndpoint = "https://onegini-msp-snapshot.test.onegini.io/client/resource/profile";
            });*/

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
