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
            MyPictures.Library lib = new MyPictures.Library();

            int x = 0, y = 0;
            lib.GetMediaSources().ForEach(source => {
                // Find picture grid
                Grid Images = (Grid)this.FindName("ImageGrid");

                // Create new Image element in XAML for a picture and fill in correct row/column
                Image image = new Image { Source = source };
                image.SetValue(Grid.RowProperty, y);
                image.SetValue(Grid.ColumnProperty, x++);

                image.MouseDown += (s,e) => {
                    this.PreviewGrid.Visibility = Visibility.Visible;
                    this.Preview.Source = ((Image)s).Source;
                } ;

                Images.Children.Add(image);

                // Go to next column at end of rows
                if(x == Images.ColumnDefinitions.Count)
                {
                    x = 0;
                    y++;
                }
            });

        }
    }
}


