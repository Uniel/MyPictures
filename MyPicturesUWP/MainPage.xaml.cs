using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyPicturesUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // Create library and Initialize.
            Library library = new Library();
            library.Initialize();

            // Find picture grid element.
            int x = 0, y = 0;
            Grid grid = this.FindName("ImageGrid") as Grid;

            // Loop through the whole library.
            library.GetLibrary().ForEach(generic => {
                // Create new extended image source.
                ExtendedSource image = new ExtendedSource(generic);

                // Set image x and y position.
                image.SetValue(Grid.RowProperty, y);
                image.SetValue(Grid.ColumnProperty, x++);

                // Add image mouse click event handler.
                image.MouseDown += (s, e) => {
                    // Enable preview and set full image source.
                    this.PreviewGrid.Visibility = Visibility.Visible;
                    this.Preview.Source = ((ExtendedSource)s).Media.LoadSource();

                    // Hide preview when clicking outside image area.
                    this.PreviewGrid.MouseDown += (sender, exception) => {
                        this.PreviewGrid.Visibility = Visibility.Hidden;
                    };
                };

                // Add image to grid.
                grid.Children.Add(image);

                // Go to next column at end of rows
                if (x == grid.ColumnDefinitions.Count)
                {
                    x = 0;
                    y++;
                    if (y >= 3)
                    {
                        // Sizing is off and images are automatically rotated?!?!
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            });
        }
    }
}
