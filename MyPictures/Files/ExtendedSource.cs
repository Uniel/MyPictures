using System.Windows.Controls;

namespace MyPictures.Files
{
    class ExtendedSource : Image
    {
        public GenericMedia Media { get; }

        public ExtendedSource(GenericMedia media)
        {
            this.Media = media;
            this.Source = media.LoadThumbnail();
        }
    }
}
