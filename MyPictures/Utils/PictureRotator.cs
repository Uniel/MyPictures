﻿using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyPictures.Utils
{
    static class PictureRotator
    {
        public static BitmapFrame Rotate(BitmapFrame picture, double angle)
        {
            return BitmapFrame.Create(new TransformedBitmap(picture, new RotateTransform(angle)), null, picture.Metadata as BitmapMetadata, null);
        }
    }
}
