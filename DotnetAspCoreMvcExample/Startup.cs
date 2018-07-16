using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOnegini(o =>
            {
                o.ClientId = Configuration["OneginiAuth:ClientId"];
                o.ClientSecret = Configuration["OneginiAuth:ClientSecret"];

                foreach (var scope in Configuration["OneginiAuth:Scope"].Split(" "))
                {
                    o.Scope.Add(scope);
                }
                
                o.AuthorizationEndpoint = Configuration["OneginiAuth:AuthorizationEndpoint"];
                o.TokenEndpoint = Configuration["OneginiAuth:TokenEndpoint"];
                o.UserInformationEndpoint = Configuration["OneginiAuth:UserInformationEndpoint"];
                o.ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "content", "userId");
                o.ClaimActions.MapJsonSubKey(ClaimTypes.Name, "content", "firstName");
                o.ClaimActions.MapJsonSubKey(ClaimTypes.Surname, "content", "lastName");
                o.ClaimActions.MapJsonSubKey(ClaimTypes.Email, "content", "email");
                o.ClaimActions.MapJsonSubKey(ClaimTypes.DateOfBirth, "content", "birthDate");
            });
            
            /*
            //Example: how to add general OAuth -> does not work for Onegini!
            //Read more here on how to use this: https://www.jerriepelser.com/blog/authenticate-oauth-aspnet-core-2/
            services.AddAuthentication().AddOAuth("[ProviderName]", c =>
            {
                c.ClientId = "";
                c.ClientSecret = "";
                c.Scope.Add("write");
                c.Scope.Add("read");
                c.CallbackPath = "/signin-sericex"; //Advised not to use provider name in callback -> when a zero day occurs we want to be sure that attackers can not simply find the endpoints of our customers use a custom callback url
                c.AuthorizationEndpoint = "[url]/oauth/authorize";
                c.TokenEndpoint = "[url]/oauth/token";
                c.UserInformationEndpoint = "[url]/client/resource/profile";
                c.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        context.RunClaimActions(user);
                    }
                };
            });
            */

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
