using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;

namespace ImageServiceGUI.Communication
{
    class CommunicationSingleton : ICommunicationSingleton
    {
        TcpClient client;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        private static CommunicationSingleton instance;
        public event EventHandler<CommandEventArgs> InMessage;
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

        public void Connect(string ip, int port)
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
                StartReading();
            }
            // if error occurred
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Write(string command)
        {
            // check connection
            if (IsConnected)
            {
                try
                {
                    // write command to server
                    writer.Write(command);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void StartReading()
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
                        JObject msgObj = JObject.Parse(temp);
                        // create command event args
                        CommandEventArgs msg = new CommandEventArgs();
                        msg.Command = (CommandEnum)msgObj["Command"];
                        msg.Args = (string)msgObj["Args"];
                        // if close command
                        if (msg.Command == CommandEnum.CloseCommand)
                        {
                            // disconnect
                            Disconnect();
                            return;
                        }
                        // invoke incoming message event
                        this.InMessage?.Invoke(this, msg);
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