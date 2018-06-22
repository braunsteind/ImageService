using System.Net;
using System.Net.Sockets;
using System;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class TCPServer : ITCPServer
    {
        //class members
        string localIP = "127.0.0.1";
        ILoggingService LoggingService { get; set; }
        int Port { get; set; }
        TcpListener Listener { get; set; }
        ITCPClientHandler Handler { get; set; }
        private static Mutex mutexLock = new Mutex();
        private List<TcpClient> clients = new List<TcpClient>();


        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="port"></param>
        /// <param name="logging"></param>
        /// <param name="ch"></param>
        public TCPServer(int port, ILoggingService logging, ITCPClientHandler ch)
        {
            this.Port = port;
            this.LoggingService = logging;
            this.Handler = ch;
            ClientHandler.MutexLock = mutexLock;
        }

        /// <summary>
        /// Starting server function
        /// </summary>
        public void StartServer()
        {
            try
            {
                //initializing network compopnents
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(localIP), Port);
                Listener = new TcpListener(ep);
                Listener.Start();
                //accepting connections
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient newClient = Listener.AcceptTcpClient();
                            clients.Add(newClient);
                            Handler.HandleClient(newClient, clients);
                        }
                        catch (Exception e)
                        {
                            LoggingService.Log("Failed accepting clients " + e.Message, MessageTypeEnum.FAIL);
                            break;
                        }
                    }
                });
                task.Start();
            }
            catch (Exception e)
            {
                LoggingService.Log("Failed starting server " + e.Message, MessageTypeEnum.FAIL);
            }
        }
    }
}
