using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceWebApplication.Communication
{
    class CommunicationSingleton : ICommunicationSingleton
    {
        const string IP = "127.0.0.1";
        const int PORT = 8000;
        private static Mutex m_mutex = new Mutex();
        private static Mutex m_readMutex = new Mutex();
        TcpClient client;
        //NetworkStream stream;
        //BinaryReader reader;
        //BinaryWriter writer;
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

        /// <summary>
        /// Get instance of the singleton
        /// </summary>
        public static CommunicationSingleton Instance
        {
            get
            {
                //if not instance, create one
                if (instance == null)
                {
                    instance = new CommunicationSingleton();
                }
                //if instance exist, return it
                return instance;
            }
        }

        /// <summary>
        /// Connect to the server using TCP
        /// </summary>
        private void Connect()
        {
            // try to connect
            try
            {
                //connect to server
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
                client = new TcpClient();
                client.Connect(ep);
                //stream = client.GetStream();
                //reader = new BinaryReader(stream);
                //writer = new BinaryWriter(stream);
                IsConnected = true;
            }
            // if error occurred
            catch (Exception e)
            {
                IsConnected = false;
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Write to the server
        /// </summary>
        /// <param name="command">The command the write</param>
        public void Write(CommandRecievedEventArgs command)
        {
            try
            {
                //json the command
                string json = JsonConvert.SerializeObject(command);
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                //send json
                m_mutex.WaitOne();
                writer.Write(json);
                m_mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Reading info from the server
        /// </summary>
        public void Read()
        {
            new Task(() =>
            {
                // while connected
                while (IsConnected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        // read from server
                        m_readMutex.WaitOne();
                        string temp = reader.ReadString();
                        m_readMutex.ReleaseMutex();
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

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.DisconnectClient, null, "");
            this.Write(commandRecievedEventArgs);
            // close the connection
            client.Close();
            // change IsConnected to false
            IsConnected = false;
        }
    }
}