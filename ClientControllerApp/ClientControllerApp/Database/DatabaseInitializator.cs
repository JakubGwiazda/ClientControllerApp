using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace ClientControllerApp
{
    public class DatabaseInitializator
    {
/*        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DataBase.DatabasePath, DataBase.Flags);
        });
        public static SQLiteAsyncConnection Database => lazyInitializer.Value;*/
        static SQLiteAsyncConnection _connection;

        static bool initialized = false;
    
        public static SQLiteAsyncConnection Database()
        {
            if (_connection == null)
            {
                _connection = new SQLiteAsyncConnection(DataBase.DatabasePath, DataBase.Flags);
                _connection.CreateTableAsync<Playlist>();
                return _connection;
            }
            else
            {
                return _connection;
            }
        }
/*
        async Task InitializeAsyncDatabase()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PlaylistModel).Name)){
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PlaylistModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }*/

    }
}
