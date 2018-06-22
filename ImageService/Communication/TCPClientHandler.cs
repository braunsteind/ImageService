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
        IImageController ImageController { get; set; }
        ILoggingService LoggingService { get; set; }
        private bool m_isStopped = false;
        public static Mutex Mutex { get; set; }

       
        public TCPClientHandler(IImageController imageController, ILoggingService logging)
        {
            this.ImageController = imageController;
            this.LoggingService = logging;

        }
        
      
        public void HandleClient(TcpClient client, List<TcpClient> clientList)
        {
            try
            {

                new Task(() =>
                {
                    try
                    {
                        while (!m_isStopped)
                        {
                            NetworkStream stream = client.GetStream();
                            string finalNameString = GetFileName(stream);
                            Byte[] confirmation = new byte[1];
                            confirmation[0] = 1;
                            stream.Write(confirmation, 0, 1);
                            List<Byte> finalbytes = GetImageBytes(stream);
                            File.WriteAllBytes(ImageController.Server.Directories[0] + @"\" + finalNameString + ".jpg", finalbytes.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        clientList.Remove(client);
                        LoggingService.Log(ex.ToString(), MessageTypeEnum.FAIL);
                        client.Close();
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex.ToString(), MessageTypeEnum.FAIL);
            }
        }

        private string GetFileName(NetworkStream stream)
        {
            Byte[] temp = new Byte[1];
            List<Byte> fileName = new List<byte>();
            do
            {
                stream.Read(temp, 0, 1);
                fileName.Add(temp[0]);
            } while (stream.DataAvailable);

            return Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(fileName.ToArray()));

        }

      
        private List<Byte> GetImageBytes(NetworkStream stream)
        {
            List<Byte> bytesArr = new List<byte>();
            Byte[] tempForReadBytes;
            Byte[] data = new Byte[6790];
            int i = 0;

            do
            {
                i = stream.Read(data, 0, data.Length);
                tempForReadBytes = new byte[i];
                for (int n = 0; n < i; n++)
                {
                    tempForReadBytes[n] = data[n];
                    bytesArr.Add(tempForReadBytes[n]);
                }
                System.Threading.Thread.Sleep(300);
            } while (stream.DataAvailable || i == data.Length);
            return bytesArr;
        }
    }
}
