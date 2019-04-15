using System;
using System.IO;
using MyPictures.Utils;
using MyPictures.Servers;
using System.Windows.Media.Imaging;
using MyPictures.Storage;

namespace MyPictures.Files
{
    class GenericMedia
    {
        protected String path;
        protected IServer server;

        protected Thumbnailer thumbnailGenerator;

        public GenericMedia(String path, IServer server)
        {
            this.path = path;
            this.server = server;
            this.thumbnailGenerator = new Thumbnailer();
        }

        public Stream Stream()
        {
            return this.server.GetMediaStream(this.path);
        }

        public String GetPath ()
        {
            // Find last backslash
            int Pos = this.path.LastIndexOf("\\") + 1;
            return this.path.Substring(0,Pos);
        }

        public String GetName()
        {
            return Path.GetFileName(this.path);
        }

        protected BitmapFrame CorrectRotation(BitmapFrame image)
        {
            // Read the meta data of the input BitmapFrame
            BitmapMetadata bitmapMetadata = image.Metadata as BitmapMetadata;

            // If metadata exists and contains orientational data
            if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery("System.Photo.Orientation")))
            {
                // Get orientational data and rotate accordingly
                object o = bitmapMetadata.GetQuery("System.Photo.Orientation");

                if (o != null)
                {
                    //refer to http://www.impulseadventure.com/photo/exif-orientation.html for details on orientation values
                    switch ((ushort)o)
                    {
                        // Case 6 is 90 degrees rotation
                        case 6:
                            return PictureRotator.Rotate(image, 90);

                        // Case 3 is 180 degrees rotation
                        case 3:
                            return PictureRotator.Rotate(image, 180.0);

                        // Case 8 is 270 degrees rotation
                        case 8:
                            return PictureRotator.Rotate(image, 270.0);
                    }
                }
            }
            return image;
        }

    }
}
