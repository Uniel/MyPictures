using System;
using System.Linq;
using MyPictures.Files;
using MyPictures.Servers;
using MyPictures.Storage;
using System.Data.SQLite;
using System.Collections.Generic;

namespace MyPictures
{
    class Library
    {
        protected LocalServer local;
        protected List<IServer> servers = new List<IServer>();

        protected List<string> paths = new List<string>();
        protected List<GenericMedia> media = new List<GenericMedia>();

        protected Database database;
        protected Dictionary<int, string> dbPaths = new Dictionary<int, string>();

        public void Initialize()
        {
            // TODO: Load user server config.

            // Initialize local server.
            this.local = new LocalServer("default", @"C:\Users\Andreas\Pictures\dogs");
            this.servers.Add(this.local);

            // Create thumbnails directory on local server. 
            this.local.CreateThumbnailsDirectory();

            // TODO: Connect to external servers.

            // Create database and connect.
            this.database = new Database();
            this.database.Connect();

            // Load the full library.
            this.LoadLibrary();
        }

        public List<GenericMedia> GetLibrary()
        {
            return this.media;
        }

        public void LoadLibrary()
        {
            // Reset media lists.
            this.paths.Clear();
            this.media.Clear();

            // Loop though the server connections.
            this.servers.ForEach(server => {
                // Add the server media paths to library.
                this.paths.AddRange(server.GetMediaPaths());

                // Add the image generics to library.
                this.media.AddRange(server.GetMediaGenerics());
            });
            
            // Load and clean the database.
            this.LoadDatabase();
        }

        protected void LoadDatabase()
        {
            // Get database media reader and prepare paths key-value pair.
            SQLiteDataReader reader = this.database.RetrieveMedia();

            // Keep reading while data is available.
            while (reader.Read()) {
                // Create new media data instance.
                MediaData data = new MediaData(reader);

                // Find deleted paths still in database.
                if (! this.paths.Contains(data.Path)) {
                    // Remove path from database.
                    this.database.DeleteID(data.ID);
                    continue;
                }

                // Bind data instance to generic media objct.
                this.media.Find(source => source.GetPath() == data.Path).Data = data;

                // Add the id as key and path as value to list.
                this.dbPaths.Add(data.ID, data.Path);
            }

            // Create new thumbnail generator for local server.
            Thumbnailer generator = new Thumbnailer(this.local);

            // Generate thumbnail for all media items.
            this.media.ForEach(media => generator.Process(media));

            // Add missing paths to database.
            this.media.FindAll(media => ! this.dbPaths.ContainsValue(media.GetPath()))
                .ForEach(media => {
                    // Insert media into database.
                    this.database.InsertMedia(media);
                });
        }
    }
}
