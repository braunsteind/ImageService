using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
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
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));

            foreach (string path in directories)
            {
                try
                {
                    this.CreateHandler(path);
                }
                catch (Exception ex)
                {
                    this.m_logging.Log("Error while creating handler for directory: " + path + " because:" + ex.ToString(), Logging.Modal.MessageTypeEnum.FAIL);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void CreateHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(m_logging, m_controller, path);
            CommandRecieved += handler.OnCommandRecieved;
            this.CloseServer += handler.OnCloseHandler;
            handler.StartHandleDirectory(path);
            this.m_logging.Log("Handler was created for directory: " + path, Logging.Modal.MessageTypeEnum.INFO);
        }


        /// <summary>
        /// 
        /// </summary>
        public void OnCloseServer()
        {
            try
            {
                m_logging.Log("Enter OnCloseServer", Logging.Modal.MessageTypeEnum.INFO);
                CloseServer?.Invoke(this, null);
                m_logging.Log("Leave OnCloseServer", Logging.Modal.MessageTypeEnum.INFO);
            }
            catch (Exception ex)
            {
                this.m_logging.Log("OnColeServer Exception: " + ex.ToString(), Logging.Modal.MessageTypeEnum.FAIL);
            }
        }

    }
}
// TO BE CHANGED!!