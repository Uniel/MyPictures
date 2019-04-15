using MyPictures.Files;
using MyPictures.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyPictures.Storage
{
    class Thumbnailer
    {
        public BitmapFrame Generate(BitmapFrame frame)
        {
            // Create frame from first image in object to ensure a picture
            //BitmapFrame frame = ((GenericImage)media).RetrieveFrame(0);

            // Scale values for width and height - ensures 1:1 ratio
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

            // Return the transformation as a bitmapframe
            return BitmapFrame.Create((BitmapSource)transformed);
        }

        protected Boolean ThumbnailExists(String path)
        {
            return false;
        }

    }
}
