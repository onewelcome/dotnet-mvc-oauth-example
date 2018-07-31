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
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;

namespace OneginiAuthClient.Onegini.AspNetCore.Authentication.OneginiBearer
{
    /// <summary>
    /// Options that need to be configurated inside the `ConfigureServices` method inside the startup.cs file.
    /// The options `TokenIntrospectionEndpoint`, `ClientId` and `ClientSecret` are required!
    /// </summary>
    public class OneginiBearerOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// The default name of the scheme used to indentify this scheme by the asp.net authentication middleware
        /// </summary>
        public const string DefaultScheme = "OneginiBearerScheme";
        
        /// <summary>
        /// Gives the option to overwrite the default scheme name
        /// </summary>
        public string Scheme => DefaultScheme;
        
        /// <summary>
        /// Set the endpoint used to validate the access token 
        /// </summary>
        public string TokenIntrospectionEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the client id for the Token Introspection Endpoint API
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Gets or sets the client secret for the Token Introspection Endpoint API
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The HttpClient used to query the Endpoint, in default it is set but you can overwrite it if it is neccesary.
        /// </summary>
        public HttpClient Backchannel { get; set; }
        
        /// <summary>
        /// The BackchannelHttpHandler used by the Backchannel HttpClient, in default it is set but you can overwrite it if it is neccesary.
        /// </summary>
        public HttpClientHandler BackchannelHttpHandler { get; set; }
        
        /// <summary>
        /// The timeout used by the backchannel, in default it is set to 10 sec. Example -> `BackchannelTimeout = TimeSpan.FromSeconds(10)`
        /// </summary>
        public TimeSpan BackchannelTimeout = TimeSpan.FromSeconds(10);

        public OneginiBearerOptions()
        {
            Backchannel = new HttpClient(BackchannelHttpHandler ?? new HttpClientHandler());
            Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Onegini Bearer handler");
            Backchannel.Timeout = BackchannelTimeout;
            Backchannel.MaxResponseContentBufferSize = 10485760L;
        }
    }
}