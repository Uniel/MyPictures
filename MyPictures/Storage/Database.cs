﻿using System;
using System.IO;
using System.Data;
using MyPictures.Files;
using System.Data.SQLite;

namespace MyPictures.Storage
{
    class Database
    {
        protected SQLiteConnection connection;

        public void Connect()
        {
            // Check if database file does not exist.
            if (! File.Exists("database.db")) {
                // Create new data file.
                SQLiteConnection.CreateFile("database.db");
            } 

            // Open databaseconnection.
            this.connection = new SQLiteConnection("Data Source=database.db;Version=3;");
            this.connection.Open();

            // Continue table setup.
            this.Setup();
        }

        public void Disconnect()
        {
            this.connection.Close();
            GC.Collect();
        }

        public void Setup()
        {
            // Define base media table schema.
            string sql = "" +
            "CREATE TABLE IF NOT EXISTS media(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "name VARCHAR(255) NOT NULL," +
                "path VARCHAR(255) NOT NULL," +
                "server VARCHAR(255) NOT NULL," +
                "thumbnail VARCHAR(255)," +
                "encrypted INTEGER DEFAULT 0," +
                "created_at TEXT NOT NULL," +
                "updated_at TEXT NOT NULL" +
            ")";

            // Execute table create query.
            new SQLiteCommand(sql, this.connection).ExecuteNonQuery();
        }

        public SQLiteDataReader RetrieveMedia()
        {
            // Prepare command object and query.
            SQLiteCommand cmd = new SQLiteCommand(null, this.connection);
            cmd.CommandText = "SELECT * FROM media";
            cmd.Prepare();

            // Execute the query.
            return cmd.ExecuteReader();
        }

        public SQLiteDataReader RetrieveMedia(GenericMedia media)
        {
            // Prepare command object and query.
            SQLiteCommand command = new SQLiteCommand(null, this.connection);
            command.CommandText = "SELECT * FROM media WHERE name = @name AND path = @path AND server = @server";
            command.Parameters.Add(new SQLiteParameter("@name", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@path", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@server", DbType.String, 255));
            command.Prepare();

            // Add data to prepared parameters.
            command.Parameters[0].Value = media.GetName();
            command.Parameters[1].Value = media.GetPath();
            command.Parameters[2].Value = media.Server.GetName();

            // Execute the query.
            return command.ExecuteReader();
        }

        public void UpdateMedia(MediaData data)
        {
            // Prepare command object and query.
            SQLiteCommand cmd = new SQLiteCommand(null, this.connection);
            cmd.CommandText = "UPDATE media SET name = @name, path = @path, server = @server, encrypted = @encrypted, thumbnail = @thumbnail, updated_at = @updated_at WHERE name = @Wname AND path = @Wpath AND server = @Wserver";
            cmd.Parameters.Add(new SQLiteParameter("@name", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@path", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@server", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@encrypted", DbType.Int32));
            cmd.Parameters.Add(new SQLiteParameter("@thumbnail", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@updated_at", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@Wname", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@Wpath", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@Wserver", DbType.String, 255));
            cmd.Prepare();

            // Add data to prepared parameters.
            cmd.Parameters[0].Value = data.Name;
            cmd.Parameters[1].Value = data.Path;
            cmd.Parameters[2].Value = data.Server;
            cmd.Parameters[3].Value = data.Encrypted;
            cmd.Parameters[4].Value = data.Thumbnail;
            cmd.Parameters[5].Value = DateTime.Now.ToString("u");
            cmd.Parameters[6].Value = data.Name;
            cmd.Parameters[7].Value = data.Path;
            cmd.Parameters[8].Value = data.Server;

            // Execute the query.
            cmd.ExecuteNonQuery();
        }
        
        public void InsertMedia(GenericMedia media)
        {
            // Prepare command object and query.
            SQLiteCommand cmd = new SQLiteCommand(null, this.connection);
            cmd.CommandText = "INSERT INTO media (name, path, server, encrypted, thumbnail, created_at, updated_at) VALUES (@name, @path, @server, @encrypted, @thumbnail, @created_at, @updated_at)";
            cmd.Parameters.Add(new SQLiteParameter("@name", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@path", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@server", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@encrypted", DbType.Int32));
            cmd.Parameters.Add(new SQLiteParameter("@thumbnail", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@created_at", DbType.String, 255));
            cmd.Parameters.Add(new SQLiteParameter("@updated_at", DbType.String, 255));
            cmd.Prepare();

            // Prepare current data and media creation date.
            string now = DateTime.Now.ToString("u");
            string created = media.RetrieveMetadata(media.LoadSource()).DateTaken;

            // Determine creation date to be used.
            string date = created == null ? now : DateTime.Parse(created).ToString("u");

            // Add data to prepared parameters.
            cmd.Parameters[0].Value = media.GetName();
            cmd.Parameters[1].Value = media.GetPath();
            cmd.Parameters[2].Value = media.Server.GetName();
            cmd.Parameters[3].Value = media.Data != null ? media.Data.Encrypted : 0;
            cmd.Parameters[4].Value = media.Thumbnail != null ? media.Thumbnail.GetPath() : null;
            cmd.Parameters[5].Value = date;
            cmd.Parameters[6].Value = now;

            // Execute the query.
            cmd.ExecuteNonQuery();
        }

        public void DeleteID(int id)
        {
            // Prepare command object and query.
            SQLiteCommand cmd = new SQLiteCommand(null, this.connection);
            cmd.CommandText = "DELETE FROM media WHERE id = @id";
            cmd.Parameters.Add(new SQLiteParameter("@id"));
            cmd.Prepare();

            // Set query id and execute.
            cmd.Parameters[0].Value = id;
            cmd.ExecuteNonQuery();
        }
    }
}
