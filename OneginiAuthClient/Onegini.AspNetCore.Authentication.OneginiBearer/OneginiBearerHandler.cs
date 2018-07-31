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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace OneginiAuthClient.Onegini.AspNetCore.Authentication.OneginiBearer
{
    public class OneginiBearerHandler : AuthenticationHandler<OneginiBearerOptions>
    {
        public OneginiBearerHandler(IOptionsMonitor<OneginiBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }
        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            
            if (authorizationHeader == null)
            {
                return AuthenticateResult.Fail("Authorization header not set");
            }
            
            if (!authorizationHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Incorrect authorization header");
            }
            
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            //todo: Consideration -> cache valid tokens for faster authentication -> you could use the default token store from the ASP.NET framework
            
            //See docs about endpoint to learn what is returned by the endpoint -> https://docs.onegini.com/msp/token-server/8.1.0/api-reference/token-introspection.html
            var tokenResult = await TokenIntrospect(token);
            bool isTokenActive = tokenResult.Value<bool>("active");
            
            if (!isTokenActive)
            {          
                return AuthenticateResult.Fail("Token is not active");
            }
            
            //Create ticket, more info on tickets -> https://andrewlock.net/introduction-to-authentication-with-asp-net-core/
            var identity = new ClaimsIdentity(ClaimsIssuer);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, tokenResult.Value<string>("sub"))); // claim at least the sub value as the NameIdentifier, this value is used to query the user.

            //Add all the scopes as claims -> see `Startup` and `UserController` for examples on how to use this
            var scopes = tokenResult.Value<string>("scope");
            foreach (var scope in scopes.Split(" "))
            {
                identity.AddClaim(new Claim("scope", scope));
            }
            
            //Add AMR (Authentication Methods References) as claims -> you can use this for authorisation the same way as the scope example
            var authenticationMethodReferences = tokenResult["amr"].ToObject<List<string>>();
            foreach (var amr in authenticationMethodReferences)
            {
                identity.AddClaim(new Claim("amr", amr));
            }
            
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            
            return AuthenticateResult.Success(ticket);
        }
        
        public async Task<JObject> TokenIntrospect(string accessToken)
        {
            var basicAuthHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Options.ClientId}:{Options.ClientSecret}"));
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "token", accessToken },
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            
            Uri endpoint = new Uri(Options.TokenIntrospectionEndpoint);
            
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", basicAuthHeader);
            request.Content = requestContent;
            
            var response = await Options.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorPayload = response.Content.ReadAsStringAsync();
                var error = "Token server endpoint returns error: " + errorPayload;
                
                throw new HttpRequestException(error);
            }
            
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            
            return payload;
        }
    }
}