using System;
using System.IO;
using System.Linq;
using System.Windows;
using MyPicturesUWP.Files;
using MyPicturesUWP.Utils;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace MyPicturesUWP.Servers
{
    class LocalServer : IServer
    {
        protected string name;
        protected string directory;

        public LocalServer(string name, string directory)
        {
            // Save passed server name on object.
            this.name = name;

            // Trim ending backslash from directory and save.
            this.directory = directory.TrimEnd('\\');
        }

        public string GetName()
        {
            return this.name;
        }

        public Boolean FileExists(string path)
        {
            return File.Exists(path);
        }

        public List<string> GetFilePaths()
        {
            return Directory.EnumerateFiles(this.directory, "*.*", SearchOption.AllDirectories)
                .Where(path => ! path.StartsWith(this.GetThumbnailDirectory())).ToList();
        }

        public List<string> GetMediaPaths()
        {
            return this.GetFilePaths().Where(FileValidator.IsMediaFile).ToList();
        }

        public List<GenericImage> GetMediaGenerics()
        {
            return this.GetMediaPaths().Select(s => new GenericImage("source", s, this)).ToList();
        }

        public Stream GetMediaStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public string SaveMedia(string name, BitmapFrame frame)
        {
            // Generate path for new file.
            string path = this.GetThumbnailDirectory() + "\\" + name;

            // Create new file stream for found path.
            FileStream stream = new FileStream(path, FileMode.Create);

            // Create JPEG image encoder.
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            // Add frame and save to stream.
            encoder.Frames.Add(frame);
            encoder.Save(stream);

            // Close the file stream.
            stream.Close();

            // Return the final image path.
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
    }
}
