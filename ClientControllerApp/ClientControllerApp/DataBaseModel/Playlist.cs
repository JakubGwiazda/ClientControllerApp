using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace ClientControllerApp
{
    [Table("Playlist")]
    public class Playlist//: List<Song>
    {
        [PrimaryKey,AutoIncrement,Unique,Column("ID")]
        public int ID { get; set; }
        [Column("Playlist_Name")]
        public string PlaylistName { get; set; }

    }
}
