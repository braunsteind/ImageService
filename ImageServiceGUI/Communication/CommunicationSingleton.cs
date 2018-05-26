using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using ImageService.Modal;

namespace ImageServiceGUI.Communication
{
    class CommunicationSingleton : ICommunicationSingleton
    {
        TcpClient client;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        private static CommunicationSingleton instance;
        public event EventHandler<CommandRecievedEventArgs> InMessage;
        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
        }

        // private constructor
        private CommunicationSingleton()
        {
            IsConnected = false;
            Connect();
        }

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

        private void Connect()
        {
            // try to connect
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                IsConnected = true;
            }
            // if error occurred
            catch (Exception e)
            {
                IsConnected = false;
                Console.WriteLine(e.Message);
            }
        }

        public void Write(CommandRecievedEventArgs command)
        {
            try
            {
                //json the command
                string json = JsonConvert.SerializeObject(command);
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                //send json
                writer.Write(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Read()
        {
            new Task(() =>
            {
                // while connected
                while (IsConnected)
                {
                    try
                    {
                        // read from server
                        string temp = reader.ReadString();
                        if (temp != "")
                        {
                            // create command event args
                            CommandRecievedEventArgs msg = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(temp);
                            // if close command
                            if (msg.CommandID == (int)CommandEnum.CloseCommand)
                            {
                                // disconnect
                                Disconnect();
                                return;
                            }
                            // invoke incoming message event
                            this.InMessage?.Invoke(this, msg);
                        }
                    }
                    // if failed
                    catch (Exception e)
                    {
                        // write message
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            }).Start();
        }

        public void Disconnect()
        {
            // close the connection
            client.Close();
            // change IsConnected to false
            IsConnected = false;
        }
    }
}