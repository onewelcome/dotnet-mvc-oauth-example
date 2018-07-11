using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace OneginiAuthClient.OneginiAuthorization
{
    public class OneginiTokenService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Uri _tokenServerEndpoint;
        private readonly HttpClient Backchannel;
        
        public OneginiTokenService(string clientId, string clientSecret, string tokenServerEndpoint)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenServerEndpoint = new Uri(tokenServerEndpoint);
            Backchannel = new HttpClient();
        }

        public async Task<JObject> TokenIntrospect(string accessToken)
        {
            //Get data from /oauth/api/v1/token/introspect directly
            var basicAuthHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "token", accessToken },
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            
            Uri endpoint = new Uri(_tokenServerEndpoint, "token/introspect");
            
            var request = new HttpRequestMessage(HttpMethod.Post, _tokenServerEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", basicAuthHeader);
            request.Content = requestContent;
            
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            
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