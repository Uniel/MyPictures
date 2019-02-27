using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyPictures
{
    class Library
    {
        private readonly string DirLocation = @"C:\Users\John\Pictures\Screenshots";
        public List<string> GetMediaPaths()
        {
            //Directory.GetFiles(DirLocation).Select(Path.GetFileName);
            //string[] filters = { "png", "jpeg", "jpg" };
            //string[] files = Directory.GetFiles(DirLocation, filters,SearchOption.TopDirectoryOnly);
            var files = Directory.EnumerateFiles(DirLocation, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg") );
            return new List<string>(files);
        }

        public List<BitmapImage> GetMediaSources()
        {
            return this.GetMediaPaths().Select(path => new BitmapImage(new Uri(path, UriKind.Absolute))).ToList();
        }

        public string test()
        {
            return "TESTTESTTESTTESTTESTTEST";
        }

    }
}
