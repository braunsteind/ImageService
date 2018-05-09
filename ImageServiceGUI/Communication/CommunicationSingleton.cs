using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace ImageServiceGUI.Communication
{
    class CommunicationSingleton : ICommunicationSingleton
    {
        TcpClient client;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;

        private static CommunicationSingleton instance;

        public static CommunicationSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationSingleton();
                }
                return instance;
            }
        }

        // private constructor
        private CommunicationSingleton()
        {
            
        }

        public void connect(string ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            client.Connect(ep);
        }

        public void write(string command)
        {
            throw new NotImplementedException();
        }

        public string read()
        {
            throw new NotImplementedException();
        }

        public void disconnect()
        {
            client.Close();
        }
    }
}