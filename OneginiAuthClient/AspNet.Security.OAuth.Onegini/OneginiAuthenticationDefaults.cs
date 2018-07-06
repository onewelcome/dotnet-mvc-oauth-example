using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Onegini
{
    /// <summary>
    /// Default values used by the Onegini authentication middleware.
    /// </summary>
    public class OneginiAuthenticationDefaults
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
        public const string CallbackPath = "/signin-external";

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
    }
}