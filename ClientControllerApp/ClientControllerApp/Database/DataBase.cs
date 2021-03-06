﻿using System;
using System.Collections.Generic;
using SQLite;
using System.Text;
using System.IO;
namespace ClientControllerApp
{
    public static class DataBase
    {
        public const string DataBaseFileName = "PlaylistD2.db3";
        public const SQLite.SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(basePath, DataBaseFileName);
            }
        }

    }
}
