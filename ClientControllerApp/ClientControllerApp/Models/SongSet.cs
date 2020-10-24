using System;
using System.Collections.Generic;
using System.Text;

namespace ClientControllerApp
{
    public class SongSet : List<Song>
    {
        public string FirstCharacter { get; set; }

        public SongSet(string firstChar)
        {
            this.FirstCharacter = firstChar;
        }
    }
}
