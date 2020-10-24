using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using ClientControllerApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;

namespace ClientControllerApp
{

    class PlayerPageVM : INotifyPropertyChanged
    {
        public static List<SongSet> ListOfSongsFromServer { get; set; } = new List<SongSet>();
        private string currentPlayingSong;
        private string musicButtonText = "Play";
        
        public PlayerPageVM()
        {
            var responseWithSongs = GetValueFromMsg(GetSongListFromServer());
            DeserializeDictIntoSimpleSong(responseWithSongs);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string CurrentAvailableDisplayOption
        {
            get
            {
                return musicButtonText;
            }
            set
            {
                musicButtonText = value;
                OnPropertyChanged();
            }
        }
        public string CurrentPlayingSong
        {
            get
            {
                return currentPlayingSong;
            }
            set
            {
                currentPlayingSong = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeButtonOption => new Command(() => { 
        if (CurrentAvailableDisplayOption.Equals("Play") && CurrentPlayingSong == null)
            {
                OrderSender.PlaySong(CurrentPlayingSong);
            }
            else if(CurrentAvailableDisplayOption.Equals("Stop"))
            {
                OrderSender.StopPlayingSong();
            }
            else
            {
                OrderSender.PlaySongAgain();
            }
            CurrentAvailableDisplayOption = CurrentAvailableDisplayOption.Equals("Play") ? "Stop" : "Play";

        });

        public void StopPlayingMusic()
        {
            OrderSender.StopPlayingSong();
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
        public  void PlayChoosenSong(Song song)
        {
            OrderSender.PlaySong(song.SongTitle);
        }
     

    }
}
