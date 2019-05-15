namespace MyPictures.Utils
{
    static class FileValidator
    {
        public static bool IsMediaFile(string path)
        {
            return FileValidator.IsImageFile(path) || FileValidator.IsVideoFile(path);
        }
        public static bool IsImageFile(string path)
        {
            return path.ToLower().EndsWith(".png") || path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg");
        }

        public static bool IsVideoFile(string path)
        {
            // TODO: Add suppported video files.
            return false;
        }

        public static bool SupportsOrientation(string path)
        {
            return path.ToLower().EndsWith(".tiff") || path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg");
        }
    }
}
