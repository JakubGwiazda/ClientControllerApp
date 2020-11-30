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

        
        public PlaylistsVM()
        {
            Database = DatabaseInitializator.Database();
            playList = this.GetPlaylistsFromDB();
        }

        private ObservableCollection<Playlist> GetPlaylistsFromDB()
        {
            // List<Playlist> myList = Database.Table<Playlist>().ToListAsync().Result;
            return new ObservableCollection<Playlist>(Database.Table<Playlist>().ToListAsync().Result);

        }

        public ICommand AddToPlaylist => new Command(() =>
        {

        });

        public ICommand DeleteFromPlaylist => new Command((p) =>
        {
            int PlaylistIdToDelete =  playList.Where(listPosition => listPosition.PlaylistName.Equals(p)).Select(i => i.ID).First();
            var PlaylistIdToDelete2 = playList.Where(listPosition => listPosition.PlaylistName.Equals(p)).Select(i=>i).First();
            
            Database.DeleteAsync(PlaylistIdToDelete2.ID);
            playList.Remove(PlaylistIdToDelete2);
        });
        public ICommand CreatePlaylist => new Command((p) =>
        {
            // DatabaseInitializator.Database().InsertAsync(p);
            Playlist pl = new Playlist()
            {
                PlaylistName = (string)p
            };
            Database.InsertAsync(pl);
            playList.Add(new Playlist { PlaylistName=pl.PlaylistName});
            /*  var inserted = Database.Table<Playlist>().ToListAsync();
              var a = inserted.Result;*/
        });
        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
