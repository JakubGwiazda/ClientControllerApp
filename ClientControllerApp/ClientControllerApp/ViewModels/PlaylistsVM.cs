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
        SQLiteAsyncConnection Database;
        public static string SongToAdd { get; set; }
        public static bool AddSong { get; set; }
        public PlaylistsVM()
        {
            Database = DatabaseInitializator.Database();
            playList = this.GetPlaylistsFromDB();
            AddSong = false;
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
               var a = Database.Table<Songs>().ToListAsync().Result;
                var b = 0;
            }
        });

        public ICommand DeleteFromPlaylist => new Command((p) =>
        {

            Playlist PlaylistObjectToDelete = playList.Where(listPosition => listPosition.PlaylistName.Equals(p)).Select(i => i).First();
            //Database.DeleteAllAsync<Playlist>();
            Database.DeleteAsync(PlaylistObjectToDelete);
            playList.Remove(PlaylistObjectToDelete);
        });
        public ICommand CreatePlaylist => new Command((p) =>
        {
            Playlist pl = new Playlist()
            {
                PlaylistName = (string)p
            };
            Database.InsertAsync(pl);
            playList.Add(new Playlist { PlaylistName=pl.PlaylistName});
         
        });
        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
