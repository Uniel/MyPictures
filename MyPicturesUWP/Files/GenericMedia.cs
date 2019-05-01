﻿using System;
using System.IO;
using MyPicturesUWP.Utils;
using MyPicturesUWP.Servers;
using System.Windows.Media.Imaging;
using MyPicturesUWP.Storage;

namespace MyPicturesUWP.Files
{
    abstract class GenericMedia
    {
        protected string path;
        protected string type = "source";

        public IServer Server { get; }

        public MediaData Data { get; set; }

        public GenericImage Thumbnail { get; set; }

        public GenericMedia(string type, string path, IServer server)
        {
            this.type = type;
            this.Server = server;
            this.path = path.TrimEnd('\\');
        }

        public Stream Stream()
        {
            return this.Server.GetMediaStream(this.path);
        }

        public string GetPath()
        {
            return this.path;
        }

        public string GetName()
        {
            return Path.GetFileName(this.path);
        }

        protected BitmapFrame CorrectOrientation(BitmapFrame frame)
        {
            // Get the metadata from the first frame.
            BitmapMetadata data = this.RetrieveMetadata(frame);

            // Get the file orientation from metadata.
            object query = data.GetQuery("System.Photo.Orientation");

            // Return original if has no metadata or orientation.
            if (data == null || query == null) {
                return frame;
            }

            // Cast query to readable ushort.
            ushort orientation = (ushort) query;

            // Apply orientation change to parent based on metadata.
            switch (orientation) {
                case 6: return PictureRotator.Rotate(frame, 90);
                case 3: return PictureRotator.Rotate(frame, 180);
                case 8: return PictureRotator.Rotate(frame, 270);
            }

            // TODO: Implement rest of orientation changes.

            // Return the frame if no change has been applied.
            return frame;
        }

        public abstract BitmapFrame LoadSource();

        public abstract BitmapFrame LoadThumbnail();
        
        public abstract BitmapDecoder Decode(Stream stream);

        public abstract BitmapMetadata RetrieveMetadata(BitmapFrame frame);
    }
}
