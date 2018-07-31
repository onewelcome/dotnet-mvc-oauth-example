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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetAspCoreMvcExample.Models;
using DotnetAspCoreMvcExample.Services.ApiClient;
using ExampleModel.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;

namespace DotnetAspCoreMvcExample.Controllers
{
    public class HomeController : Controller
    {
        private static IApiClient _client;

        public HomeController(IApiClient client)
        {
            _client = client;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Private()
        {
            //Example: get access token to use in api call
            //var accessToken = await HttpContext.GetTokenAsync("access_token");

            //Example: quick and dirty api call using a service that handles the access token using the HttpClient middleware
            var response = await _client.Client.GetAsync("https://localhost:5003/api/user/claims");
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var rederectUrl = "/account/login?returnUrl="+Request.Path;
                return Redirect(rederectUrl);
            }
            
            //When 403 user probably does not have authorization to use endpoint
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                //Handle situation where user is not authorized
                return null;
            }
            
            var text = await response.Content.ReadAsStringAsync();
            
            Result result = JObject.Parse(text).ToObject<Result>();
            var claimListJson = (JArray) result.content;
            var claimList = claimListJson.ToObject<List<string>>();
            
            ViewData["gatewayClaims"] = claimList;
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
