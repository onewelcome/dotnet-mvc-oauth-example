using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OneginiAuthClient.OneginiAuthorization.Middleware
{
    public class OneginiResourceGatewayAuthorization
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _conf;
        Result _result = new Result(){ succes = false};
        
        public OneginiResourceGatewayAuthorization(RequestDelegate next, IConfiguration conf)
        {
            _next = next;
            _conf = conf;
        }

        public async Task Invoke(HttpContext context)
        {
            
            string authorizationHeader = context.Request.Headers["Authorization"];
            
            if (authorizationHeader == null)
            {
                context.Response.StatusCode = 401; //Unauthorized
                _result.message = "Authorization header not set";
                SetBody(context);
                return;
            }
            
            if (!authorizationHeader.StartsWith("Bearer"))
            {
                context.Response.StatusCode = 401; //Unauthorized
                _result.message = "Incorrect authorization header";
                SetBody(context);                
                return;
            }
            
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
             
            
             
            if (!authorizationHeader.StartsWith("Bearer"))
            {
                context.Response.StatusCode = 401; //Unauthorized
                _result.message = "Authorization header not set";
                SetBody(context);                
                return;
            }
            
        }

        private void SetBody(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(this._result);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(result));

            context.Response.Body = stream;
        }
        
    }
}