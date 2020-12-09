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
        SQLiteAsyncConnection Database;
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

          //  Playlist PlaylistObjectToDelete = playList.Where(listPosition => listPosition.PlaylistName.Equals(p)).Select(i => i).First();
            //Database.DeleteAllAsync<Playlist>();
           // Database.DeleteAsync(PlaylistObjectToDelete);
            Database.ExecuteAsync($"Delete from Playlist WHERE Playlist_Name='{PlaylistObjectToDelete.PlaylistTitle}'");
           var a = Database.Table<Playlist>().ToListAsync().Result;
            PlaylistsToDisplay.Remove(PlaylistObjectToDelete);
            //playList.Remove(PlaylistObjectToDelete);
        });
        public ICommand CreatePlaylist => new Command((p) =>
        {
            Playlist pl = new Playlist()
            {
                PlaylistName = (string)p
            };
            Database.InsertAsync(pl);
            PlaylistsToDisplay = SetPlaylistToDisplay();
           // Playlists = GetPlaylistsFromDB();
           //playList.Add(new Playlist { PlaylistName=pl.PlaylistName});

        });
        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
