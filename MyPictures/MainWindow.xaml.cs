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

            // Instansiate grids
            Grid ImagePane = this.FindName("ImagePane") as Grid;
            Grid AlbumPane = this.FindName("AlbumPane") as Grid;
            Grid SettingsPane = this.FindName("SettingsPane") as Grid;

            // Build menu bar
            ((Grid)this.FindName("PhotosButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Visible;
                this.AlbumPane.Visibility = Visibility.Hidden;
            };
            ((Grid)this.FindName("AlbumsButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Hidden;
                this.AlbumPane.Visibility = Visibility.Visible;
            };

            // Find picture grid element.
            Grid ImagesGrid = this.FindName("ImageGrid") as Grid;
            int x = 0, y = 0;

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
                ImagesGrid.Children.Add(image);

                // Go to next column at end of rows
                if (x == ImagesGrid.ColumnDefinitions.Count)
                {
                    x = 0;
                    y++;
                    if (y >= ImagesGrid.RowDefinitions.Count)
                    {
                        ImagesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            });

            // List albums in the albums section
            Grid AlbumGrid = this.FindName("AlbumGrid") as Grid;

            // Reset row/col variables
            x = 0; y = 0;

            // Get the albums from the library
            library.GetAlbums().ForEach(albumpath => {
                // Generate a label for each found directory
                Label txt = new Label();
                txt.Content = albumpath;

                // Insert label into album grid
                Grid.SetColumn(txt, x++);
                Grid.SetRow(txt, y);
                AlbumGrid.Children.Add(txt);

                // Line break check
                if (x == AlbumGrid.ColumnDefinitions.Count)
                {
                    x = 0;
                    y++;
                    if (y >= AlbumGrid.RowDefinitions.Count)
                    {
                        AlbumGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            });
        }
    }
}


