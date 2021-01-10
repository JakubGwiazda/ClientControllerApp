using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
namespace ClientControllerApp
{
    public static class MessageReceiver
    {

        public static  string GetResponseFromServer()
        {
            
            byte[] msgBuffor = new byte[4096];
            StringBuilder myCompleteMessage = new StringBuilder();
            if (Connector.Instance.stream.CanRead)
            {
                 do
                {
                    int numberOfBytesRead = Connector.Instance.stream.Read(msgBuffor, 0, msgBuffor.Length);
                    myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(msgBuffor, 0, numberOfBytesRead));
                    Thread.Sleep(500);
                } while (Connector.Instance.stream.DataAvailable);
                
            }
            string dataReceived = myCompleteMessage.ToString();
            return dataReceived;
        }

      
        public static Dictionary<string,List<string>> GetServerSongListFromResponse()
        {
            byte[] msgBuffor = new byte[Connector.Instance.client.ReceiveBufferSize];
            if (Connector.Instance.stream.CanRead)
            {
                Connector.Instance.stream.Read(msgBuffor, 0, (int)Connector.Instance.client.ReceiveBufferSize);
            }
            var mStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();
            mStream.Write(msgBuffor, 0, msgBuffor.Length);
            mStream.Position = 0;
            var myObj = binFormatter.Deserialize(mStream);
            return (Dictionary<string, List<string>>)myObj;
        }
      
    }
}
