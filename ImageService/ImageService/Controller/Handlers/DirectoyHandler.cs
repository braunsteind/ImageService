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
    /// Directory Handler. Implements IDirectoryHandler Interface.
    /// </summary>
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;                  // The logger
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private readonly string[] validExtensions =
            { ".jpg", ".png", ".gif", ".bmp" , ".JPG", ".PNG", ".GIF", ".BMP"};             //The only file types are relevant.
        #endregion

        #region Events
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    // The Event That Notifies that the Directory is being closed
        #endregion

        /// <summary>
        /// DirectoryHandler constructor. Initializes class memebrs.
        /// </summary>
        /// <param name="logging">IImageLogger</param>
        /// <param name="controller">The Image Processing Controller</param>
        /// <param name="path">The Path of directory</param>
        public DirectoyHandler(ILoggingService logging, IImageController controller ,string path)
        {
            this.m_logging = logging;
            this.m_controller = controller;
            this.m_path = path;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
        }

        /// <summary>
        ///  The meothod that will be activated upon new Command when the CommandRecived event will be invoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">arguments of CommandRecieved event.</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            // execute the command
            string msg = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            // write result msg to the event log.
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
        /// Operations to be done to start the directory handler.
        /// </summary>
        /// <param name="dirPath">The path of the directory.</param>
        public void StartHandleDirectory(string dirPath)
        {
            m_logging.Log("enter StartHandleDirectory" + " " + dirPath, MessageTypeEnum.INFO);
            // add all images in the directory to the output directory.
            this.m_dirWatcher.NotifyFilter = NotifyFilters.FileName;
            string[] filesInDirectory = Directory.GetFiles(m_path);
            this.m_dirWatcher.Created += new FileSystemEventHandler(M_dirWatcher_Created);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(M_dirWatcher_Created);
            //start listen to directory
            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("Start handle directory: " + dirPath, MessageTypeEnum.INFO);

        }

        /// <summary>
        /// Method to be activated when a file is added or changed in the directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Arguments of FileSystemEvent.</param>
        private void M_dirWatcher_Created(object sender, FileSystemEventArgs e)
        {
            this.m_logging.Log("Enterd M_durWatcher_Created with: " + e.FullPath, MessageTypeEnum.INFO);
            string extension = Path.GetExtension(e.FullPath);
            // check that the file is an image.
            if (this.validExtensions.Contains(extension))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandRecievedEventArgs =
                    new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, "");
                this.OnCommandRecieved(this, commandRecievedEventArgs);
            }


        }

        /// <summary>
        /// Closing the handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Arguments of DirectoryCloaseEvent.</param>
        public void OnCloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            try
            {
                // stop listen to directory.
                this.m_dirWatcher.EnableRaisingEvents = false;
                // remove OnCommandRecieved from the CommandRecived Event.
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