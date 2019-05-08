using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPictures.Utils
{
    static class FileValidator
    {
        public static Boolean IsMediaFile(String path)
        {
            return FileValidator.IsImageFile(path) || FileValidator.IsVideoFile(path);
        }
        public static Boolean IsImageFile(String path)
        {
            return path.ToLower().EndsWith(".png") || path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg");
        }

        public static Boolean IsVideoFile(String path)
        {
            // TODO: Add suppported video files.
            return false;
        }

        public static Boolean SupportsOrientation(String path)
        {
            return path.ToLower().EndsWith(".tiff") || path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg");
        }
    }
}
