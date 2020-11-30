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
        public MainMenu()
        {
            InitializeComponent();
        }
        private async void Sound_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SoundPage());
        }

        private async void  Player_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerPage());
        }
        private async void Music_Player_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MusicPlayerMainPage());
        }
    }
}