using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPictures.Utils
{
    class FileValidator
    {
        public static Boolean IsMediaFile(String path)
        {
            return FileValidator.IsImageFile(path) || FileValidator.IsVideoFile(path);
        }
        public static Boolean IsImageFile(String path)
        {
            return path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg");
        }

        public static Boolean IsVideoFile(String path)
        {
            // TODO: Add suppported video files.
            return false;
        }
    }
}
