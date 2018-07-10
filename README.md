# readme

In this readme we explain how you can setup an external login using Onegini oAuth. There are three important things you
need to know.

1. The basic idea of oAuth [read here](https://www.digitalocean.com/community/tutorials/an-introduction-to-oauth-2) to get yourself started;
2. The Identity framework basics:
    * See this for ASP.NET;
    * See [this](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-2.1) for ASP.NET Core;
3. How to implement OneginiAuthentication, which is covered in this readme.

### 1. ASP.NET

[todo]

### 2. ASP.NET Core

In ASP.NET Core you can use `Identity` (read more [here](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.1&tabs=visual-studio%2Caspnetcore2x)). 
In `Identity` there is an option to add _external login_. The `Identity` framework provides a way to add multiple 
_external logins_. In this example we just add one external login and we simply hide all the other features `Identity` 
has to offer. This way we can use the default and by Microsoft described methods often used in ASP.NET Core 
applications. 

#### 2.1 Implement OneginiAuthentication 
##### 2.1.1 Why do we need this

We are required to implement our own `OAuthHandler` becouse the standard handler made by Microsoft does not work with
the Onegini token server. This is due to a conflict in the implementation of the [RFC 6749](https://tools.ietf.org/html/rfc6749#section-2.3.2)
. To fix this issue we implemented our own `OAuthHandler`, you can find it in _"/OneginiAuthClient/AspNet.Security.OAuth.Onegini"_.

##### 2.1.2 How it works

The four files prefixed `OneginiAuthentication` are automatically scanned by the .net framework. Inside the `OneginiAuthenticationExtensions.cs`
file we extend the `AuthenticationBuilder`. This basically adds our own build step using our own `OAuthHandler`. The 
`OneginiAuthenticationHandler` extends the `OAuthHandler`. It overwrites the default `ExchangeCodeAsync` method and 
handles the token request properly.

##### 2.1.3 What you should do

Create a new _library project_ or _folder inside your current project_. Then copy the four files from _"/OneginiAuthClient/AspNet.Security.OAuth.Onegini"_
. 

1. OneginiAuthenticationDefaults.cs
2. OneginiAuthenticationExtentions.cs
3. OneginiAuthenticationHandler.cs
4. OneginiAuthenticationOptions.cs

######Change the `CreateTicketAsync` method
Inside `OneginiAuthenticationHandler.cs` you will find a method called `CreateTicketAsync`. This method retrieves the 
user info from the *Resource gateway*. When you implement our *DotnetAspCoreResourceGatewayExample* you're already setup.
When you are using a different setup you need customize this method so it will fit your solution. 

When using a rest service which returns json you can simply map the json to the corresponding Identity fields. You can
change the json mapping inside the `OneginiAuthenticationOptions.cs` file. You can read [this](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.oauth.claims.claimactioncollection?view=aspnetcore-2.1) 
to learn more about how the mapping works.

When the resource gateway uses a different format than json, you need to bind at least two claims. You can do this by 
using the following code inside the `CreateTicketAsync` overwrite:

    //context.RunClaimActions(payload);
    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "[NameIdentifier]"));
    identity.AddClaim(new Claim(ClaimTypes.Name, "[Name]"));

These two claims must be set for the *external login* to function.

######Configure the return url
Change the `CallbackPath` inside the `OneginiAuthenticationDefaults.cs` file for security reasons.

######Setup the middleware
In default your services for the dependency injection are setup inside `Startup.cs` inside the `ConfigureServices` method.
To use the `OneginiAuthenticationHandler` we need to configure this inside the `ConfigureServices` method.

Example -> see *"/DotnetAspCoreMvcExample/Startup.cs"* or try to use this example:

    services.AddAuthentication().AddOnegini(o =>
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
    });
    
Make sure that inside the `Configure` method you enable the following:

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();
    app.UseAuthentication();

######Customize the deafult *Identity* pages or create your own
Depending on your requirements you can customize the deafult generated *Views* and *Controllers*. You can also create
your own from scratch. Take a look inside the *DotnetAspCoreMvcExample*. You can find the examples
here *"/DotnetAspCoreMvcExample/Areas/Identity/Pages"*. Note that the `cshtml.cs` files represent the controller logic.

##### 2.1.4 Considerations

You could add some of the default configuration like endpoints to the `OneginiAuthenticationDefaults.cs` file.

#### 2.2 Configure the OneginiAuthentication

We use the default setup to configure our example. You can find the configuration inside `appsettings.json`. It is
located at *"/DotnetAspCoreMvcExample/appsettings.json"*.

