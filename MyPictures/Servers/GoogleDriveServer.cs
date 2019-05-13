﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using MyPictures.Files;
using MyPictures.Utils;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using MyPictures.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MyPictures.Servers
{
    class GoogleDriveServer : Server
    {
        public GoogleDriveServer(string name, string directory, OAuthProvider provider = null) : base(name, directory, provider)
        {
            //
        }

        public override bool FileExists(string path)
        {
            // TODO: Currently only used by thumbnailer.
            return false;
        }

        public override List<string> GetFilePaths()
        {
            // Add Google Drive OAuth authorization bearer token.
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.provider.GetToken());

            // Send get request to files endpoint.
            HttpResponseMessage response = http.GetAsync("https://www.googleapis.com/drive/v3/files").Result;

            // Get and parse JSON response.
            string rawResponse = response.Content.ReadAsStringAsync().Result;
            dynamic parsed = JObject.Parse(rawResponse);

            // Cast files array to list of dynamics.
            List<dynamic> files;
            try
            {
                 files = parsed.files.ToObject<List<dynamic>>();
            }
            catch (Exception)
            {
                Console.WriteLine("An error occourred when trying to fetch pictures from Drive, check authentication settings.");
                return new List<string>();
            }

            // Map file objects into file names and cast to list.
            return files.Select(file => {
                // Cast name to a string.
                string name = (string) file.name;

                // Combine id and file extension.
                return (string) file.id + "." + name.Split('.').Last();
            }).Cast<string>().ToList();
        }

        public override Stream GetMediaStream(string path)
        {
            // Add Google Drive OAuth authorization bearer token.
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.provider.GetToken());

            // Send get request to files endpoint.
            string id = path.Split('.').First();
            HttpResponseMessage response = http.GetAsync("https://www.googleapis.com/drive/v3/files/" + id + "?alt=media").Result;
            
            // Read the response as a byte array and convert to stream.
            return new MemoryStream(response.Content.ReadAsByteArrayAsync().Result);
        }

        public override List<string> GetAlbumPaths()
        {
            // TODO implement albums for Drive
            return new List<string>();
        }
    }
}