using System;
using System.IO;
using MyPicturesUWP.Files;
using System.Collections.Generic;

namespace MyPicturesUWP.Servers
{
    interface IServer
    {
        string GetName();

        List<String> GetFilePaths();

        List<String> GetMediaPaths();

        List<GenericImage> GetMediaGenerics();

        Stream GetMediaStream(String path);
    }
}
