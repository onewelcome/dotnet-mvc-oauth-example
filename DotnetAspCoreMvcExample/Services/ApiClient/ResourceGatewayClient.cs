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