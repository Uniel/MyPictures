using System;
using System.IO;
using System.Windows;
using MyPictures.Files;
using MyPictures.Servers;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace MyPictures
{
    public partial class MainWindow : Window
    {
        Library library;

        public MainWindow()
        {
            // Initialize UI.
            InitializeComponent();

            // Create and initialize library.
            this.library = new Library();
            this.library.Initialize();

            // Instansiate panes (grid containers)
            Grid ImagePane = this.FindName("ImagePane") as Grid;
            Grid AlbumPane = this.FindName("AlbumPane") as Grid;
            Grid SettingsPane = this.FindName("SettingsPane") as Grid;
            Grid SelectedAlbumPane = this.FindName("SelectedAlbumPane") as Grid;

            // Instansiate grids contained in panes
            Grid ImagesGrid = this.FindName("ImageGrid") as Grid;
            Grid AlbumGrid = this.FindName("AlbumGrid") as Grid;
            Grid AlbumImageGrid = this.FindName("AlbumImageGrid") as Grid;
            Grid SettingsGrid = this.FindName("SettingsGrid") as Grid;

            // Build menu bar - Photos overview
            ((Grid)this.FindName("PhotosButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Visible;
                this.AlbumPane.Visibility = Visibility.Hidden;
                this.SettingsPane.Visibility = Visibility.Hidden;
                this.SelectedAlbumPane.Visibility = Visibility.Hidden;
                PopulatePhotos();
            };

            // Albums
            ((Grid)this.FindName("AlbumsButton")).MouseDown += (s, e) => {
                this.ImagePane.Visibility = Visibility.Hidden;
                this.AlbumPane.Visibility = Visibility.Visible;
                this.SettingsPane.Visibility = Visibility.Hidden;
                this.SelectedAlbumPane.Visibility = Visibility.Hidden;
                PopulateAlbums();
            };

            // Settings list
            ((Grid)this.FindName("SettingsButton")).MouseDown += (s, e) => {
                SettingsGrid.Children.Clear();
                this.ImagePane.Visibility = Visibility.Hidden;
                this.AlbumPane.Visibility = Visibility.Hidden;
                this.SettingsPane.Visibility = Visibility.Visible;
                this.SelectedAlbumPane.Visibility = Visibility.Hidden;
                PopulateSettings();
            };

            // Generate content for photo overview grid
            PopulatePhotos();
        }

        private void PopulatePhotos()
        {
            // Clear gird.
            this.ImageGrid.Children.Clear();

            // Loop through the whole library.
            int x = 0; int y = 0;
            this.library.GetLibrary().ForEach(generic => {
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
                this.ImageGrid.Children.Add(image);

                // Go to next column at end of rows
                if (x == this.ImageGrid.ColumnDefinitions.Count)
                {
                    x = 0; y++;
                    if (y >= this.ImageGrid.RowDefinitions.Count)
                    {
                        this.ImageGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            });
        }

        private void PopulateAlbums()
        {
            int x = 0; int y = 0;
            this.library.GetAlbums().ForEach(albumpath => {
                // Generate a label for each found directory
                Button txt = new Button();
                txt.Content = albumpath.ToString();

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
                        if (SelectedAlbumX == ImageGrid.ColumnDefinitions.Count)
                        {
                            SelectedAlbumX = 0;
                            SelectedAlbumY++;
                            if (SelectedAlbumY >= ImageGrid.RowDefinitions.Count)
                            {
                                AlbumImageGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            }
                        }
                    });

                    // Hide other panes
                    this.ImagePane.Visibility = Visibility.Hidden;
                    this.AlbumPane.Visibility = Visibility.Hidden;
                    this.SettingsPane.Visibility = Visibility.Hidden;
                    this.SelectedAlbumPane.Visibility = Visibility.Visible;
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
        }

        private void PopulateSettings()
        {
            SettingsGrid.Children.Clear();
            int y = 0;

            // Set the settings
            this.GetSettings().ForEach(Setting => {
                // Generate a label for setting name and description
                Label name = new Label();
                Label description = new Label();
                name.Content = Setting.Item1;
                description.Content = Setting.Item2;

                // Insert Name
                Grid.SetColumn(name, 0);
                Grid.SetRow(name, y);
                SettingsGrid.Children.Add(name);

                // Insert Description
                Grid.SetColumn(description, 1);
                Grid.SetRow(description, y);
                SettingsGrid.Children.Add(description);

                // Generate and insert action button
                Button action = new Button();
                action.Content = Setting.Item3.ToString();

                // Insert label into album grid
                Grid.SetColumn(action, 2);
                Grid.SetRow(action, y++);

                // Add album selection event handler.
                action.PreviewMouseDown += (s, e) =>
                {
                    // Get the setting of the first column in the same row
                    var setting = SettingsGrid.Children.Cast<UIElement>()
                        .Where(col => Grid.GetColumn(col) == 0)
                        .Where(row => Grid.GetRow(row) == Grid.GetRow(action));

                    Label txt = (Label)setting.ElementAt(0);

                    // Clear settings page
                    SettingsGrid.Children.Clear();
                    
                    // Call library for setting edit
                    library.ToggleProvider(txt.Content.ToString());

                    // Refresh UI
                    PopulateSettings();
                };

                SettingsGrid.Children.Add(action);

                // Line break check
                if (y >= SettingsGrid.RowDefinitions.Count)
                {
                    SettingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }
            });
        }
        
        internal List<Tuple<string, string, string>> GetSettings()
        {
            // Return value -- 3 strings: Name, Description and Action
            List<Tuple<string, string, string>> Settings = new List<Tuple<string, string, string>>();

            // Locate state of Google Drive
            this.library.providers.ForEach(provider => {

                // For each provider add their name and the functionality of the button to the list
                Settings.Add(new Tuple<string, string, string>(
                    provider.Name,
                    "Manage your connection to " + provider.Name + " cloud storage",
                    provider.IsConnected() ? "Disconnect" : "Connect"
                ));
            });

            return Settings;
        }

        private void DropPanel_Drop(object sender, DragEventArgs e)
        {
            // Skip if no file data is present.
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            // Get array of file paths and convert to list.
            string[] paths = (string[]) e.Data.GetData(DataFormats.FileDrop);
            List<string> files = new List<string>(paths);

            // Loop through the dropped files.
            files.ForEach(file => {
                // Get current local server for library.
                LocalServer server = this.library.local;

                // Generate name for dropped file.
                int index = file.LastIndexOf('\\');
                string name = file.Substring(index + 1, file.Length - index - 1);

                // Get media stream and upload to local server.
                Stream stream = server.GetMediaStream(file);
                server.UploadMediaStream(server.GetDirectory() + "\\" + name, stream);

                // Refresh the library.
                this.library.Initialize();
            });
        }
    }

}


