using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ClientControllerApp
{
    class Connector
    {

        private static Connector _instance;
        public static Connector Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Connector();
                return _instance;
            }
        }
        public TcpClient client { get; set; }
        public NetworkStream stream { get { return client.GetStream(); } }
       

    }
}
