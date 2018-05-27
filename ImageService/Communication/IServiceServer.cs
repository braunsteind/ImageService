using ImageService.Modal;

namespace ImageService.Server
{
    interface IServiceServer
    {
        /// <summary>
        /// Start the server
        /// </summary>
        void StartServer();
        /// <summary>
        /// Update args to the clients
        /// </summary>
        /// <param name="e">The args</param>
        void Update(CommandRecievedEventArgs e);
        /// <summary>
        /// Stop the server
        /// </summary>
        void StopServer();
    }
}