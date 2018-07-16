using System;
using Microsoft.AspNetCore.Authentication;

namespace OneginiAuthClient.Onegini.AspNetCore.Authentication.OneginiBearer
{
    /// <summary>
    /// Extension method to add Onegini Bearer authentication capabilities to the ASP.NET authentication pipeline.
    /// </summary>
    public static class OneginiBearerExtensions
    {
        public static AuthenticationBuilder AddOneginiBearer(this AuthenticationBuilder builder, Action<OneginiBearerOptions> configureOptions)
        {
            return builder.AddScheme<OneginiBearerOptions, OneginiBearerHandler>(OneginiBearerOptions.DefaultScheme, configureOptions);
        }
    }
}