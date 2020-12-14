using System;
using System.Collections.Generic;
using System.Text;

namespace ClientControllerApp
{
    public class DBSongsModel
    {
        public int ID { get; set; }
        public int Playlist_ID { get; set; }
        public string SongTitle { get; set; }
        
      /*  public DBSongsModel(int id, int playlistID, string songTitle)
        {
            this.ID = id;
            this.Playlist_ID = playlistID;
            this.SongTitle = songTitle;
        }*/
    }
}
