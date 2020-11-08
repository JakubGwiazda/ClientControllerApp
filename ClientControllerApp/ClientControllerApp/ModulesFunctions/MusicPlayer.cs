using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
namespace ClientControllerApp
{

    public class SongData
    {
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public SongData() { }
        
    }
    class MusicPlayer
    {

        public static void PlayChoosenSong(Song song)
        {
            OrderSender.PlaySong(song.SongTitle);
        }

        public static void GetSongDataFromServer()
        {
            Thread getData = new Thread(() => {
            var songTimes = MessageReceiver.GetResponseFromServer();
            SongData times = JsonConvert.DeserializeObject<SongData>(songTimes);
               
            });
            getData.Start();
        }

    }
}
