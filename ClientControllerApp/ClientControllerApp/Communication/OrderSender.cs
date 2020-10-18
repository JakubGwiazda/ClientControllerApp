using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ClientControllerApp
{
    public static class OrderSender
    {

        private async static void SendOrder(string order, string level)
        {
            try
            {
                NetworkStream stream = Connector.Instance.client.GetStream();
                string soundLvL = order+":" + level;
                byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes(soundLvL);
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", "" + ex.ToString(), "OK");
            }
        }
        private async static void SendRequestForData(string request)
        {
            try
            {
                NetworkStream stream = Connector.Instance.client.GetStream();
                string soundLvL = request;
                byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes(soundLvL);
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", "" + ex.ToString(), "OK");
            }

        }
        public static void ChangeMainSoundLevelOnServer(string level)
        {

            SendOrder(ServerOrderList.setsound.ToString(), level);
        }

        public static void GetCurrentServerSoundLevel()
        {
            SendRequestForData(ServerOrderList.getsound.ToString());

        }
        public static void GetServerSoundList()
        {
            SendRequestForData(ServerOrderList.GET_SONG_LIST.ToString());
        }

    }
}
