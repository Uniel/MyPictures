using MyPictures.Files;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyPictures.Storage
{
    class ThumbnailGenerator
    {
        public BitmapFrame Generate(GenericMedia media)
        {
            // Create frame from first image in object to ensure a picture
            BitmapFrame frame = ((GenericImage)media).RetrieveFrame(0);
            
            // Scale values for width and height - ensures 1:1 ratio
            double scaleWidth = 300d / frame.PixelWidth;
            double scaleHeight = 300d / frame.PixelHeight;
   
            // Transform the picture
            ScaleTransform transform = new ScaleTransform(scaleWidth, scaleHeight);
            TransformedBitmap transformed = new TransformedBitmap(frame, transform);

            // Return the transformation as a bitmapframe
            return BitmapFrame.Create((BitmapSource)transformed);
        }

    }
}
