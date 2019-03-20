using System;
using System.IO;
using MyPictures.Files;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MyPictures.Files
{
    interface IGenericMedia
    {
        Stream Stream();

        BitmapDecoder Decode(Stream stream);

        BitmapFrame RetrieveFrame(int frame = 0);

        BitmapMetadata RetrieveMetadata(int frame = 0);

        BitmapMetadata RetrieveMetadata(BitmapFrame frame);
    }
}
