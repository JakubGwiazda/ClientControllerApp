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
    public partial class MainMenu : ContentPage
    {
        private readonly SoundPage _soundPage;
        private readonly MusicPlayerMainPage _musicPlayerMainPage;

        public MainMenu()
        {
            InitializeComponent();
            _soundPage = new SoundPage();
            _musicPlayerMainPage = new MusicPlayerMainPage();
        }
        private async void Sound_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_soundPage);
        }


        private async void Music_Player_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_musicPlayerMainPage);
        }
    }
}