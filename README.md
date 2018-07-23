# readme

In this readme we explain how you can setup an external login using Onegini OAuth. There are a few important things you
need to know before we can start.

1. The basic idea of OAuth [read here](https://www.digitalocean.com/community/tutorials/an-introduction-to-OAuth-2) to get yourself started;
2. The basic idea of ASP.NET Core fundamentals:
    * [Authentication](https://ignas.me/tech/custom-authentication-asp-net-core-20/);
    * [Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/?view=aspnetcore-2.1);
    * [Dependency Injection](https://joonasw.net/view/aspnet-core-di-deep-dive);
    * [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1&tabs=aspnetcore2x).
    
## 1. ASP.NET
      
Currently we only provide an example on ASP.NET Core.

## 2. ASP.NET Core

In ASP.NET Core you can use the frameworks default authentication and authorization flow. The authentication and 
authorization flow use the claim principle also covered in this example. 

We will not use the ASP.NET Core `Identity` system, this is possible becouse the `Identity` and authentication,
authorization and claims are loosely coupled. `Identity` is used to setup you own user store, since we already provide
an Identity provider we'll drop `Identity`.

**Example setup**

The example solution exists out of four projects.

1. DotnetAspCoreMvcExample -> The front-end webapplication that will login the user and create a session and query the resource gateway;
2. DotnetAspCoreResourceGatewayExample -> The resource gateway that will validate the access token and provides data to the front-end webapplication;
3. ExampleModel -> Contains a shared model between DotnetAspCoreMvcExample and DotnetAspCoreResourceGatewayExample;
4. OneginiAuthClient -> A class library that extends the default middleware to implement Onegini authentication.

First we'll cover the *Resource gateway*, then we'll cover the *front-end webapplication*.

### 2.1 Resource gateway

The resource gateway provides the users data and will authenticate using a bearer access token. In our example the access
 token is validated on each request. Also we cover an example on how to implement autentication using the claims principle
 in ASP.NET Core.

#### 2.1.1 Setup middleware

When you create an new ASP.NET Core project choose the API template without authentication. In the `Startup.cs` file we
can configurate our middleware. The default middleware offered by the ASP.NET framework does not cover what we need so
we have to implement a custom autentication middleware. 

In the exmple we extend the `AuthenticationHandler` middleware to validate the access token and login the user using the 
frameworks default authentication flow. Take a look inside `~/OneginiAuthClient/Onegini.AspNetCore.Authentication.OneginiBearer`
. You'll find three classes:

1. `OneginiBearerExtensions` -> Used to extend the authentication builder so we can easily add our custom middleware;
2. `OneginiBearerHandler` -> The handler that will be called on each request to validate the access token;
3. `OneginiBearerOptions` -> The options we'll use to configure the handler inside `Startup.cs`.

Implement these three classes either directly or through a library into your project. Note that this is just an example,
inspect the code and change it if for your situation. 

#### 2.1.2 Configure middleware

When the middleware implemetation is in place we can configure ASP.NET Core to use it. Before we can test this you'll
need to register an API client at the Onegini admin panel. You can find this under *Configuration -> System -> API clients*.
The API client needs at least access to the [*Token introspection*](https://docs.onegini.com/msp/token-server/8.1.0/api-reference/token-introspection.html) endpoint.

You'll need these three values to continue:

1. Token introspection endpoint;
2. Client Id;
3. Client Secret.

**Authentication**

Inside the `ConfigureServices` method in `Startup.cs` you use the builder to setup the middleware:

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = OneginiBearerOptions.DefaultScheme;
                o.DefaultChallengeScheme = OneginiBearerOptions.DefaultScheme;
            })
            .AddOneginiBearer(o =>
            {
                o.TokenIntrospectionEndpoint = Configuration["OneginiAuth:TokenIntrospectionEndpoint"]; //Introspect endpoint, example -> https://domain.com/oauth/api/v1/token/introspect 
                o.ClientId = Configuration["OneginiAuth:ClientId"]; //The 'Client ID' of the Token introspection API
                o.ClientSecret = Configuration["OneginiAuth:ClientSecret"]; //The 'Client secret' of the Token introspection API
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

When you use the `[Authorize]` annotation the middleware will be triggered. When the access token is valid the controller
will behave as usual. When the access token is invalid a 401 will be returned.

When all controllers should be authorized it would be more convenient to configure this right away and omit the 
`[Authorize]` annotation:

        services.AddMvc(o =>
        {
            o.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

**Authorization**

When there is also the need for authorization based on whatever data returned by the token introspection endpoint we
can easily do this using policies. Inspect the `Startup.cs` and the `UserController.cs` to view an example on this.

In the example we register claims inside the `OneginiBearerHandler`. Note that it depends on configuration which 
fields are returned by the token introspection endpoint. Change `OneginiBearerHandler` accordingly to fit your environment
setup. 

#### 2.1.3 Test the resource gateway

If the Onegini test client is available you can test the resource gateway quite easily. Use the *Authorization code grant*
to request an access token. Copy the token and setup a request using a tool like Postman. Set an authorization header
manually and prefix the token with `Bearer ` (including the space). Do a request and see if it works.

### 2.2 Front-end webapplication

The front-end webapplication interfaces with the user. When the user needs to be authenticated there are multiple setups
possible. In the example we use the OAuth flow. When the users logs in he's redirected to an external page to login.
When the loggin is successfull the user is redirected back where he was before the login.

#### 2.2.1 Setup middleware

ASP.NET Core provides an OAuth autentication middleware by default. However we are required to implement our own 
`OAuthHandler` becouse the standard handler made by Microsoft does not work with the Onegini token server. This is 
due to a conflict in the implementation of the [RFC 6749](https://tools.ietf.org/html/rfc6749#section-2.3.2). To fix 
this issue we implemented our own `OAuthHandler` in the example. Take a look inside `~/OneginiAuthClient/AspNet.Security.OAuth.Onegini`.
We have followed the same pattern as the default implemetations of other OAuth providers. The custom middleware is 
almost identical to the middelware we use in the resource gateway. The only diffence is that we now extend the 
`OAuthHandler` and implement the `IAuthenticationSignOutHandler` to handle the signout event.

The `OneginiAuthenticationHandler` exists of three methods:

1. `ExchangeCodeAsync` -> Fixes issue with [RFC6749 2.3.1.](https://tools.ietf.org/html/rfc6749#section-2.3.1) (see comment in method for more information);
2. `CreateTicketAsync` -> Queries the Resource gateway to get user data and creates ticket to log in the user; 
3. `SignOutAsync` -> Revokes access token when signout event is called. When single-signout is used this is not necessary.

Implement the four classes either directly or through a library into your project. Note that this is just an example, 
inspect the code and change it for your situation.

#### 2.2.2 Configure middleware

When the middleware implemetation is in place we can configure ASP.NET Core to use it. Before we can use this you'll 
need to register a web client in the Onegini admin panel. You can find this under *Configuration -> Web clients*.

You'll need these values to continue:

1. Client Id;
2. Client Secret;
3. Scope -> The scopes you want to require;
4. Authorization Endpoint -> https://[Onegini token server]/oauth/authorize
5. Token Endpoint -> https://[Onegini token server]/oauth/token
6. User Information Endpoint -> The endpoint on our resource gateway
7. Revoke Token Endpoint -> Optional https://[Onegini token server]/oauth/revoke

**Authentication**

In the example we use `CookieAuthentication` to setup a session and `OneginiAuthentication` to handle the login validation.
Inside the `ConfigureServices` method in `Startup.cs` you use the builder to setup the middleware:

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
                o.RevokeTokenEndpoint = Configuration["OneginiAuth:RevokeTokenEndpoint"];
                o.ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "content", "userId"); //In order to successfully create a ticket we need to set at least the NameIdentifier claim 
            });
            
Set `UseAuthentication` inside the `Configure` method:

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ...

            app.UseAuthentication();

            ...
        }
        
#### 2.2.3 Setup the controller

Create a `AccountController` to handle the login and logout requests:

    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Onegini", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Remove session data from cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            //Revoke access token
            await HttpContext.SignOutAsync(OneginiAuthenticationDefaults.AuthenticationScheme);
            
            return Redirect("/");
            
            //Use this when you want to logout through the identity provider
            //The Identity provider automatically revokes the access token, when you use this comment out this line -> `HttpContext.SignOutAsync(OneginiAuthenticationDefaults.AuthenticationScheme)`
            //return Redirect("https://[ipdDomain]/personal/logout?origin=https://[returndomain]"); //Example url -> https://cim.domain.com/personal/logout?origin=https://localhost:5001
        }
    }
    
Use the `[Authorize]` annotation on a controller or action to redirect the user to the login page when they are not authenticated.
Example:

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            ViewData["accessToken"] = accessToken;
            
            return View("Claims");
        }

#### 2.2.3 Setup the view

You set your login or logout UI elements like this:

    @if (User.Identity.IsAuthenticated)
    {
        <a asp-controller="Account" asp-action="Profile">@User.Identity.Name</a>
        <a asp-controller="Account" asp-action="Logout">Logout</a>
    }
    else
    {
        <a asp-controller="Account" asp-action="Login">Login</a>
    }
    
#### 2.2.4 Use the resource gateway

When you want to query the resource gateway you need to authenticate using the access token. You can retrive the
token inside a controller like this:
    
    var accessToken = await HttpContext.GetTokenAsync("access_token");

When you setup an API call you can pass it like this:

    var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/api/some/endpoint");
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    var response = await _client.Client.SendAsync(request);
    
Note that the resource gateway can return a 401 or a 403. You'll need to handle this. The example does not cover how
to do this. It is advised to create a Repository and use it with the dependency injection. You can pass the access token
through the controller or retrieve it using `IHttpContextAccessor` available through dependency injection.