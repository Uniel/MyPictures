using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MyPicturesUWP.Storage
{
    class MediaData
    {
        public int ID { get; }
        public string Name { get; }
        public string Path { get; }
        public string Server { get; }
        public string Thumbnail { get; }
        public string CreatedAt { get; }
        public string UpdatedAt { get; }

        public MediaData(SQLiteDataReader reader)
        {
            this.ID = int.Parse(reader["id"].ToString());
            this.Name = reader["name"] as string;
            this.Path = reader["path"] as string;
            this.Server = reader["server"] as string;
            this.Thumbnail = reader["thumbnail"] as string;
            this.CreatedAt = reader["created_at"] as string;
            this.UpdatedAt = reader["updated_at"] as string;
        }
    }
}
