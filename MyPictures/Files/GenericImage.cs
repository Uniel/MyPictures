﻿using System;
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

        public BitmapDecoder Decode(Stream stream)
        {
            return BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.None,
                BitmapCacheOption.Default
            );
        }

        public BitmapFrame RetrieveFrame(int frame = 0)
        {
            // Get decoder for file stream.
            BitmapDecoder decoder = this.Decode(this.Stream());

            // Return passed frame index.
            return decoder.Frames[frame];
        }

        public BitmapMetadata RetrieveMetadata(int frame = 0)
        {
            return (BitmapMetadata) this.RetrieveFrame(frame).Metadata;
        }

        public BitmapMetadata RetrieveMetadata(BitmapFrame frame)
        {
            return (BitmapMetadata) frame.Metadata;
        }
    }
}
