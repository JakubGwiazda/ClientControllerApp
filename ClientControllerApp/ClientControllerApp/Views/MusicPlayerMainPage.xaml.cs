
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientControllerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPlayerMainPage : TabbedPage
    {
        readonly Page playerPage;
        readonly Page playlistPage;

        public MusicPlayerMainPage()
        {
            InitializeComponent();
            playerPage = new PlayerPage();
            playlistPage = new PlaylistLibrary();

            Children.Add(playerPage);
            Children.Add(playlistPage);
        }
     
        public void SwitchToPlayerPage()
        {
            CurrentPage = playerPage;
        }
        public void SwitchToPlaylistPage()
        {
            CurrentPage = playlistPage;
        }


    }
}