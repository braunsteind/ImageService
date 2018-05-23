using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Server
{
    interface IClientHandler
    {
        /// <summary>
        /// Handle the client in the server
        /// </summary>
        /// <param name="client">The client to handle</param>
        /// <param name="clients">All the clients</param>
        void HandleClient(TcpClient client, List<TcpClient> clients);
    }
}
