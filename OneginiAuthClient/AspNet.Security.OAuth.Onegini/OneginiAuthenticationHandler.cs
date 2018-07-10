using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Onegini;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Onegini
{
    public class OneginiAuthenticationHandler : OAuthHandler<OneginiAuthenticationOptions>
    {
        public OneginiAuthenticationHandler(
            IOptionsMonitor<OneginiAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ): base(options, logger, encoder, clock){}

        //Overwrite default token exchange -> see comment below why
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var basicAuthHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Options.ClientId}:{Options.ClientSecret}"));
            
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "redirect_uri", redirectUri },
                { "code", code },
                { "grant_type", "authorization_code" },
                //{ "client_id", Options.ClientId },
                //{ "client_secret", Options.ClientSecret },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            //The default `ExchangeCodeAsync` method does not use HTTP Basic authentication which is required by the Onegini token server
            //The use of HTTP Basic authentication is enforced in RFC6749 2.3.1. -> https://tools.ietf.org/html/rfc6749#section-2.3.1
            requestMessage.Headers.Add("Authorization", basicAuthHeader);
            requestMessage.Content = requestContent;
            
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            if (response.IsSuccessStatusCode)
            {
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                var error = "Could not retrieve acces token: " + payload;
                return OAuthTokenResponse.Failed(new Exception(error));
            }
        }
        
        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {

            //Get data from /oauth/api/v1/token/introspect directly
            var basicAuthHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"TestValidationClientId:TestValidationClientSecret"));
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "token", tokens.AccessToken },
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            
            var request = new HttpRequestMessage(HttpMethod.Post, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", basicAuthHeader);
            request.Content = requestContent;
            
            /*
            //Get data from Resource gateway
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            */
            
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorPayload = JObject.Parse(await response.Content.ReadAsStringAsync());
                var error = "Could not retrieve user profile: " + errorPayload;
                
                throw new HttpRequestException(error);
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);
            
            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}