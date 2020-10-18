using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace ClientControllerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        public PlayerPage()
        {
            InitializeComponent();
            ShowMainTitle();
            ShowList();
        
        
        }
       void ShowMainTitle()
        {
            Label titleLabel = new Label();
            titleLabel.Text = "List of songs on the server. Choose one to play.";
            titleLabel.HorizontalOptions = LayoutOptions.Center;

        }
        void ShowList()
        {
            ListView listView = new ListView
            {
                ItemsSource = PlayerPageVM.ListOfSongsFromServer,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("FirstCharacter"),
                ItemTemplate = new DataTemplate(() =>
                {
                    Label titleLabel = new Label();
                    titleLabel.SetBinding(Label.TextProperty, "SongTitle");
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new StackLayout
                                {
                                    VerticalOptions=LayoutOptions.Center,
                                    Spacing=0,
                                    Children =
                                    {
                                         titleLabel
                                    }
                                }
                            }

                        }
                    };
                })

            };
            this.Content = new StackLayout
            {
                Children =
                {
                    listView
                }
            };
         

        }
    }

}