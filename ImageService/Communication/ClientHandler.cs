using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        public static Mutex MutexLock { get; set; }
        //Properties
        IImageController ImageController { get; set; }
        ILoggingService Logging { get; set; }
        
        //flow control indicator
        private bool stoppedRunning = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ic">An image controller</param>
        /// <param name="ls">A logging service object</param>
        public ClientHandler(IImageController ic, ILoggingService ls)
        {
            this.ImageController = ic;
            this.Logging = ls;

        }

        /// <summary>
        /// Handling client/clients.
        /// </summary>
        /// <param name="client">client to handle with</param>
        /// <param name="clients">all connected clients</param>
        public void HandleClient(TcpClient client, List<TcpClient> clientList)
        {
            try
            {
                //handle inside a thread
                new Task(() =>
                {
                    try
                    {
                        while (!stoppedRunning)
                        {
                            //create network transaction objects
                            NetworkStream ns = client.GetStream();
                            BinaryReader br = new BinaryReader(ns);
                            
                            //log the recieved command
                            string recieved = br.ReadString();
                            string message = "Command recieved: " + recieved;

                            Logging.Log(message, MessageTypeEnum.INFO);
                            //Deserialize the client's command
                            CommandRecievedEventArgs commandArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(recieved);
                            //if a close command was recieved
                            if (commandArgs.CommandID == (int)CommandEnum.DisconnectClient)
                            {
                                //remove client and close connection
                                this.Logging.Log("Disconnecting client", MessageTypeEnum.INFO);
                                clientList.Remove(client);
                                client.Close();
                                break;
                            }
                            //else, execute
                            else
                            {
                                bool indicator;
                                string executed = this.ImageController.ExecuteCommand((int)commandArgs.CommandID, commandArgs.Args, out indicator);
                                
                                //create the network writing object
                                BinaryWriter bw = new BinaryWriter(ns);

                                //lock the writing
                                MutexLock.WaitOne();
                                bw.Write(executed);
                                MutexLock.ReleaseMutex();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //in case of exception, log the client's error and close connection
                        Logging.Log(e.ToString(), MessageTypeEnum.FAIL);
                        clientList.Remove(client);
                        client.Close();
                    }
                    //start handling
                }).Start();
            }
            catch (Exception e)
            {
                Logging.Log(e.Message, MessageTypeEnum.FAIL);

            }
        }
    }
}