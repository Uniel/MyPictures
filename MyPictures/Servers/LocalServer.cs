﻿using System;
using System.IO;
using System.Linq;
using MyPictures.Files;
using MyPictures.Utils;
using System.Collections.Generic;

namespace MyPictures.Servers
{
    class LocalServer : IServer
    {
        protected String directory;

        public LocalServer(String directory)
        {
            this.directory = directory;
        }

        public List<String> GetFilePaths()
        {
            return Directory.EnumerateFiles(this.directory, "*.*", SearchOption.AllDirectories).ToList();
        }

        public List<String> GetMediaPaths()
        {
            return this.GetFilePaths().Where(FileValidator.IsMediaFile).ToList();
        }

        public List<GenericMedia> GetMediaGenerics()
        {
            return this.GetMediaPaths().Select(s => new GenericMedia(s, this)).ToList();
        }

        public Stream GetMediaStream(String path)
        {
            return null; // TODO: Implement stream
        }
    }
}
