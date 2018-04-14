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
        /// Server constuctor
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logging"></param>
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
            //extract folders from App.config
            string[] foldersToListen = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            //loop on all folders
            foreach (string directory in foldersToListen)
            {
                    //create handler for every directory
                    this.CreateHandler(directory);
            }
        }

        /// <summary>
        /// creating handler for a specific directory
        /// </summary>
        /// <param name="path"></param>
        private void CreateHandler(string path)
        {
            //creating the handler
            IDirectoryHandler handler = new DirectoyHandler(m_logging, m_controller, path);
            CommandRecieved += handler.OnCommandRecieved;
            //register the handler to CloseServer event
            this.CloseServer += handler.CloseHandler;
            handler.StartHandleDirectory(path);
            this.m_logging.Log("Handler was created for directory: " + path, Logging.Modal.MessageTypeEnum.INFO);
        }

        /// <summary>
        /// Dealing with server closing
        /// </summary>
        public void ServerClosing()
        {
                CloseServer?.Invoke(this, null);
                this.m_logging.Log("Closing server", Logging.Modal.MessageTypeEnum.INFO);
        }
    }
}