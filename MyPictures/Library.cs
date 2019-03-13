using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace MyPictures
{
    class Library
    {
        private readonly string DirLocation = @"C:\Users\Andreas\Pictures\dogs";
        public List<string> GetMediaPaths()
        {
            var files = Directory.EnumerateFiles(DirLocation, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg"))
            .OrderByDescending(filename => filename);
            return new List<string>(files);
        }

        public List<BitmapImage> GetMediaSources()
        {
            return this.GetMediaPaths().Select(path => new BitmapImage(new Uri(path, UriKind.Absolute))).ToList();
        }

        /*public List<BitmapImage> GetMetaData()
        {
           this.GetMediaSources().First().PropertyItems;
        }*/

    }
}
