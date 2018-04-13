using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<DirectoryCloseEventArgs> CloseServer;
        #endregion
        /// <summary>
        /// ImageServer ctr.
        /// </summary>
        /// <param name="controller">IImageController obj</param>
        /// <param name="logging">ILoggingService obj</param>
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
 
            //creating handlers for each directory
            this.ExtractHandlersFromConfig();       
        }

        /// <summary>
        /// Extracting the folders 
        /// </summary>
        private void ExtractHandlersFromConfig()
        {
            string[] foldersToListen = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            foreach (string directory in foldersToListen)
            {
                this.CreateHandler(directory);
            }
        }


        /// <summary>
        /// CreateHandler function.
        /// </summary>
        /// <param name="path">the path the handler is on charge</param>
        private void CreateHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(m_logging, m_controller);
            CommandRecieved += handler.OnCommandRecieved;
            this.CloseServer += handler.OnCloseHandler;
            handler.StartHandleDirectory(path);
            this.m_logging.Log("Handler was created for directory: " + path, Logging.Modal.MessageTypeEnum.INFO);
        }


        public void ServerClosing()
        {
                CloseServer?.Invoke(this, null);
                this.m_logging.Log("Closing server", Logging.Modal.MessageTypeEnum.INFO);
        }
    }
}