using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyPictures.Servers;
using MyPictures.Files;

namespace MyPictures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IServer server = new LocalServer(@"C:\Users\Andreas\Pictures\dogs");
            server.GetMediaPaths();

            int x = 0, y = 0;
            server.GetMediaGenerics().ForEach(generic => {
                // Find picture grid element.
                Grid Images = (Grid) this.FindName("ImageGrid");

                // Cast generic to image and retrieve frame.
                GenericImage source = (GenericImage)generic;
                BitmapFrame frame = source.RetrieveFrame(0);

                // Print metadata example to console.
                Console.WriteLine(source.RetrieveMetadata(frame).Copyright);

                // Create new Image element in XAML for a picture and fill in correct row/column
                Image image = new Image { Source = frame };
                image.SetValue(Grid.RowProperty, y);
                image.SetValue(Grid.ColumnProperty, x++);

                // For each image added to the grid add click event for preview
                image.MouseDown += (s, e) => {
                    Console.WriteLine(e);
                    this.PreviewGrid.Visibility = Visibility.Visible;
                    this.Preview.Source = ((Image)s).Source;

                    // Add click event to hide current preview
                    this.Preview.MouseDown += (sender, exception) =>
                    {
                        this.PreviewGrid.Visibility = Visibility.Hidden;
                    };

                } ;

                Images.Children.Add(image);

                // Go to next column at end of rows
                if(x == Images.ColumnDefinitions.Count)
                {
                    x = 0;
                    y++;
                    if (y >= 3)
                    {
                        // Sizing is off and images are automatically rotated?!?!
                        Images.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto } );
                    }
                }
            });

        }
    }
}


