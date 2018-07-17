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
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Onegini", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Remove session data from cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //todo: revoke access token
            await HttpContext.SignOutAsync(OneginiAuthenticationDefaults.AuthenticationScheme);
            
            //todo: redirect
            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            ViewData["accessToken"] = accessToken;
            
            return View("Claims");
        }
    }
}