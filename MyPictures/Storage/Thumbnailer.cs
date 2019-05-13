using MyPictures.Files;
using MyPictures.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyPictures.Servers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.IO;

namespace MyPictures.Storage
{
    class Thumbnailer
    {
        protected LocalServer server;

        public Thumbnailer(LocalServer server)
        {
            this.server = server;
        }

        public Boolean Process(GenericMedia source)
        {
            // Check and load thumbnail if exists.
            if (this.Exists(source))
            {
                this.Load(source);
                return false;
            }

            // Generate new thumbnail.
            this.Generate(source);
            return true;
        }
        public delegate Boolean AsyncMethodCaller(GenericMedia source);

        public void Load(GenericMedia source)
        {
            // Find path for source thumbnail.
            string path = this.MediaPath(source);

            // Add new thumbnail media to source image.
            source.Thumbnail = new GenericImage("thumbnail", path, this.server);
        }

        protected void Generate(GenericMedia source)
        {
            // Load first frame from source.
            BitmapFrame frame = source.LoadSource();

            // Find constants to scale image to 1:1 ratio.
            double scaleWidth = 300d / frame.PixelWidth;
            double scaleHeight = 300d / frame.PixelHeight;
            double scale = (scaleWidth > scaleHeight) ? scaleHeight : scaleWidth;

            // Transform the picture
            // https://stackoverflow.com/questions/22013835/how-to-effectively-crop-and-scale-image-data
            ScaleTransform transform = new ScaleTransform(scale, scale); //(scaleWidth, scaleHeight);
            double size = (frame.PixelHeight > frame.PixelWidth) ? frame.PixelWidth : frame.PixelHeight;
            int startX = (frame.PixelWidth == (int)size) ? 0 : (int)((frame.PixelWidth/ 2) - (0.5* size));
            int startY = (frame.PixelHeight == (int)size) ? 0 : (int)((frame.PixelHeight / 2) - (0.5*size));

            // Crop picture and create new transformation, with 1:1 ratio
            CroppedBitmap crop = new CroppedBitmap(frame, new Int32Rect(startX, startY, (int)size, (int)size));
            TransformedBitmap transformed = new TransformedBitmap(crop, transform);

            // Check picture filetype support for rotation
            BitmapMetadata ThumbnailMeta = ((BitmapMetadata)frame.Metadata).Clone();
            if (FileValidator.SupportsOrientation(source.GetPath()))
            {
                ThumbnailMeta.SetQuery("System.Photo.Orientation", 1);
            }

            // Convert transformed to bitmap frame.
            BitmapFrame thumbnail = BitmapFrame.Create(transformed as BitmapSource, null, ThumbnailMeta, null);

            // Generate unique filename.
            string name = source.Data.ID.ToString();
            name += '.' + source.GetName().Split('.').Last();

            // Create bitmap encoder and add frame.
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(thumbnail);

            // Create stream and save encoder contents.
            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);

            // Save thumbnail in local storage.
            string path = this.server.GetThumbnailDirectory() + "\\" + name;
            this.server.UploadMediaStream(path, stream);

            // Add new thumbnail media to source image.
            source.Thumbnail = new GenericImage("thumbnail", path, this.server);
            source.Data.Thumbnail = source.Thumbnail.GetPath();
        }

        protected string MediaPath(GenericMedia source)
        {
            // Return thumbnail or null if not set.
            return source.Data?.Thumbnail;
        }
 
        protected bool Exists(GenericMedia source)
        {
            // Get thumbnail path from source.
            string path = this.MediaPath(source);
            // Check that path is not null and file exists.
            return path != null && this.server.FileExists(path);
        }
    }
}
