using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class TCPServer : ITCPServer
    {

        ILoggingService Logging { get; set; }
        int Port { get; set; }
        TcpListener Listener { get; set; }
        ITCPClientHandler Ch { get; set; }
        private List<TcpClient> clients = new List<TcpClient>();
        private static Mutex m_mutex = new Mutex();

        /// <summary>
        /// ImageServiceSrv constructor.
        /// </summary>
        /// <param name="port">port num</param>
        /// <param name="logging">ILoggingService obj</param>
        /// <param name="ch">IClientHandler obj</param>
        public TCPServer(int port, ILoggingService logging, ITCPClientHandler ch)
        {
            this.Port = port;
            this.Logging = logging;
            this.Ch = ch;
            ClientHandler.MutexLock = m_mutex;
        }

        /// <summary>
        /// Start function.
        /// lissten to new clients.
        /// </summary>
        public void Start()
        {
            try
            {
                IPEndPoint ep = new
                IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
                Listener = new TcpListener(ep);

                Listener.Start();
                Logging.Log("Waiting for client connections...", MessageTypeEnum.INFO);
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = Listener.AcceptTcpClient();
                            Logging.Log("Got new connection", MessageTypeEnum.INFO);
                            clients.Add(client);
                            Ch.HandleClient(client, clients);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                    Logging.Log("Server stopped", MessageTypeEnum.INFO);
                });
                task.Start();
            }
            catch (Exception ex)
            {
                Logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
            }
        }

    }
}
