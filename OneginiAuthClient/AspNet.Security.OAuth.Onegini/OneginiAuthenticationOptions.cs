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

using System.Security.Claims;
using AspNet.Security.OAuth.Onegini;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Onegini
{
    
    /// <summary>
    /// Defines a set of options used by <see cref="OneginiAuthenticationHandler"/>.
    /// </summary>
    public class OneginiAuthenticationOptions : OAuthOptions{
        
        /// <summary>
        /// Get or set the endpoint used to revoke the access token
        /// </summary>
        public string RevokeTokenEndpoint { get; set; }
        
        /// <summary>
        /// Initialize a new instance of the <see cref="OneginiAuthenticationOptions"/> class.
        /// </summary>
        public OneginiAuthenticationOptions()
        {
            ClaimsIssuer = OneginiAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(OneginiAuthenticationDefaults.CallbackPath);
            
            //todo: Set default endpoints -> there is no deafult endpoint -> endpoints differ per customer
            AuthorizationEndpoint = OneginiAuthenticationDefaults.AuthorizationEndPoint;
            TokenEndpoint = OneginiAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OneginiAuthenticationDefaults.UserInformationEndpoint;
            SaveTokens = true; //Now we can access the access token later on in the application
            
            //Consideration -> you can add default claims
            //ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            //ClaimActions.MapJsonKey(ClaimTypes.Name, "sub");
            //ClaimActions.MapJsonKey(ClaimTypes.Email, "sub");
            //ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "date_of_birth");
            //ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            //ClaimActions.MapJsonKey(ClaimTypes.StreetAddress, "address");
            //ClaimActions.MapJsonKey(ClaimTypes.PostalCode, "zip_code");
        }
        
    }
}