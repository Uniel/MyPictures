using MyPictures.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyPictures.Storage
{
    class Database
    {
        protected SQLiteConnection connection;

        public void Connect()
        {
            // Check for databse existance
            if (! File.Exists("database.db"))
            {
                SQLiteConnection.CreateFile("database.db");
            } 

            // Open database
            this.connection = new SQLiteConnection("Data Source=database.db;Version=3;");
            this.connection.Open();
            this.Setup();
        }

        public void Setup()
        {
            string sql = "CREATE TABLE IF NOT EXISTS photos(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "name VARCHAR(255) NOT NULL UNIQUE," +
                "path VARCHAR(255)," +
                "thumbName VARCHAR(255)," +
                "thumbDir VARCHAR(255)," +
                "created_at TEXT NOT NULL," +
                "updated_at TEXT NOT NULL" +
            ")";

            new SQLiteCommand(sql, this.connection).ExecuteNonQuery();
        }

        public void InsertMedia(GenericMedia media)
        {
            if (this.HasMedia(media.GetName())) return;

            // Create command object
            SQLiteCommand command = new SQLiteCommand(null, this.connection);

            // Insert sql command into command object
            command.CommandText = "INSERT INTO photos (name, path, thumbName, thumbDir, created_at, updated_at) VALUES (@name, @path, @thumbName, @thumbDir, @created_at, @updated_at)";
            command.Parameters.Add(new SQLiteParameter("@name", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@path", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@thumbName", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@thumbDir", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@created_at", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@updated_at", DbType.String, 255));

            command.Prepare();

            // Get correct format for date of picture
            String now = DateTime.Now.ToString("u");
            String rawDate = ((GenericImage)media).RetrieveMetadata().DateTaken;
            String date = rawDate == null ? now : DateTime.Parse(rawDate).ToString("u");

            command.Parameters[0].Value = media.GetName();
            command.Parameters[1].Value = media.GetPath();

            command.Parameters[2].Value = ("thumb_" + media.GetName());
            command.Parameters[3].Value = "TODO";
            command.Parameters[4].Value = date;
            command.Parameters[5].Value = now;

            command.ExecuteNonQuery();
        }
        
        public SQLiteDataReader RetrieveMedia(String path)
        {
            // Create command object
            SQLiteCommand command = new SQLiteCommand(null, this.connection);

            command.CommandText = "SELECT * FROM photos WHERE name = @name";
            command.Parameters.Add(new SQLiteParameter("@name", DbType.String, 255));

            command.Prepare();

            command.Parameters[0].Value = path;

            return command.ExecuteReader();
        }

        public Boolean HasMedia(String path)
        {
            return this.RetrieveMedia(path).HasRows;
        }

    }
}
