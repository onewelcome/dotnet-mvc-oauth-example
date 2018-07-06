using System;
using AspNet.Security.OAuth.Onegini;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    
    /// <summary>
    /// Extension methods to add Onegini authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class OneginiAuthenticationExtensions
    {
        
            /// <summary>
            /// Adds <see cref="OneginiAuthenticationHandler"/> to the specified
            /// <see cref="AuthenticationBuilder"/>, which enables Onegini authentication capabilities.
            /// </summary>
            /// <param name="builder">The authentication builder.</param>
            /// <returns>A reference to this instance after the operation has completed.</returns>
            public static AuthenticationBuilder AddOnegini(this AuthenticationBuilder builder)
            {
                return builder.AddOnegini(OneginiAuthenticationDefaults.AuthenticationScheme, options => { });
            }
    
            /// <summary>
            /// Adds <see cref="OneginiAuthenticationHandler"/> to the specified
            /// <see cref="AuthenticationBuilder"/>, which enables Onegini authentication capabilities.
            /// </summary>
            /// <param name="builder">The authentication builder.</param>
            /// <param name="configuration">The delegate used to configure the OAuth 2.0 options.</param>
            /// <returns>A reference to this instance after the operation has completed.</returns>
            public static AuthenticationBuilder AddOnegini(
                this AuthenticationBuilder builder,
                Action<OneginiAuthenticationOptions> configuration)
            {
                return builder.AddOnegini(OneginiAuthenticationDefaults.AuthenticationScheme, configuration);
            }
    
            /// <summary>
            /// Adds <see cref="OneginiAuthenticationHandler"/> to the specified
            /// <see cref="AuthenticationBuilder"/>, which enables Onegini authentication capabilities.
            /// </summary>
            /// <param name="builder">The authentication builder.</param>
            /// <param name="scheme">The authentication scheme associated with this instance.</param>
            /// <param name="configuration">The delegate used to configure the Onegini options.</param>
            /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
            public static AuthenticationBuilder AddOnegini(
                this AuthenticationBuilder builder, string scheme,
                Action<OneginiAuthenticationOptions> configuration)
            {
                return builder.AddOnegini(scheme, OneginiAuthenticationDefaults.DisplayName, configuration);
            }
    
            /// <summary>
            /// Adds <see cref="OneginiAuthenticationHandler"/> to the specified
            /// <see cref="AuthenticationBuilder"/>, which enables Onegini authentication capabilities.
            /// </summary>
            /// <param name="builder">The authentication builder.</param>
            /// <param name="scheme">The authentication scheme associated with this instance.</param>
            /// <param name="caption">The optional display name associated with this instance.</param>
            /// <param name="configuration">The delegate used to configure the Onegini options.</param>
            /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
            public static AuthenticationBuilder AddOnegini(
                this AuthenticationBuilder builder,
                string scheme, string caption,
                Action<OneginiAuthenticationOptions> configuration)
            {
                return builder.AddOAuth<OneginiAuthenticationOptions, OneginiAuthenticationHandler>(scheme, caption, configuration);
            }
    }
}