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
                "name VARCHAR(255) NOT NULL," +
                "thumbnail VARCHAR(255)," +
                "created_at TEXT NOT NULL," +
                "updated_at TEXT NOT NULL" +
            ")";

            new SQLiteCommand(sql, this.connection).ExecuteNonQuery();
        }

        public void InsertMedia(GenericMedia media)
        {
            // Create command object
            SQLiteCommand command = new SQLiteCommand(null, this.connection);

            // Insert sql command into command object
            command.CommandText = "INSERT INTO photos (name, thumbnail, created_at, updated_at) VALUES (@name, @thumbnail, @created_at, @updated_at)";
            command.Parameters.Add(new SQLiteParameter("@name", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@thumbnail", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@created_at", DbType.String, 255));
            command.Parameters.Add(new SQLiteParameter("@updated_at", DbType.String, 255));

            command.Prepare();

            command.Parameters[0].Value = media.GetPath();
            command.Parameters[1].Value = "TODO";

            String now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String rawDate = ((GenericImage)media).RetrieveMetadata().DateTaken;
            String date = rawDate == null ? now : DateTime.Parse(rawDate).ToString("yyyy-MM-dd HH:mm:ss");

            command.Parameters[2].Value = date;
            command.Parameters[3].Value = now;

            command.ExecuteNonQuery();
        }

    }
}
