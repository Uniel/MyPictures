using MyPictures.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MyPictures.Auth
{
    class GoogleProvider : OAuthProvider
    {
        public override string Name => "google";

        private readonly string scopes = "https://www.googleapis.com/auth/drive";

        private readonly string redirect = "urn:ietf:wg:oauth:2.0:oob";

        private readonly string client = Secrets.Get("google:client");
        private readonly string secret = Secrets.Get("google:secret");

        public GoogleProvider(string provider) : base(provider)
        {
            //
        }

        protected override string GetRedirectAddress()
        {
            return "https://accounts.google.com/o/oauth2/v2/auth" +
                "?client_id=" + this.client +
                "&scope=" + this.scopes +
                "&redirect_uri=" + this.redirect +
                "&response_type=code";
        }

        protected override string GetAuthorizationAddress(string code, string type)
        {
            return "https://www.googleapis.com/oauth2/v4/token";
        }

        protected override Dictionary<string, string> GetAuthorizationPayload(string code, string type)
        {
            // Find payload key to pass code as.
            string CodeField = (type == "authorize"
                ? "code"
                : "refresh_token"
            );

            // Find OAuth grant type.
            string GrantType = (type == "authorize"
                ? "authorization_code"
                : "refresh_token"
            );

            // Create dictionary payload list.
            return new Dictionary<string, string>
            {
                { CodeField, code },

                { "client_id", this.client },
                { "client_secret", this.secret },
                { "redirect_uri", this.redirect },

                { "grant_type", GrantType }
            };
        }

        protected override OAuthAuthorization ParseResponse(string response)
        {
            // Parse the passed response.
            dynamic content = JObject.Parse(response);

            // Create and return new response object for content.
            return new OAuthAuthorization
            {
                access_token = content.access_token,
                refresh_token = content.refresh_token,

                scopes = content.scope,
                expires = DateTime.Now.AddMinutes(
                    Convert.ToDouble(content.expires_in)
                ),
            };
        }
    }
}
