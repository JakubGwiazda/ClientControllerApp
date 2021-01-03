using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ClientControllerApp
{
    [Table("Songs")]
    public class Songs
    {
        [PrimaryKey,AutoIncrement,Unique,Column("ID")]
        public int ID { get; set; }
     //   [ForeignKey(typeof(Playlist))]
        [Column("Playlist_id")]
        public int PlaylistId { get; set; }
        [Column("Song_Title")]
        public string SongTitle { get; set; }
    }
}
