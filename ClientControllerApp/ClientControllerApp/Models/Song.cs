using System;
using System.Collections.Generic;
using System.Text;

namespace ClientControllerApp
{
    public class Song
    {
        public string SongTitle { get; set; }
        public Song(string title)
        {
            this.SongTitle = title;
        }
    }
}
