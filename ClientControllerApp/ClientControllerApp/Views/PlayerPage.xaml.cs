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
        private PlayerPageVM ppvm;
        public PlayerPage()
        {
            ppvm = new PlayerPageVM();
            BindingContext = ppvm;
            InitializeComponent();
            DisplayPageElements();
           
        }
       
        void DisplayPageElements()
        {
            this.Content = new StackLayout
            {
                Children =
                {
                    ShowMainTitle(),
                    ShowList(),
                  ShowSongPlayerMenu()
                }
            };
        }
       private Label ShowMainTitle()
        {
            Label titleLabel = new Label();
            titleLabel.Text = "List of songs on the server. Choose one to play.";
            titleLabel.HorizontalOptions = LayoutOptions.Center;
            titleLabel.VerticalOptions = LayoutOptions.Start;
            return titleLabel;
        }
        private ListView ShowList()
        {
            ListView listView = new ListView
            {
                ItemsSource = PlayerPageVM.ListOfSongsFromServer,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("FirstCharacter"),
                SelectionMode=ListViewSelectionMode.Single,
                
              // SelectedItem=new Binding("SelectedSong"),
                
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
            listView.ItemSelected += OnSelection;
            return listView;
         
        }
        
        private StackLayout ShowSongPlayerMenu()
        {
            
            Label actualSongTitle = new Label();
            actualSongTitle.SetBinding(Label.TextProperty, "CurrentPlayingSong");

            Button playSongButton = new Button();
            playSongButton.Text = "Play";
            playSongButton.SetBinding(Button.TextProperty, "CurrentAvailableDisplayOption");
            playSongButton.Command = ppvm.ChangeButtonOption;

            Button backwardSong = new Button();
            backwardSong.Text = "Back";
            Button forwardSong = new Button();
            forwardSong.Text = "Forward";
            StackLayout songPlayer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(0, 10),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    actualSongTitle,
                    new StackLayout
                    {
                        Orientation=StackOrientation.Horizontal,
                        Children =
                        {
                            backwardSong,
                            playSongButton,
                            forwardSong
                        }
                    }
                }
            };
            return songPlayer;
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            
            ppvm.CurrentPlayingSong = ((Song)e.SelectedItem).SongTitle;
            ppvm.CurrentAvailableDisplayOption = "Stop";
            ppvm.PlayChoosenSong((Song)e.SelectedItem);

        }



    }

}