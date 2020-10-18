using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using ClientControllerApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;

namespace ClientControllerApp
{
    class CustomDataTableConverter : DataTableConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }

    public class SongSet : List<Song>
    {
        public string FirstCharacter { get; set; }
       
        public SongSet(string firstChar)
        {
            this.FirstCharacter = firstChar;
        }
    }

    public class Song 
    {
        public string SongTitle { get; set; }
        public Song(string title)
        {
            this.SongTitle = title;
        }
    }

    class PlayerPageVM
    {
        public static List<SongSet> ListOfSongsFromServer { get; set; } = new List<SongSet>();

        public PlayerPageVM()
        {
            var responseWithSongs = GetValueFromMsg(GetSongListFromServer());
            DeserializeDictIntoSimpleSong(responseWithSongs);
        }

        public void DeserializeDictIntoSimpleSong(Dictionary<string,List<string>> songList)
        {
            foreach(var item in songList)
            {
                SongSet tmpSet = new SongSet(item.Key);
                foreach(var song in item.Value)
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
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(msg, new CustomDataTableConverter());

        }

    }
}
