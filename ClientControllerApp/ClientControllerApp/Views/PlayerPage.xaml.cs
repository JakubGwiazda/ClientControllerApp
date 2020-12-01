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
            backwardSong.Command = ppvm.Backward;
            Button forwardSong = new Button();
            forwardSong.Text = "Forward";
            forwardSong.Command = ppvm.Forward;

            Label currentSongTime = new Label();
            currentSongTime.SetBinding(Label.TextProperty, "CurrentSongTime");
            currentSongTime.WidthRequest = 40;
            currentSongTime.HorizontalTextAlignment = TextAlignment.Center;
            Label durationSongTime = new Label();
            durationSongTime.SetBinding(Label.TextProperty, "SongDurationTime");
            durationSongTime.HorizontalTextAlignment = TextAlignment.Center;
            durationSongTime.WidthRequest = 40;
            Label testowa = new Label();
            testowa.HorizontalOptions = LayoutOptions.Center;
            testowa.BackgroundColor = Color.Blue;
            Slider songProgressSlider = new Slider();
            songProgressSlider.HorizontalOptions = LayoutOptions.FillAndExpand;
            songProgressSlider.Minimum = 0;
            songProgressSlider.SetBinding(Slider.MaximumProperty, "CurrentSongMaxDurationInSeconds");
            songProgressSlider.SetBinding(Slider.ValueProperty, "CurrentSongPosition",BindingMode.TwoWay);
            songProgressSlider.DragStartedCommand =  ppvm.StopPlayingOnDragSlider;
            songProgressSlider.DragCompletedCommand = ppvm.StartPlayingOnDropSlider;
            songProgressSlider.ValueChanged += SliderValueChanged;
            songProgressSlider.MinimumTrackColor = Color.Red;
            songProgressSlider.MaximumTrackColor = Color.Gray;

            ImageButton addToPlayListButton = new ImageButton
            {
                Source = "add25_25.png",
                BackgroundColor=Color.White,
                HorizontalOptions=LayoutOptions.End
            };

            addToPlayListButton.Clicked += ((sender,e) => {
                (this.Parent as TabbedPage).CurrentPage = (this.Parent as TabbedPage).Children[1];
                PlaylistsVM.SongToAdd = actualSongTitle.Text;
                PlaylistsVM.AddSong = true;
            });
            
            StackLayout songPlayer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(0, 10),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    actualSongTitle,
                    new StackLayout
                    {
                        Orientation=StackOrientation.Horizontal,
                        HorizontalOptions=LayoutOptions.Center,
                        Children =
                        {
                            backwardSong,
                            playSongButton,
                            forwardSong,
                            addToPlayListButton
                        }
                    },
                    new StackLayout
                       {
                         Orientation=StackOrientation.Horizontal,
                         HorizontalOptions=LayoutOptions.FillAndExpand,
                                Children={
                                currentSongTime,
                                songProgressSlider,
                                durationSongTime
                                
                                }
                                
                      },
                }
            };
            return songPlayer;
        }
    
         void SliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds((int)e.NewValue);
            ppvm.CurrentSongTime = time.ToString("mm':'ss");
            
        }
       
        async void ShowPlaylistMiniMenu(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddToPlaylistModal());
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