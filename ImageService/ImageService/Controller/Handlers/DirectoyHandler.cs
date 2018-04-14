using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using ImageService.Server;

namespace ImageService.Controller.Handlers
{
   
    /// <summary>
    /// 
    /// </summary>
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;                  
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private readonly string[] relevantFiles =
            {".jpg", ".JPG", ".gif", ".GIF" , ".bmp", ".BMP", ".png", ".PNG"};             //The only file types are relevant.
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    // The Event That Notifies that the Directory is being closed

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="controller"></param>
        /// <param name="path"></param>
        public DirectoyHandler(ILoggingService logging, IImageController controller ,string path)
        {
            this.m_logging = logging;
            this.m_controller = controller;
            this.m_path = path;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;

            string msg = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);

            if (result)
            {

                this.m_logging.Log(msg, MessageTypeEnum.INFO);
            }
            else
            {
                this.m_logging.Log(msg, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        public void StartHandleDirectory(string dirPath)
        {
            m_logging.Log("enter StartHandleDirectory" + " " + dirPath, MessageTypeEnum.INFO);

            this.m_dirWatcher.NotifyFilter = NotifyFilters.FileName;
            string[] filesInDirectory = Directory.GetFiles(m_path);
            this.m_dirWatcher.Created += new FileSystemEventHandler(M_dirWatcher_Created);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(M_dirWatcher_Created);

            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("Start handle directory: " + dirPath, MessageTypeEnum.INFO);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_dirWatcher_Created(object sender, FileSystemEventArgs e)
        {
            this.m_logging.Log("Enterd M_durWatcher_Created with: " + e.FullPath, MessageTypeEnum.INFO);
            string extension = Path.GetExtension(e.FullPath);

            if (this.relevantFiles.Contains(extension))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandRecievedEventArgs =
                    new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, "");
                this.OnCommandRecieved(this, commandRecievedEventArgs);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            try
            {

                this.m_dirWatcher.EnableRaisingEvents = false;

                ((ImageServer)sender).CommandRecieved -= this.OnCommandRecieved;
                this.m_logging.Log("Succsess on closing handler of path " + this.m_path, MessageTypeEnum.INFO);
            }
            catch (Exception ex)
            {
                this.m_logging.Log("Error while trying to close handler of path " + this.m_path + " "
                    + ex.ToString(), MessageTypeEnum.FAIL);
            }
        }

    }
}