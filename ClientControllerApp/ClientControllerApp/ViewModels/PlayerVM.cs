﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SQLite;
using System.Linq;
namespace ClientControllerApp
{
     public class PlayerVM:INotifyPropertyChanged
    {

        private string musicButtonText = "play25x25.png";

        private string currentPlayingSong;
        private string currentSongTime;
        private string songTimeBeforeChange;
        private int currentSongPosition;
        CancellationToken token;
        SongData times;
        private string songDurationTime;
        private int currentSongMaxDurationInSeconds;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private bool _addIsVisible = true;
        SQLiteAsyncConnection Database;
        public  bool IsSongFromPlaylist { get; set; }

        private PlayerVM()
        {
            CurrentSongMaxDurationInSeconds = 100;
            token = cancellationTokenSource.Token;
            Database = DatabaseInitializator.Database();
            this.SongEnding += TryToPlayNextSong;
        }
        public event EventHandler SongEnding;

        private void TryToPlayNextSong(object sender, EventArgs e)
        {
            CurrentSongTime = "00:00";
            PlaySongsFromPlaylist(PlayNextSongFromPlaylist(CurrentPlayingSong));
        }

        private void OnSongEndingReached(EventArgs e)
        {
            EventHandler handler = SongEnding;
            if(handler != null)
            {
                handler(this, e);
            }
        }
        private static PlayerVM _instance;
        public static PlayerVM Instance
        {
            get { return _instance ?? (_instance = new PlayerVM());}
        }

        public bool AddIsVisible
        {
            get
            {
                return _addIsVisible;
            }
            set
            {
                _addIsVisible = value;
                OnPropertyChanged();
            }
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
        public int CurrentSongPosition
        {
            get { return currentSongPosition; }
            set
            {
                currentSongPosition = value;
                OnPropertyChanged();
                if (CurrentSongPosition== CurrentSongMaxDurationInSeconds && IsSongFromPlaylist)
                {
                    PlaySongsFromPlaylist(PlayNextSongFromPlaylist(CurrentPlayingSong));
                   // OnSongEndingReached(EventArgs.Empty);
                }

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
        public int CurrentSongMaxDurationInSeconds { get { return currentSongMaxDurationInSeconds; } set { currentSongMaxDurationInSeconds = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
 
        public ICommand ChangeButtonOption => new Command(() =>
        {
            if (CurrentAvailableDisplayOption.Equals("play25x25.png") && CurrentPlayingSong == null)
            {
                OrderSender.PlaySong(CurrentPlayingSong);
            }
            else if (CurrentAvailableDisplayOption.Equals("pause.png"))
            {
                OrderSender.StopPlayingSong();
            }
            else
            {
                OrderSender.PlaySongAgain();
            }
            CurrentAvailableDisplayOption = CurrentAvailableDisplayOption.Equals("play25x25.png") ? "pause.png" : "play25x25.png";

        });

        public ICommand Forward => new Command(() =>
        {
            OrderSender.Forward();
        });
        public ICommand Backward => new Command(() =>
        {
            OrderSender.Backward();
        });
        public ICommand StopPlayingOnDragSlider => new Command(() => {
            OrderSender.StopPlayingSong();
            songTimeBeforeChange = CurrentSongTime;
        });

        public ICommand StartPlayingOnDropSlider => new Command(() =>
        {
            OrderSender.PlaySongFromSpecificPoint(ChangeSongTime());
        });
        public ICommand AddSongToPlaylist => new Command(()=>
        {
            MessagingCenter.Send<PlayerVM, int>(this, "SetActiveTab",1);
            PlaylistsVM.SongToAdd = CurrentPlayingSong;
            PlaylistsVM.AddSong = true;
      
        });
        private int ChangeSongTime()
        {
            DateTime dateTime = DateTime.ParseExact(songTimeBeforeChange, "mm:ss", CultureInfo.InvariantCulture);
            int startSeconds = dateTime.Minute * 60 + dateTime.Second;
            DateTime dateTimeAfterChanges = DateTime.ParseExact(CurrentSongTime, "mm:ss", CultureInfo.InvariantCulture);
            int endSeconds = dateTimeAfterChanges.Minute * 60 + dateTimeAfterChanges.Second;
            return endSeconds - startSeconds;
        }
        public void StartPlayingChoosenSong(string songTitle)
        {
            CurrentSongPosition = 0;
            CurrentSongTime = "00:00";
            OrderSender.PlaySong(songTitle);
            times = new SongData();
            Action update = UpdateTime;
            Thread.Sleep(500);// wymagane do odpowiedniego ustawienia czasów otrzymanych z serwera oraz czasu na wykonanie na nim operacji
            UpdateSongStatus(update, 0.5, token);
            
        }

        public void PlaySongsFromPlaylist(string choosenSongFromPlaylist)
        {
            IsSongFromPlaylist = true;
            if (choosenSongFromPlaylist != null) {
           CurrentPlayingSong = choosenSongFromPlaylist;
           CurrentAvailableDisplayOption = "pause.png";
           Thread.Sleep(1000);
           StartPlayingChoosenSong(choosenSongFromPlaylist);
            }
        }
        private string PlayNextSongFromPlaylist(string songFromPlaylist)
        {
            string nextSong;
            var dbSongs = Database.Table<Songs>().ToListAsync().Result;
            var playlistId = (from song in dbSongs where song.SongTitle.Equals(songFromPlaylist) select song.PlaylistId).FirstOrDefault();
            var songsFromCurrentPlayingPlaylist = (from songs in dbSongs where songs.PlaylistId.Equals(playlistId) orderby songs.ID select songs).ToList();
            try { 
            nextSong = songsFromCurrentPlayingPlaylist.SkipWhile(song => !song.SongTitle.Equals(songFromPlaylist)).Skip(1).FirstOrDefault().SongTitle;
            }catch(Exception ex)
            {
                return null;
            }
                return nextSong;
        }
        public void UpdateTime()
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
                TimeSpan time = TimeSpan.FromSeconds(CurrentSongPosition);
                CurrentSongTime = time.ToString("mm':'ss");
                
            }
        }
        private void UpdateSongStatus(Action action, double seconds, CancellationToken token)
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

        public void CheckIfAddShoulBeVisible(int pageNumber)
        {
            AddIsVisible = pageNumber == 0;
        }
       
    }
}
