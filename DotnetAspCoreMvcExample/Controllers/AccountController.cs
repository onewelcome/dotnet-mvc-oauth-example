using System;
using System.Security.Policy;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Onegini;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DotnetAspCoreMvcExample.Controllers
{
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
            
            //Use this when you want to logout trough the identity provider
            //The Identity provider automatically revokes the access token, when you use this comment out this line -> `HttpContext.SignOutAsync(OneginiAuthenticationDefaults.AuthenticationScheme)`
            //return Redirect("https://[ipdDomain]/personal/logout?origin=https://[returndomain]"); //Example url -> https://cim.domain.com/personal/logout?origin=https://localhost:5001
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            
            //Do not use in production, never expose the access token to the view
            ViewData["accessToken"] = accessToken;
            
            return View("Claims");
        }
    }
}