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

using System.Net.Http;
using System.Threading.Tasks;
using ExampleModel.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Newtonsoft.Json.Linq;

namespace DotnetAspCoreMvcExample.Services.ApiClient
{
    //Simple example for resource gateway client service
    public class ResourceGatewayClient : IApiClient
    {
        private static HttpClient _client;
        public HttpClient Client => _client;

        public ResourceGatewayClient(IHttpContextAccessor contextAccessor)
        {
            if (_client == null)
            {
                _client = new HttpClient(new ResourceGatewayMessageHandler(contextAccessor));
                //configurate default base address
                //_client.BaseAddress = "https://gateway.domain.com/api";
            }
        }
        
        //Set methods for api endpoints
        /*
        public async Task<Result> getClaims()
        {
            var response = await _client.GetAsync("/user/claims");

            var result = JObject.Parse(await response.Content.ReadAsStringAsync()).ToObject<Result>();
            
            return result;
        }
        */
        
    }
}