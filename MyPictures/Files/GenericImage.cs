using System;
using System.IO;
using MyPictures.Servers;
using System.Windows.Media.Imaging;

namespace MyPictures.Files
{
    class GenericImage : GenericMedia, IGenericMedia
    {
        public GenericImage(String path, IServer server) : base(path, server)
        {
            //
        }

        public override BitmapDecoder Decode(Stream stream)
        {
            return BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.None,
                BitmapCacheOption.Default
            );
        }

        public override BitmapFrame RetrieveFrame(int frame = 0)
        {
            // Get decoder for file stream.
            BitmapDecoder decoder = this.Decode(this.Stream());
            BitmapFrame image = decoder.Frames[frame];
            image = this.CorrectRotation(image);
            // kald thumb ting
            this.thumbnailer.Generate(image);
            // Return passed frame index.
            return image;
        }

        public override BitmapMetadata RetrieveMetadata(int frame = 0)
        {
            return (BitmapMetadata) this.RetrieveFrame(frame).Metadata;
        }

        public override BitmapMetadata RetrieveMetadata(BitmapFrame frame)
        {
            return (BitmapMetadata) frame.Metadata;
        }
    }
}
