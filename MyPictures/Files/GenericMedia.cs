using System;
using System.IO;
using MyPictures.Utils;
using MyPictures.Servers;
using System.Windows.Media.Imaging;
using MyPictures.Storage;

namespace MyPictures.Files
{
    abstract class GenericMedia : IGenericMedia
    {
        protected String path;
        protected IServer server;

        protected Thumbnailer thumbnailer;

        public GenericMedia(String path, IServer server)
        {
            this.path = path.TrimEnd('\\');
            this.server = server;

            this.thumbnailer = new Thumbnailer();
        }

        public Stream Stream()
        {
            return this.server.GetMediaStream(this.path);
        }

        public String GetPath()
        {
            return this.path;
        }

        public String GetName()
        {
            // TODO: Replace with cross server implementation.
            return Path.GetFileName(this.path);
        }

        protected BitmapFrame CorrectRotation()
        {
            // Get the metadata from the first frame.
            BitmapFrame frame = this.RetrieveFrame();
            BitmapMetadata data = this.RetrieveMetadata(frame);

            // Get the file orientation from metadata.
            ushort orientation = (ushort) data.GetQuery("System.Photo.Orientation");

            // Return original if has no metadata or orientation.
            if (data == null || orientation == 1) {
                return frame;
            }

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
        
        public abstract BitmapDecoder Decode(Stream stream);

        public abstract BitmapFrame RetrieveFrame(int frame = 0);

        public abstract BitmapMetadata RetrieveMetadata(int frame = 0);

        public abstract BitmapMetadata RetrieveMetadata(BitmapFrame frame);
    }
}
