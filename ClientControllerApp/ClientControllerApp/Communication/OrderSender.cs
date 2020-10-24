using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
namespace ClientControllerApp
{
    public static class OrderSender
    {

        private async static void SendOrderToServer(string orderToSend)
        {
            try
            {
                NetworkStream stream = Connector.Instance.client.GetStream();
                byte[] bytesToSend = System.Text.Encoding.UTF8.GetBytes(orderToSend);
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", "" + ex.ToString(), "OK");
            }
        }
        public static void ChangeMainSoundLevelOnServer(string level)
        {
           SendOrderToServer(CreateJSON(ServerOrderList.setsound.ToString(), level));
        }

        public static void GetCurrentServerSoundLevel()
        {
            SendOrderToServer(CreateJSON(ServerOrderList.getsound.ToString(), null));
        }
        public static void GetServerSoundList()
        {
            SendOrderToServer(CreateJSON(ServerOrderList.GET_SONG_LIST.ToString(), null));
        }

        public static void PlaySong(string song)
        {
            SendOrderToServer(CreateJSON(ServerOrderList.PLAY_SONG.ToString(),song));
        }
        public static void StopPlayingSong()
        {
            SendOrderToServer(CreateJSON(ServerOrderList.STOP_PLAY.ToString(), null));
        }
        public static void PlaySongAgain()
        {
            SendOrderToServer(CreateJSON(ServerOrderList.START_PLAY_AGAIN.ToString(), null));
        }
        public static string CreateJSON(string order, string message)
        {
            var messageToConvert = new MessageToServer { Order = order, Message = message };
            return JsonConvert.SerializeObject(messageToConvert);
        }
    }
    public class MessageToServer
    {
        public string Order { get; set; }
        public string Message { get; set; }
    }
}
