using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;
using System.Threading;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Timers;
namespace ClientControllerApp
{

    class PlayerPageVM
    {
        public static List<SongSet> ListOfSongsFromServer { get; set; } = new List<SongSet>();
      
        public PlayerPageVM()
        {
            var responseWithSongs = GetValueFromMsg(GetSongListFromServer());
            DeserializeDictIntoSimpleSong(responseWithSongs);
        }

        public void DeserializeDictIntoSimpleSong(Dictionary<string, List<string>> songList)
        {
            foreach (var item in songList)
            {
                SongSet tmpSet = new SongSet(item.Key);
                foreach (var song in item.Value)
                {
                    tmpSet.Add(new Song(song));
                }
                ListOfSongsFromServer.Add(tmpSet);
            }
        }
        public string GetSongListFromServer()
        {
            OrderSender.GetServerSoundList();
            return MessageReceiver.GetResponseFromServer();
        }

        public Dictionary<string, List<string>> GetValueFromMsg(string msg)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(msg);
        }
       
    }
}
