/*
 * Copyright (c) 2017 Onegini B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
            await HttpContext.ChallengeAsync(OneginiAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Remove session data from cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            //*Optional -> Revoke access token
            await HttpContext.SignOutAsync(OneginiAuthenticationDefaults.AuthenticationScheme);
            
            return Redirect("/");
            
            //Use this when you want to logout trough the identity provider
            //The Identity provider automatically revokes the access token, when you use this make sure you also comment out this line 29
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