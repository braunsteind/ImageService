using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;


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

        public delegate void UpdateClients(CommandRecievedEventArgs args);
        public static event UpdateClients UpdateOnRemovingHandler;

        public Dictionary<string, IDirectoryHandler> HandlerPerPath { get; set; }
        public IImageController Controller { get { return this.m_controller; } }
        public ILoggingService Logging { get { return this.m_logging; } }
        #endregion

        public string[] Directories { get; set; }

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
            Directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
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
            foreach (string directory in Directories)
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
            HandlerPerPath[path] = handler;
            CommandRecieved += handler.OnCommandRecieved;
            //register the handler to CloseServer event
            this.CloseServer += handler.StopHandler;
            handler.StartHandleDirectory(path);
            this.m_logging.Log("Handler was created for directory: " + path, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// Update removal.
        /// </summary>
        /// <param name="args"></param>
        public static void HandlerRemovalExecution(CommandRecievedEventArgs args)
        {
            UpdateOnRemovingHandler.Invoke(args);
        }


        /// <summary>
        /// Closing wanted handler.
        /// </summary>
        /// <param name="handler"></param>
        internal void CloseHandler(string path)
        {
            //making sure path the wanted path exists
            if (HandlerPerPath.ContainsKey(path))
            {
                IDirectoryHandler handler = HandlerPerPath[path];
                this.CloseServer -= handler.StopHandler;
                handler.StopHandler(this, null);
            }

        }


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