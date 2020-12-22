using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientControllerApp
{
    public enum Communicats
    {
        [Description("Playlist with given title already exists. Please change title.")]
        Playlist_Exists,
        [Description("Song can be added only to one playlist.")]
        Song_Was_Already_Added,
        [Description("Playlist title cannot be empty.")]
        Playlist_empty

    }
}
