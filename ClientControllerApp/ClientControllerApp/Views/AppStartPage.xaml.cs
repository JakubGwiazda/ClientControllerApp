using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Sockets;

namespace ClientControllerApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AppStartPage : ContentPage
    {
        public AppStartPage()
        {
            InitializeComponent();
        }

    private async void Connect_Clicked(object sender,EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync(IPAddress.Text, Convert.ToInt32(Port.Text));
                if (client.Connected)
                {
                    Connector.Instance.client = client;
                    await DisplayAlert("Connected", "Connected to server successfully", "OK");
                    Application.Current.MainPage = new NavigationPage(new MainMenu());
                }
                else
                {
                    await DisplayAlert("Error", "Connection unsucessful", "OK");
                }
            }catch(Exception ex)
            {
                await DisplayAlert("Error", "" + ex.ToString(), "OK");
            }
        }

    }
}
