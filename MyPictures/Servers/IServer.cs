using System;
using System.IO;
using MyPictures.Files;
using System.Collections.Generic;

namespace MyPictures.Servers
{
    interface IServer
    {
        List<String> GetFilePaths();

        List<String> GetMediaPaths();

        List<GenericMedia> GetMediaGenerics();

        Stream GetMediaStream(String path);
    }
}
