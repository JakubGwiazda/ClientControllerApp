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
    public partial class PlaylistLibrary : ContentPage
    {

    //    private readonly PlaylistsVM vm;

        public PlaylistLibrary()
        {
            InitializeComponent();
           // this.vm = BindingContext as PlaylistsVM;
        }
   


    }
}