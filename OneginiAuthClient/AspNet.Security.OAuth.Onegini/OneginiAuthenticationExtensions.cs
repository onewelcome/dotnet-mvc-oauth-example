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