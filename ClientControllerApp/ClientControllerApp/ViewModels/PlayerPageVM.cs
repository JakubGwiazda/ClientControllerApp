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
using System.Threading;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Timers;
namespace ClientControllerApp
{

    class PlayerPageVM : INotifyPropertyChanged
    {
        public static List<SongSet> ListOfSongsFromServer { get; set; } = new List<SongSet>();
        private string currentPlayingSong;
        private string musicButtonText = "Play";
        private string currentSongTime;
        private string songTimeBeforeChange;
        private string songDurationTime;
        private int currentSongPosition;
        private int currentSongMaxDurationInSeconds;
        SongData times;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token;
        public PlayerPageVM()
        {
            var responseWithSongs = GetValueFromMsg(GetSongListFromServer());
            DeserializeDictIntoSimpleSong(responseWithSongs);
            CurrentSongMaxDurationInSeconds = 100;
            
            token = cancellationTokenSource.Token;

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
        public string CurrentSongTime
        {
            get
            {
                return currentSongTime;
            }
            set
            {
                currentSongTime = value;
                OnPropertyChanged();
            }
        }
        public string SongDurationTime
        {
            get
            {
                return songDurationTime;
            }
            set
            {
                songDurationTime = value;
                OnPropertyChanged();
            }
        }
        public int CurrentSongPosition
        {
            get { return currentSongPosition; }
            set
            {
                currentSongPosition = value;
                OnPropertyChanged();
            }

        }
        public int CurrentSongMaxDurationInSeconds { get { return currentSongMaxDurationInSeconds; } set { currentSongMaxDurationInSeconds = value; OnPropertyChanged(); } }
     
        public ICommand ChangeButtonOption => new Command(() =>
        {
            if (CurrentAvailableDisplayOption.Equals("Play") && CurrentPlayingSong == null)
            {
                OrderSender.PlaySong(CurrentPlayingSong);
            }
            else if (CurrentAvailableDisplayOption.Equals("Stop"))
            {
                OrderSender.StopPlayingSong();
            }
            else
            {
                OrderSender.PlaySongAgain();
            }
            CurrentAvailableDisplayOption = CurrentAvailableDisplayOption.Equals("Play") ? "Stop" : "Play";

        });
        public ICommand Forward => new Command(() =>
        {
            OrderSender.Forward();
        });
        public ICommand Backward => new Command(() =>
        {
            OrderSender.Backward();
        });

         public ICommand StopPlayingOnDragSlider => new Command (() => {
             OrderSender.StopPlayingSong();
             songTimeBeforeChange = CurrentSongTime;
          });


        public ICommand StartPlayingOnDropSlider => new Command(() =>
        {
            OrderSender.PlaySongFromSpecificPoint(ChangeSongTime());
        });

        public void StopPlayingMusic()
        {
            OrderSender.StopPlayingSong();
        }
        public void StartPlayingMusic()
        {
            OrderSender.PlaySongAgain();
        }
        private int ChangeSongTime()
        {
            DateTime dateTime = DateTime.ParseExact(songTimeBeforeChange, "mm:ss",CultureInfo.InvariantCulture);
            int startSeconds = dateTime.Minute * 60 + dateTime.Second;
            DateTime dateTimeAfterChanges = DateTime.ParseExact(CurrentSongTime, "mm:ss", CultureInfo.InvariantCulture);
            int endSeconds = dateTimeAfterChanges.Minute * 60 + dateTimeAfterChanges.Second;
            return endSeconds - startSeconds;
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
        public void PlayChoosenSong(Song song)
        {
            
            CurrentSongPosition = 0;
            CurrentSongTime = "00:00";
            OrderSender.PlaySong(song.SongTitle);
            times = new SongData();
            Action update = UpdateTime;
            Thread.Sleep(500);// wymagane do odpowiedniego ustawienia czasów otrzymanych z serwera oraz czasu na wykonanie na nim operacji
            UpdateSongStatus(update, 0.5,token);

        }
    
        public  void UpdateTime()
        {
            var songTimes = MessageReceiver.GetResponseFromServer();
            times = JsonConvert.DeserializeObject<SongData>(songTimes);
            if (times.IsSongDuration)
            {
                TimeSpan time = TimeSpan.FromSeconds(GetSongTimeInSeconds(times));
                SongDurationTime = time.ToString("mm':'ss");
                CurrentSongMaxDurationInSeconds = GetSongTimeInSeconds(times);
            }
            else
            {
                CurrentSongPosition = GetSongTimeInSeconds(times);

            }
        }
        private void UpdateSongStatus(Action action, double seconds,CancellationToken token)
        {
            if (action == null)
                return;
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                }
            });
        }
    
        public int GetSongTimeInSeconds(SongData songTimes)
        {
            return songTimes.Minutes * 60 + songTimes.Seconds;
        }

    }
}
