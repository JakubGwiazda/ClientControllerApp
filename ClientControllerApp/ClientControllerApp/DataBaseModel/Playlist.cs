using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace ClientControllerApp
{
    public class Playlist: List<Song>
    {
        [PrimaryKey,AutoIncrement,Unique]
        public int ID { get; set; }
        public string PlaylistName { get; set; }

    }
}
