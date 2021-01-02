
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
            CheckPage();
        }


        public void SwitchToPlayerPage()
        {
            CurrentPage = playerPage;
        }
        public void SwitchToPlaylistPage()
        {
            CurrentPage = playlistPage;
        }

        private void CheckPage()
        {
            this.CurrentPageChanged += (object sender, EventArgs e) =>
            {
                int i = this.Children.IndexOf(this.CurrentPage);
                PlayerVM.Instance.CheckIfAddShoulBeVisible(i);
            };
        }

    }
}