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
            Grid SelectedAlbumPane = this.FindName("SelectedAlbumPane") as Grid;

            // Build menu bar
            ((Grid)this.FindName("PhotosButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Visible;
                this.AlbumPane.Visibility = Visibility.Hidden;
                this.SettingsPane.Visibility = Visibility.Hidden;
                this.SelectedAlbumPane.Visibility = Visibility.Hidden;
            };
            ((Grid)this.FindName("AlbumsButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Hidden;
                this.AlbumPane.Visibility = Visibility.Visible;
                this.SettingsPane.Visibility = Visibility.Hidden;
                this.SelectedAlbumPane.Visibility = Visibility.Hidden;
            };
            ((Grid)this.FindName("SettingsButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Hidden;
                this.AlbumPane.Visibility = Visibility.Hidden;
                this.SettingsPane.Visibility = Visibility.Visible;
                this.SelectedAlbumPane.Visibility = Visibility.Hidden;
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
            Grid AlbumImageGrid = this.FindName("AlbumImageGrid") as Grid;
            // Reset row/col variables
            x = 0; y = 0;

            // Get the albums from the library
            library.GetAlbums().ForEach(albumpath => {
                // Generate a label for each found directory
                Button txt = new Button();
                txt.Content = albumpath.ToString();
                System.Console.WriteLine(txt.Content);

                // Insert label into album grid
                Grid.SetColumn(txt, x++);
                Grid.SetRow(txt, y);

                // Add album selection event handler.
                txt.PreviewMouseDown += (s, e) => {
                    int SelectedAlbumX = 0, SelectedAlbumY = 0;

                    AlbumImageGrid.Children.Clear();
                    // Hide preview when clicking outside image area.
                    library.GetLibraryAlbums(((Button)s).Content.ToString()).ForEach(generic => {
                        // Create new extended image source.
                        ExtendedSource image = new ExtendedSource(generic);

                        // Set image x and y position.
                        image.SetValue(Grid.RowProperty, SelectedAlbumY);
                        image.SetValue(Grid.ColumnProperty, SelectedAlbumX++);

                        // Add image mouse click event handler.
                        image.MouseDown += (si, ei) => {
                            // Enable preview and set full image source.
                            this.PreviewGrid.Visibility = Visibility.Visible;
                            this.Preview.Source = ((ExtendedSource)si).Media.LoadSource();

                            // Hide preview when clicking outside image area.
                            this.PreviewGrid.MouseDown += (sender, exception) => {
                                this.PreviewGrid.Visibility = Visibility.Hidden;
                            };
                        };

                        // Add image to grid.
                        AlbumImageGrid.Children.Add(image);

                        // Go to next column at end of rows
                        if (SelectedAlbumX == ImagesGrid.ColumnDefinitions.Count)
                        {
                            SelectedAlbumX = 0;
                            SelectedAlbumY++;
                            if (SelectedAlbumY >= ImagesGrid.RowDefinitions.Count)
                            {
                                AlbumImageGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            }
                        }
                    });
                    // AlbumImageGrid

                    // Hide other panes
                    this.ImagePane.Visibility = Visibility.Hidden;
                    this.AlbumPane.Visibility = Visibility.Hidden;
                    this.SettingsPane.Visibility = Visibility.Hidden;
                    this.SelectedAlbumPane.Visibility = Visibility.Visible;


                    System.Console.WriteLine(((Button)s).Content.ToString());
                    
                };

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

            // List albums in the albums section
            Grid SettingsGrid = this.FindName("SettingsGrid") as Grid;
            x = 0; y = 0;

            // Get the albums from the library
            library.GetAlbums().ForEach(albumpath => {
                // Generate a label for each found directory
                Label txt = new Label();
                txt.Content = albumpath;

                // Insert label into album grid
                Grid.SetColumn(txt, 0);
                Grid.SetRow(txt, y++);
                SettingsGrid.Children.Add(txt);

                // Line break check
                if (y >= SettingsGrid.RowDefinitions.Count)
                {
                    SettingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }
            });
        }
    }
}


