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
        protected List<IServer> servers = new List<IServer>();

        protected List<string> paths = new List<string>();
        protected List<GenericMedia> media = new List<GenericMedia>();

        protected Database database;
        protected Dictionary<int, string> dbPaths = new Dictionary<int, string>();

        public void Initialize()
        {
            // TODO: Load user server config.

            // Initialize local server.
            LocalServer local = new LocalServer(@"C:\Users\Andreas\Pictures\dogs");
            this.servers.Add(local);

            // Create thumbnails directory on local server. 
            local.CreateThumbnailsDirectory();

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
            while (reader.Read())
            {
                // Add the id as key and path as value to list.
                this.dbPaths.Add(int.Parse(reader["id"].ToString()), reader["path"].ToString());
            }

            // Find paths not already in database.
            this.media.FindAll(media => ! this.dbPaths.ContainsValue(media.GetPath()))
                .ForEach(media => {
                    // Insert media in database.
                    this.database.InsertMedia(media);
                });

            // Find deleted paths still in database.
            this.dbPaths.Where(item => ! this.paths.Contains(item.Value)).ToList()
                .ForEach(pair => {
                    // Remove path from database.
                    this.database.DeleteID(pair.Key);
                });
        }
    }
}
