using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ServiceServer : IServiceServer
    {
        //properties
        ILoggingService Logging { get; set; }
        IClientHandler Handler { get; set; }
        TcpListener Listener { get; set; }
        int PortNumber { get; set; }
        //members
        private List<TcpClient> clientList;
        private static Mutex serverMutex = new Mutex();

        /// <summary>
        /// Constuctor.
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="handler"></param>
        /// <param name="port"></param>
                    
        public ServiceServer (ILoggingService logging, IClientHandler handler, int port)
        {
            this.Logging = logging;
            this.PortNumber = port;
            this.Handler = handler;
            clientList = new List<TcpClient>();
            ClientHandler.MutexLock = serverMutex;
        }

        
        public void StartServer()
        {
            try
            {
                Logging.Log("Server started", MessageTypeEnum.INFO);

                //IP translation
                string ip = "127.0.0.1";
                IPAddress IpFormat = IPAddress.Parse(ip);

                //network components
                IPEndPoint endpoint = new IPEndPoint(IpFormat, PortNumber);

                //start listening
                Listener = new TcpListener(endpoint);
                Listener.Start();
 
                //thread for accepting clients
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        try
                        {    
                            TcpClient client = Listener.AcceptTcpClient();
                            Logging.Log("Client connected", MessageTypeEnum.INFO);
                            clientList.Add(client);
                            Handler.HandleClient(client, clientList);
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                    }
                    Logging.Log("Stopped listening", MessageTypeEnum.INFO);
                });
                task.Start();
            }
            catch (Exception e)
            {
                Logging.Log(e.Message, MessageTypeEnum.FAIL);
            }
        }

       
        
        public void Update(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            try
            {
                List<TcpClient> copyClients = new List<TcpClient>(clientList);
                foreach (TcpClient client in copyClients)
                {
                    new Task(() =>
                    {
                        try
                        {
                            //netowrk components
                            NetworkStream ns = client.GetStream();
                            BinaryWriter bw = new BinaryWriter(ns);
                            string execute = JsonConvert.SerializeObject(commandRecievedEventArgs);


                            serverMutex.WaitOne();
                            bw.Write(execute);
                            serverMutex.ReleaseMutex();
                        }
                        catch (Exception e)
                        {
                            Logging.Log("Failed writing to one of the clients", MessageTypeEnum.FAIL);
                            this.clientList.Remove(client);
                        }

                    }).Start();
                }
            }
            catch (Exception e)
            {
                Logging.Log(e.Message, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// Stop the server from running - stop listening for clients
        /// </summary>
        public void StopServer()
        {
            Listener.Stop();
        }

    }
}