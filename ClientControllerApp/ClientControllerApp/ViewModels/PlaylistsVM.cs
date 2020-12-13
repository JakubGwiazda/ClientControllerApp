using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;

namespace ClientControllerApp
{
    public class PlaylistsVM : INotifyPropertyChanged
    {
        ObservableCollection<Playlist> playList = new ObservableCollection<Playlist>();
        ObservableCollection<Songs> songsOnPlaylist = new ObservableCollection<Songs>();
        ObservableCollection<PlaylistModel> playlistModel = new ObservableCollection<PlaylistModel>();
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _IsVisible=false;
       private string _PlaylistTitleToAdd;
        private string _inputtedText;
        SQLiteAsyncConnection Database;


        public ObservableCollection<Playlist> Playlists
        {
            get { return playList; }
            set
            {
                playList=value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Songs> Songs
        {
            get { return songsOnPlaylist; }
            set
            {
                songsOnPlaylist = value;
                OnPropertyChanged();
            }
        }
        public static string SongToAdd { get; set; }
        public static bool AddSong { get; set; }
        public ObservableCollection<PlaylistModel> PlaylistsToDisplay
        {
            get { return playlistModel; }
            set
            {
                playlistModel = value;
                OnPropertyChanged();
            }
        }
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
                OnPropertyChanged();
            }
        }
        public string InputtedText { get { return _inputtedText; } set { _inputtedText = value; OnPropertyChanged(); } }
        public string PlaylistTitleToAdd
        {
            get
            {
                return _PlaylistTitleToAdd;
            }
            set
            {
                _PlaylistTitleToAdd = value;
                OnPropertyChanged();
            }
        }
        public PlaylistsVM()
        {
            Database = DatabaseInitializator.Database();
           // Playlists = this.GetPlaylistsFromDB();
            PlaylistsToDisplay= SetPlaylistToDisplay();
            AddSong = false;
        }
        private ObservableCollection<PlaylistModel> SetPlaylistToDisplay()
        {

            var dbPlaylist = Database.Table<Playlist>().ToListAsync().Result;
            var dbSongs = Database.Table<Songs>().ToListAsync().Result;
            ObservableCollection<PlaylistModel> tempList = new ObservableCollection<PlaylistModel>();
            foreach(var playlist in dbPlaylist)
            {

                IEnumerable<Songs> playlistSongSet = from song in dbSongs where song.PlaylistId == playlist.ID select song;
                var tmp = new PlaylistModel { PlaylistTitle = playlist.PlaylistName };
                foreach(var item in playlistSongSet)
                {
                    tmp.Add(new DBSongsModel { SongTitle = item.SongTitle });
                }
                tempList.Add(tmp);
            }
            return tempList;


        }

        private ObservableCollection<Playlist> GetPlaylistsFromDB()
        {
            // List<Playlist> myList = Database.Table<Playlist>().ToListAsync().Result;
            return new ObservableCollection<Playlist>(Database.Table<Playlist>().ToListAsync().Result);

        }

        public ICommand AddToPlaylist => new Command((p) =>
        {
            if (AddSong == true) { 
            int PlaylistID = playList.Where(listPosition => listPosition.PlaylistName.Equals(p)).Select(i => i.ID).First();
            Songs song = new Songs
            {
                PlaylistId = PlaylistID,
                SongTitle = SongToAdd
            };
                Database.InsertAsync(song);
                AddSong = false;
             
            }
        });

        public ICommand DeleteFromPlaylist => new Command((p) =>
        {
            PlaylistModel PlaylistObjectToDelete = PlaylistsToDisplay.Where(listPosition => listPosition.PlaylistTitle.Equals(p)).Select(i => i).First();//.PlaylistName.Equals(p)).Select(i => i).First();

            Database.ExecuteAsync($"Delete from Playlist WHERE Playlist_Name='{PlaylistObjectToDelete.PlaylistTitle}'");
           var a = Database.Table<Playlist>().ToListAsync().Result;
            PlaylistsToDisplay.Remove(PlaylistObjectToDelete);
        });
        public ICommand CreatePlaylist => new Command((p) =>
        {
            if (!CheckIfPlaylistAlreadyExists((string)p)) { 
            Playlist pl = new Playlist()
            {
                PlaylistName = (string)p
            };
                IsVisible = false;
                Database.InsertAsync(pl).GetAwaiter().GetResult();
                InputtedText = "";
                PlaylistsToDisplay = SetPlaylistToDisplay();
            }
            else
            {
                IsVisible = true;
            }

        });
        private bool CheckIfPlaylistAlreadyExists(string playlistTitleToSave)
        {

            var valueFromList = (from savedNames in PlaylistsToDisplay where savedNames.PlaylistTitle == playlistTitleToSave select savedNames);
            if (valueFromList.Count() == 0)
            {
                return false;
            }
           
            if (valueFromList.First().PlaylistTitle.ToLower().Equals(playlistTitleToSave.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
       
        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
