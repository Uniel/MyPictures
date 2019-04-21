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
        public static BitmapFrame Rotate(BitmapFrame picture, double angle)
        {
            return BitmapFrame.Create(new TransformedBitmap(picture, new RotateTransform(angle)), null, picture.Metadata as BitmapMetadata, null);
        }
    }
}
