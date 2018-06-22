using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class TCPClientHandler : ITCPClientHandler
    {
        //properties
        IImageController ImageController { get; set; }
        ILoggingService LoggingService { get; set; }
        private bool isStopped = false;
        public static Mutex Mutex { get; set; }

       /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="imageController"></param>
       /// <param name="logging"></param>
        public TCPClientHandler(IImageController imageController, ILoggingService logging)
        {
            this.ImageController = imageController;
            this.LoggingService = logging;
        }
        
        /// <summary>
        /// Handle client function, using threads(tasks)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientList"></param>
        public void HandleClient(TcpClient client, List<TcpClient> clientList)
        {
            try
            {
                new Task(() =>
                {
                    try
                    {
                        while (!isStopped)
                        {
                            //bytes communication instead of strings
                            NetworkStream ns = client.GetStream();
                            Byte[] temp = new Byte[1];
                            List<Byte> fileName = new List<byte>();
                            do
                            {
                                ns.Read(temp, 0, 1);
                                fileName.Add(temp[0]);
                            } while (ns.DataAvailable);
                            string name = Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(fileName.ToArray()));

                            Byte[] confirm = new byte[1];
                            confirm[0] = 1;
                            ns.Write(confirm, 0, 1);

                            List<Byte> bytesArray = new List<byte>();
                            Byte[] tempBytes;
                            Byte[] data = new Byte[6790];
                            int i = 0;
                            //getting the images in bytes
                            do
                            {
                                i = ns.Read(data, 0, data.Length);
                                tempBytes = new byte[i];
                                for (int n = 0; n < i; n++)
                                {
                                    tempBytes[n] = data[n];
                                    bytesArray.Add(tempBytes[n]);
                                }
                                System.Threading.Thread.Sleep(300);
                            } while (ns.DataAvailable || i == data.Length);

                            File.WriteAllBytes(ImageController.Server.Directories[0] + @"\" + name + ".jpg", bytesArray.ToArray());
                        }
                    }
                    catch (Exception e)
                    {
                        clientList.Remove(client);
                        LoggingService.Log(e.Message, MessageTypeEnum.FAIL);
                        client.Close();
                    }
                }).Start();
            }
            catch (Exception e)
            {
            }
        }
    }
}
