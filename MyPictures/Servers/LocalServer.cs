using System;
using System.IO;
using System.Linq;
using System.Windows;
using MyPictures.Files;
using MyPictures.Utils;
using System.Collections.Generic;

namespace MyPictures.Servers
{
    class LocalServer : IServer
    {
        protected String directory;
        protected String thumbnails = "\\.thumbnails";

        public LocalServer(String directory)
        {
            // Trim ending backslash from directory and save.
            this.directory = directory.TrimEnd('\\');
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
 
        public void CreateThumbnailsDirectory()
        {
            // Find thumbnails directory based on base.
            string directory = this.directory + this.thumbnails;

            // Check if thumbnail directory does not exist.
            if (! Directory.Exists(directory))
            {
                try {
                    // Create new directory.
                    Directory.CreateDirectory(directory);

                    // Mark the directory as hidden.
                    DirectoryInfo DirInfo = new DirectoryInfo(directory) {
                        Attributes = FileAttributes.Hidden
                    };
                } catch (Exception) {
                    // Show failed thumnail initialization warning.
                    MessageBox.Show("Failed to create thumbnails directory!", "Initialization Failed");
                    throw;
                }
            }
        }
    }
}
