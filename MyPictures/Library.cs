using System;
using MyPictures.Auth;
using MyPictures.Files;
using MyPictures.Servers;
using MyPictures.Storage;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Threading;
using MyPictures.Encryption;

namespace MyPictures
{
    class Library
    {
        public LocalServer local;
        protected List<Server> servers = new List<Server>();
        protected List<OAuthProvider> providers = new List<OAuthProvider>();

        protected List<string> paths = new List<string>();
        protected List<GenericMedia> media = new List<GenericMedia>();
        protected List<string> albums = new List<string>();

        protected Database database;
        protected Dictionary<int, string> dbPaths = new Dictionary<int, string>();

        public void Initialize()
        {
            // Create encryption module.
            EncryptionManager encryptor = new EncryptionManager();

            // Load user server config.
            string path = Properties.Settings.Default.Path;
            path = Environment.ExpandEnvironmentVariables(path);

            // Initialize local server.
            this.local = new LocalServer("default", path);
            this.servers.Add(this.local);

            // Create thumbnails directory on local server. 
            this.local.CreateThumbnailsDirectory();

            // Load external providers and servers.
            this.LoadProviders();

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
            this.servers.ForEach(server =>
            {
                // Add the server media paths to library.
                this.paths.AddRange(server.GetMediaPaths());

                // Add the image generics to library.
                this.media.AddRange(server.GetMediaGenerics());
            });

            // Load and clean the database.
            this.LoadDatabase();
        }

        public List<string> GetAlbums()
        {
            this.albums = this.local.GetAlbumPaths();
            return this.albums;
        }

        protected void LoadProviders()
        {
            // Load Google Provider.
            string GoogleSettings = Properties.Settings.Default.GoogleProvider;
            GoogleProvider GoogleInstance = new GoogleProvider(GoogleSettings);
            this.providers.Add(GoogleInstance);

            // Check if Google Provider is connected.
            if (GoogleInstance.IsConnected())
            {
                // Create and add new Google Drive server instance to list.
                this.servers.Add(new GoogleDriveServer("google", "/", GoogleInstance));
            } else {
                // TODO: Redirect for now until UI feature..
                GoogleInstance.Redirect();
            }
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

            // Add missing paths to database.
            this.media.FindAll(media => !this.dbPaths.ContainsValue(media.GetPath()))
                .ForEach(media => {
                    // Insert media into database.
                    this.database.InsertMedia(media);
                    media.Data = new MediaData(this.database.RetrieveMedia(media));
                });

            // Create new thumbnail generator for local server.
            Thumbnailer generator = new Thumbnailer(this.local);
            Thumbnailer.AsyncMethodCaller caller = new Thumbnailer.AsyncMethodCaller(generator.Process);

            // Generate thumbnail for all media items.
            this.media.ForEach(media => {
                IAsyncResult created = caller.BeginInvoke(media, null, null);
                created.AsyncWaitHandle.WaitOne();
                Boolean returnValue = caller.EndInvoke(created);
                if (! returnValue)
                {
                    Console.WriteLine(media.Data.Thumbnail);
                    this.database.UpdateMedia(media.Data);
                    media.Data = new MediaData(this.database.RetrieveMedia(media));
                }
            });
        }

    }
}
