using System;
using MyPictures.Files;
using MyPictures.Storage;
using MyPictures.Servers;
using System.Collections.Generic;

namespace MyPictures
{
    class Library
    {
        protected Database database;
        protected List<IServer> servers = new List<IServer>();

        protected List<string> paths = new List<string>();
        protected List<GenericMedia> media = new List<GenericMedia>();

        public void Initialize()
        {
            // Initialize local server.
            IServer server = new LocalServer(@"C:\Users\Andreasf98\Pictures\Steam");
            this.servers.Add(server);

            // TODO: Connect to external servers.s

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
            // Loop though the server connections.
            this.servers.ForEach(server => {
                // Add the server media paths to library.
                this.paths.AddRange(server.GetMediaPaths());

                // Add the image generics to library.
                this.media.AddRange(server.GetMediaGenerics());
            });

            // TODO: Load in paths from database and compare.
        }
    }
}
