using System;
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
        protected String thumbDir;

        public LocalServer(String directory, String thumbDir = "")
        {
            // Removes last index of directory input it's a backslash
            if( (directory.LastIndexOf("\\") + 1) == directory.Length)
            {
                directory = directory.Substring(0, directory.Length - 2);
            }
            // Removes last index of thumbdir if it's a backslash
            if ( ((thumbDir.LastIndexOf("\\") + 1) == thumbDir.Length) && thumbDir != "")
            {
                thumbDir = thumbDir.Substring(0, thumbDir.Length - 2);
            }

            this.directory = directory;
            this.thumbDir = (thumbDir == "") ? (directory + "\\.thumbnails") : thumbDir;
            CreateThumbnailDirectory();
        }

        public List<String> GetFilePaths()
        {
            return Directory.EnumerateFiles(this.directory, "*.*", SearchOption.AllDirectories).ToList();
        }

        public List<String> GetMediaPaths()
        {
            return this.GetFilePaths().Where(FileValidator.IsMediaFile).ToList();
        }

        public List<GenericImage> GetMediaGenerics()
        {
            return this.GetMediaPaths().Select(s => new GenericImage(s, this)).ToList();
        }

        public Stream GetMediaStream(String path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        // Thumbnails
        public void CreateThumbnailDirectory()
        {
            // If thumbnail directory does not exist
            if (!Directory.Exists(thumbDir))
            {
                // Create directory and make it hidden
                try
                {
                    Directory.CreateDirectory(thumbDir);
                    DirectoryInfo DirInfo = new DirectoryInfo(thumbDir)
                    {
                        Attributes = FileAttributes.Hidden
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
