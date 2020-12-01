using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ClientControllerApp
{
    public class Songs
    {
        [PrimaryKey,AutoIncrement,Unique]
        public int ID { get; set; }
        [ForeignKey(typeof(Playlist))]
        public int PlaylistId { get; set; }
        public string SongTitle { get; set; }
    }
}
