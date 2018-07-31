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

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace DotnetAspCoreMvcExample.Services.ApiClient
{
    public class ResourceGatewayMessageHandler : HttpClientHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        public ResourceGatewayMessageHandler(IHttpContextAccessor context)
        {
            _contextAccessor = context;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Retrieve acces token from token store
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            
            //Add token to request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            //Execute request
            var response = await base.SendAsync(request, cancellationToken);
            
            //When 401 user is probably not logged in any more -> redirect to login screen
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Handle situation where user is not authenticated
                var context = _contextAccessor.HttpContext;
                var rederectUrl = "/account/login?returnUrl="+context.Request.GetDisplayUrl();
                context.Response.Redirect(rederectUrl); //not working
            }
            
            //When 403 user probably does not have authorization to use endpoint
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                //Handle situation where user is not authorized
            }
                
            return response;
        }
        
    }
}