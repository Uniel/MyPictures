using System.Windows;
using MyPictures.Files;
using System.Windows.Controls;

namespace MyPictures
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize UI.
            InitializeComponent();

            // Create library and Initialize.
            Library library = new Library();
            library.Initialize();

            // Build menu bar
            ((Grid)this.FindName("PhotosGrid")).MouseDown += (s, e) => {
                System.Environment.Exit(0);
            };

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
                    this.Preview.Source = ((ExtendedSource) s).Media.LoadSource();

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
                    if (y >= grid.RowDefinitions.Count)
                    {
                        // Sizing is off and images are automatically rotated?!?!
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            });
        }
    }
}


