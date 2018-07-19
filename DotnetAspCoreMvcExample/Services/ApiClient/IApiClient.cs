using System.Net.Http;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAspCoreMvcExample.Services.ApiClient
{
    public interface IApiClient
    {
        HttpClient Client { get; }
    }
}