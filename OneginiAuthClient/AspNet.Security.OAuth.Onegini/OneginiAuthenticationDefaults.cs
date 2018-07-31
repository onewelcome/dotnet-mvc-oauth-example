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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Onegini
{
    /// <summary>
    /// Default values used by the Onegini authentication middleware.
    /// </summary>
    public static class OneginiAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Onegini";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Onegini";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Onegini";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-external"; //Change value to whatever you whish, make sure not to use the exact value used in this example. When an exploit may be found an attacker could try to find all party's that use this endpoint. Therefore customize it! 

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// Currently there is no default value for this endpoint, the endpoint differiates per customer.
        /// When we provide one cloud based oAuth solution we probably would configure the default endpoints.
        /// </summary>
        public const string AuthorizationEndPoint = null;

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// Currently there is no default value for this endpoint, the endpoint differiates per customer.
        /// When we provide one cloud based oAuth solution we probably would configure the default endpoints.
        /// </summary>
        public const string TokenEndpoint = null;

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// Currently there is no default value for this endpoint, the endpoint differiates per customer.
        /// When we provide one cloud based oAuth solution we probably would configure the default endpoints.
        /// </summary>
        public const string UserInformationEndpoint = null;
        
        /// <summary>
        /// Default value for <see cref="OAuthOptions.RevokeTokenEndpoint"/>.
        /// Currently there is no default value for this endpoint, the endpoint differiates per customer.
        /// When we provide one cloud based oAuth solution we probably would configure the default endpoints.
        /// </summary>
        public const string RevokeTokenEndpoint = null;
        
    }
}