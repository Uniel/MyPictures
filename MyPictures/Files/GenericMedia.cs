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
            return this.server.GetMediaStream(this.path);
        }

        public String GetPath ()
        {
            // Find last backslash
            int Pos = this.path.LastIndexOf("\\") + 1;
            return this.path.Substring(0,Pos);
        }

        public String GetName()
        {
            return Path.GetFileName(this.path);
        }
    }
}
