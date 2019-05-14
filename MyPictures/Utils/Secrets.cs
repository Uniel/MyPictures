using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MyPictures.Utils
{
    static class Secrets
    {
        private static IEnumerable<KeyValuePair<string, JToken>> list;

        static Secrets()
        {
            // Show warning if secrets file is missing.
            if (! File.Exists("secrets.json"))
            {
                MessageBox.Show("Secrets file missing! OAuth will be unavailable.", "Loading Failed");
                return;
            }

            // Read all text from secrets file.
            string raw = File.ReadAllText("secrets.json");

            // Parse and save the JSON result.
            Secrets.list = JObject.Parse(raw);
        }

        public static string Get(string key)
        {
            return Secrets.list.First(item => item.Key == key).Value.ToString();
        }
    }
}
