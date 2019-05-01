using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyPictures.Auth
{
    class OAuthAuthorization
    {
        public string scopes;
        public DateTime expires;

        public string access_token;
        public string refresh_token;
        
        public bool HasExpired()
        {
            return this.expires < DateTime.Now;
        }

        public static void Merge(OAuthAuthorization a, OAuthAuthorization b)
        {
            if (a != null && b != null)
            {
                if (a.scopes == null) a.scopes = b.scopes;
                if (a.expires == null) a.expires = b.expires;
                if (a.access_token == null) a.access_token = b.access_token;
                if (a.refresh_token == null) a.refresh_token = b.refresh_token;
            }
        }
    }

    abstract class OAuthProvider
    {
        public abstract string Name { get; }

        protected OAuthAuthorization authorization;

        protected static readonly HttpClient http = new HttpClient();

        public OAuthProvider(string provider)
        {
            // Attempt to parse provider.
            try
            {
                // Parse the passed provider JSON.
                JObject obj = JObject.Parse(provider);

                // Skip if missing access token
                if (!obj.TryGetValue("access_token", out JToken token)) return;

                // Cast read JSON to authorization object.
                this.authorization = obj.ToObject<OAuthAuthorization>();

                // Refresh token if has expired.
                if (this.authorization.HasExpired())
                {
                    this.Refresh();
                }
            } catch (Exception) { /* Ignore Exception */ }
        }

        public bool IsConnected()
        {
            return this.authorization != null && this.authorization.access_token != null;
        }

        public void Redirect()
        {
            // Open the OAuth authorization window.
            System.Diagnostics.Process.Start(this.GetRedirectAddress());

            // Request authorization token from user. @wip - Replace with WPF form?
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter in the authorization token", this.Name + " Authorization", "", 0, 0);
            if (input != "") this.Authorize(input);
        }

        public void Refresh()
        {
            // Attempt to refresh the authorization details.
            this.Authorize(this.authorization.refresh_token, "refresh");
        }

        public async void Authorize(string code, string type = "authorize")
        {
            /**
             * The following should really be done by passing the data to some server-side application.
             * This is because the OAuth secret cannot be saved safely on a desktop application.
             *
             * It's done here on the application side for simplicity.
             */

            // Encode the requested authorization payload.
            FormUrlEncodedContent payload = new FormUrlEncodedContent(this.GetAuthorizationPayload(code, type));

            // Make async POST request to authorization address with payload
            HttpResponseMessage response = await http.PostAsync(this.GetAuthorizationAddress(code, type), payload);

            // Retrieve the data as a string and parse it.
            string rawResponse = await response.Content.ReadAsStringAsync();
            OAuthAuthorization parsed = this.ParseResponse(rawResponse);

            // Merge the parsed and existing authorization instance.
            // This is needed for refreshing where some fields is not present.
            OAuthAuthorization.Merge(parsed, this.authorization);

            // Save parsed authorization object on instance.
            this.authorization = parsed;

            // Save OAuth Authorization details in user settings.
            Properties.Settings.Default[this.Name + "Provider"] = JsonConvert.SerializeObject(this.authorization);
            Properties.Settings.Default.Save();
        }

        abstract protected string GetRedirectAddress();

        abstract protected string GetAuthorizationAddress(string code, string type);

        abstract protected OAuthAuthorization ParseResponse(string response);

        abstract protected Dictionary<string, string> GetAuthorizationPayload(string code, string type);
    }
}
