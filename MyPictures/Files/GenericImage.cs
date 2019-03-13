using System;
using System.IO;
using MyPictures.Servers;

namespace MyPictures.Files
{
    class GenericImage : GenericMedia
    {
        public GenericImage(String path, IServer server) : base(path, server)
        {
            //
        }

        public new Stream Stream()
        {
            Stream stream = this.server.GetMediaStream(this.path);

            // TODO: Implement stream.
            return null;
        }
    }
}
