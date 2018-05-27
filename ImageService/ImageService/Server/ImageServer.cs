using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
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

        public delegate void UpdateClients(CommandRecievedEventArgs commandRecievedEventArgs);
        public static event UpdateClients NotifyAllHandlerRemoved;

        public Dictionary<string, IDirectoryHandler> HandlerPerPath { get; set; }
        public IImageController Controller { get { return this.m_controller; } }
        public ILoggingService Logging { get { return this.m_logging; } }
        #endregion


        /// <summary>
        /// Server constuctor
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="logging">The logging service</param>
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            //dictionary of folder path:relevant handler
            this.HandlerPerPath = new Dictionary<string, IDirectoryHandler>();
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
                try
                {
                    //create handler for every directory
                    this.CreateHandler(directory);
                }
                catch (Exception e)
                {
                    //logging a problem with creating handler, if needed
                    string error = "Error creating handler to: " + directory + ". Details:" + e.ToString();
                    this.m_logging.Log(error, MessageTypeEnum.FAIL);
                }
            }
        }

        /// <summary>
        /// creating handler for a specific directory
        /// </summary>
        /// <param name="path">The directory path</param>
        private void CreateHandler(string path)
        {
            //creating the handler
            IDirectoryHandler handler = new DirectoyHandler(m_logging, m_controller);
            //****************
            HandlerPerPath[path] = handler;
            //*****************
            CommandRecieved += handler.OnCommandRecieved;
            //register the handler to CloseServer event
            this.CloseServer += handler.StopHandler;
            handler.StartHandleDirectory(path);
            this.m_logging.Log("Handler was created for directory: " + path, MessageTypeEnum.INFO);
        }

        //************************************************************************************************
    
        public static void PerformSomeEvent(CommandRecievedEventArgs args)
        {
            NotifyAllHandlerRemoved.Invoke(args);
        }






        /// <summary>
        /// CloseSpecipicHandler function.
        /// closes specipic handler.
        /// </summary>
        /// <param name="toBeDeletedHandler">path of to be deleted handler</param>
        internal void CloseSpecipicHandler(string toBeDeletedHandler)
        {
            if (HandlerPerPath.ContainsKey(toBeDeletedHandler))
            {
                IDirectoryHandler handler = HandlerPerPath[toBeDeletedHandler];
                this.CloseServer -= handler.StopHandler;
                handler.StopHandler(this, null);
            }

        }
        //*****************************************************************************


    /// <summary>
    /// Dealing with server closing
    /// </summary>
    public void ServerClosing()
        {
            //close server event
            CloseServer?.Invoke(this, null);
            //write to log
            this.m_logging.Log("Closing server", MessageTypeEnum.INFO);
        }
    }
}