﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClientControllerApp
{
     public   class PlaylistModel:List<DBSongsModel>
    {
        public  string PlaylistTitle { get; set; }
     
    }
}
