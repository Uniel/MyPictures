using System;
using System.IO;
using System.Linq;
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
    }
}
