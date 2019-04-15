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
using MyPictures.Storage;

namespace MyPictures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize UI.
            InitializeComponent();

            // Create library and Initialize.
            Library library = new Library();
            library.Initialize();

            // Find picture grid element.
            Grid Images = (Grid)this.FindName("ImageGrid");

            // TODO
            int x = 0, y = 0;
            library.GetLibrary().ForEach(generic => {
                // Create new Image element in XAML for a picture and fill in correct row/column
                Image image = new Image { Source = ((GenericImage) generic).RetrieveFrame() };
                image.SetValue(Grid.RowProperty, y);
                image.SetValue(Grid.ColumnProperty, x++);

                // For each image added to the grid add click event for preview
                image.MouseDown += (s, e) => {
                    this.PreviewGrid.Visibility = Visibility.Visible;
                    this.Preview.Source = ((Image)s).Source;

                    // Add click event to hide current preview
                    this.PreviewGrid.MouseDown += (sender, exception) =>
                    {
                        this.PreviewGrid.Visibility = Visibility.Hidden;
                    };

                };

                Images.Children.Add(image);

                // Go to next column at end of rows
                if (x == Images.ColumnDefinitions.Count)
                {
                    x = 0;
                    y++;
                    if (y >= 3)
                    {
                        // Sizing is off and images are automatically rotated?!?!
                        Images.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            });
        }
    }
}


