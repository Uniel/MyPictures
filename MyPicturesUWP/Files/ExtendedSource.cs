using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyPicturesUWP.Files
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
