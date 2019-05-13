﻿using System;
using System.IO;
using MyPictures.Files;
using MyPictures.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using MyPictures.Auth;

namespace MyPictures.Servers
{
    abstract class Server
    {
        protected string name;
        protected string directory;
        protected OAuthProvider provider;
        protected static readonly HttpClient http = new HttpClient();

        public Server(string name, string directory, OAuthProvider provider = null)
        {
            // Save passed data on object.
            this.name = name;
            this.provider = provider;

            // Trim ending backslash from directory and save.
            this.directory = directory.TrimEnd('\\');
        }

        public string GetName()
        {
            return this.name;
        }

        public List<string> GetMediaPaths()
        {
            //List<string> paths = await this.GetFilePaths();
            return this.GetFilePaths().Where(FileValidator.IsMediaFile).ToList();
        }

        public List<GenericImage> GetMediaGenerics()
        {
            return this.GetMediaPaths().Select(s => new GenericImage("source", s, this)).ToList();
        }

        public abstract bool FileExists(string path);

        public abstract List<string> GetFilePaths();

        public abstract Stream GetMediaStream(string path);

        public abstract List<string> GetAlbumPaths();
    }
}