using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Server
{
    interface IClientHandler
    {
        /// <summary>
        /// Handle client/clients.
        /// </summary>
        /// <param name="client">The client to handle</param>
        /// <param name="clients">all connected clients</param>
        void HandleClient(TcpClient client, List<TcpClient> clientList);
    }
}
