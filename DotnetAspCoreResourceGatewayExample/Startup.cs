using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneginiAuthClient.Onegini.AspNetCore.Authentication.OneginiBearer;

namespace DotnetAspCoreResourceGatewayExample
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
            services.AddAuthentication(o =>
            {
                //We need to set OneginiBearer as the default AuthenticateScheme and ChallengeScheme to trigger the
                //authentication when we use the default `[Authorize]` annotation
                o.DefaultAuthenticateScheme = OneginiBearerOptions.DefaultScheme;
                o.DefaultChallengeScheme = OneginiBearerOptions.DefaultScheme;
            })
            .AddOneginiBearer(o =>
            {
                o.TokenIntrospectionEndpoint = Configuration["OneginiAuth:TokenIntrospectionEndpoint"]; //Introspect endpoint, example -> https://domain.com/oauth/api/v1/token/introspect 
                o.ClientId = Configuration["OneginiAuth:ClientId"]; //The 'Client ID' of the Token introspection API
                o.ClientSecret = Configuration["OneginiAuth:ClientSecret"]; //The 'Client secret' of the Token introspection API
            });
            
            //Create scope policies, more info on policy -> https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-2.1
            //This is just an example of how to add authorization based on scope and amr. Omit this when only
            //authentication is required.
            services.AddAuthorization(o => {
                o.AddPolicy("ReadProfile", p =>
                {
                    p.RequireClaim("scope", "profile");
                    p.RequireClaim("scope", "read");
                });
                o.AddPolicy("ReadProfileAdmin", p =>
                {
                    p.RequireClaim("scope", "admin"); //require admin role -> user gets 403 when his acces token does not provide the admin scope
                    p.RequireClaim("amr", "FINGER_PRINT"); //require to log in with finger print
                });
            });
            
            services.AddMvc(o =>
            {
                //Consideration: When all endpoints should be authorized it is more convenient to just enforce it right
                //away. Uncomment the line below to enforce `Authorize` filter on all endpoint, making the `[Authorize]`
                //annotation obsolete. 
                //o.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
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
            else
            {
                app.UseHsts();
            }
            
            app.UseAuthentication();
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}