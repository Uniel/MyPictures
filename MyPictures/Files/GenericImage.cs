using System;
using System.IO;
using MyPictures.Servers;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;

namespace MyPictures.Files
{
    class GenericImage : GenericMedia
    {
        public GenericImage(string type, string path, IServer server) : base(type, path, server)
        {
            //
        }

        public override BitmapFrame LoadSource()
        {
            return this.RetrieveFrame();
        }

        public override BitmapFrame LoadThumbnail()
        {
            // Return source image if no thumbnail exists.
            if (this.Thumbnail == null) {
                return this.LoadSource();
            }

            // Return the thumbnail source image.
            return this.Thumbnail.LoadSource();
        }
        
        public override BitmapDecoder Decode(Stream stream)
        {
            return PngBitmapDecoder.Create(
                stream,
                BitmapCreateOptions.None,
                BitmapCacheOption.Default
            );
        }

        public BitmapFrame RetrieveFrame(int frame = 0)
        {
            // Get decoder for file stream.
            BitmapDecoder decoder = this.Decode(this.Stream());

            // Get and return request frame with fixed orientation.
            return this.CorrectOrientation(decoder.Frames[frame]);
        }

        public override BitmapMetadata RetrieveMetadata(BitmapFrame frame)
        {
            return (BitmapMetadata)frame.Metadata;
        }
    }
}
