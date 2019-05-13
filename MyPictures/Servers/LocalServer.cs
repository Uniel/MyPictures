using System;
using System.IO;
using System.Linq;
using System.Windows;
using MyPictures.Files;
using MyPictures.Utils;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Threading.Tasks;
using MyPictures.Auth;

namespace MyPictures.Servers
{
    class LocalServer : Server
    {
        public LocalServer(string name, string directory, OAuthProvider provider = null) : base(name, directory, provider)
        {
            //
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public override List<string> GetFilePaths()
        {
            return Directory.EnumerateFiles(this.directory, "*.*", SearchOption.AllDirectories)
                .Where(path => ! path.StartsWith(this.GetThumbnailDirectory())).ToList();
        }

        public override Stream GetMediaStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override string UploadMediaStream(string path, Stream stream)
        {
            // Create new file stream for found path.
            FileStream location = new FileStream(path, FileMode.OpenOrCreate);

            // Reset to stream to start of contents.
            stream.Seek(0, SeekOrigin.Begin);

            // Copy contents to file stream.
            stream.CopyTo(location);

            // Close the file stream. 
            location.Close();

            // Return the final path.
            return path;
        }

        public string GetThumbnailDirectory()
        {
            return this.directory + "\\.thumbnails";
        }

        public void CreateThumbnailsDirectory()
        {
            // Find thumbnails directory based on base.
            string directory = this.GetThumbnailDirectory();

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

        public override List<string> GetAlbumPaths()
        {
            // Get the directories in working directory - which are not thumbnail dir
            List<string> dirs = new List<string>(Directory.EnumerateDirectories(this.directory))
                .Where(path => !path.StartsWith(this.GetThumbnailDirectory())).ToList();

            List<string> albums = new List<string>();
            // Evaluate contents of each directory
            foreach(string dir in dirs)
            {
                // Check for any media files
                if(Directory.GetFiles(dir)
                    .Where(path => FileValidator.IsMediaFile(path)) != null)
                {
                    albums.Add(dir);
                }
            }
            return albums;
        }
    }
}
