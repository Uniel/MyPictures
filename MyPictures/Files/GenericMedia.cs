using System;
using System.IO;
using MyPictures.Utils;
using MyPictures.Servers;

namespace MyPictures.Files
{
    class GenericMedia
    {
        protected String path;
        protected IServer server;

        public GenericMedia(String path, IServer server)
        {
            this.path = path;
            this.server = server;
        }

        public Stream Stream()
        {
            // TODO: Implement GenericVideo
            return FileValidator.IsImageFile(this.path)
                ? ((GenericImage) this).Stream()
                : null;
        }
    }
}
