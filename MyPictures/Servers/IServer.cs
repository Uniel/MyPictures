using System;
using System.IO;
using System.Collections.Generic;

namespace MyPictures.Servers
{
    interface IServer
    {
        List<String> GetFilePaths();

        List<String> GetMediaPaths();

        Stream GetFileStream(String path);
    }
}
