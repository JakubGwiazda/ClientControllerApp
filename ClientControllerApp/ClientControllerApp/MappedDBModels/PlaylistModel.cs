using System;
using System.Collections.Generic;
using System.Text;

namespace ClientControllerApp
{
     public   class PlaylistModel:List<DBSongsModel>
    {
        public int ID { get; set; }
        public  string PlaylistTitle { get; set; }
     
     /*   public PlaylistModel(int id, string playlistTitle)
        {
            this.ID = id;
            this.PlaylistTitle = playlistTitle;
        }*/

    }
}
