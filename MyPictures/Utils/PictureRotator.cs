using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyPictures.Utils
{
    class PictureRotator
    {
        private const string _orientationQuery = "System.Photo.Orientation";

        public BitmapFrame RotatePicture(BitmapFrame picture)
        {
            // Read the meta data of the input BitmapFrame
            BitmapMetadata bitmapMetadata = picture.Metadata as BitmapMetadata;

            // If metadata exists and contains orientational data
            if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery(_orientationQuery)))
            {
                // Get orientational data and rotate accordingly
                object o = bitmapMetadata.GetQuery(_orientationQuery);

                if (o != null)
                {
                    //refer to http://www.impulseadventure.com/photo/exif-orientation.html for details on orientation values
                    switch ((ushort)o)
                    {
                        // Case 6 is 90 degrees rotation
                        case 6:
                            //                            return 90D;
                            return BitmapFrame.Create(new TransformedBitmap(picture, new RotateTransform(90.0)));
                        // Case 3 is 180 degrees rotation
                        case 3:
                            //                            return 180D;
                            return BitmapFrame.Create(new TransformedBitmap(picture, new RotateTransform(180.0)));
                        // Case 8 is 270 degrees rotation
                        case 8:
                            //                              return 270D;
                            return BitmapFrame.Create(new TransformedBitmap(picture, new RotateTransform(270.0)));
                    }
                }
            }
            // If no rotational metadata is found return picture as is
            return picture;
        }
    }
}
