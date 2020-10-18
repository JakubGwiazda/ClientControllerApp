using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
namespace ClientControllerApp
{
    class SoundPageVM:INotifyPropertyChanged
    {
        public Command<string> ChangeSoundLevelCommand { get; set; }
        string currentSoundLevel;
       
        public SoundPageVM()
        {
           OrderSender.GetCurrentServerSoundLevel();
           CurrentSoundLevel = MessageReceiver.GetResponseFromServer();
           ChangeSoundLevelCommand = new Command<string>((level) => ChangeSoundLevel(level));
        }

        
        public string CurrentSoundLevel
        {
            get
            {
                return currentSoundLevel;
            }
            set
            {
                currentSoundLevel = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        void ChangeSoundLevel(string level)
        {
            CurrentSoundLevel = level;
            OrderSender.ChangeMainSoundLevelOnServer(level);

        }
     
         void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
