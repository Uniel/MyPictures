using System.Data.SQLite;

namespace MyPictures.Storage
{
    class MediaData
    {
        public int ID { get; }
        public string Name { get; }
        public string Path { get; }
        public string Server { get; }
        public int Encrypted { get; set; }
        public string Thumbnail { get; set; }
        public string CreatedAt { get; }
        public string UpdatedAt { get; }

        public MediaData(SQLiteDataReader reader)
        {
            this.ID = int.Parse(reader["id"].ToString());
            this.Name = reader["name"] as string;
            this.Path = reader["path"] as string;
            this.Server = reader["server"] as string;
            this.Encrypted = int.Parse(reader["encrypted"].ToString());
            this.Thumbnail = reader["thumbnail"] as string;
            this.CreatedAt = reader["created_at"] as string;
            this.UpdatedAt = reader["updated_at"] as string;
        }
    }
}
