using System;
using System.Collections.Generic;
using System.Text;

namespace ClientControllerApp
{
    class MusicPlayer
    {

        public static void PlayChoosenSong(Song song)
        {
            OrderSender.PlaySong(song.SongTitle);
        }

    }
}
