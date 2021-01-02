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
    public partial class Player : ContentView
    {
        public Player()
        {
            InitializeComponent();
            BindingContext = PlayerVM.Instance;
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds((int)e.NewValue);
            PlayerVM.Instance.CurrentSongTime = time.ToString("mm':'ss");

        }
        private void OnAddButtonClicked(object sender, ValueChangedEventArgs e)
        {
             (this.Parent as TabbedPage).CurrentPage = (this.Parent as TabbedPage).Children[1];

        }

    }
}