using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.RangeSlider.Common;
using Xamarin.RangeSlider.Forms;

namespace ClientControllerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        public PlayerPage()
        {
            BindingContext = new PlayerPageVM();
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
                  new Player()

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
                SelectionMode = ListViewSelectionMode.Single,
                

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

       
        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            PlayerVM.Instance.CurrentPlayingSong = ((Song)e.SelectedItem).SongTitle;
            PlayerVM.Instance.CurrentAvailableDisplayOption = "pause.png";
            PlayerVM.Instance.StartPlayingChoosenSong(((Song)e.SelectedItem).SongTitle);
          

        }
     
    }

}