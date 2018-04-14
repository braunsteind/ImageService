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


    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        //The Image Processing Controller
        private IImageController m_controller;
        private ILoggingService m_logging;
        //The Watcher of the Dir
        private FileSystemWatcher m_dirWatcher;
        //The Path of directory
        private string m_path;
        //The only file types are relevant.
        private readonly string[] relevantFiles = { ".jpg", ".JPG", ".gif", ".GIF", ".bmp", ".BMP", ".png", ".PNG" };
        #endregion

        //The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logging">The logging</param>
        /// <param name="controller">The controller</param>
        public DirectoyHandler(ILoggingService logging, IImageController controller)
        {
            this.m_logging = logging;
            this.m_controller = controller;
        }


        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;

            //execute command and get data
            string data = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);

            //if execute succeeded
            if (result)
            {
                //write INFO data to log
                this.m_logging.Log(data, MessageTypeEnum.INFO);
            }
            else
            {
                //write FAIL data to log
                this.m_logging.Log(data, MessageTypeEnum.FAIL);
            }
        }

        public void StartHandleDirectory(string dirPath)
        {
            //set directory path
            this.m_path = dirPath;
            //set dirWatcher
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);

            //set notify filter to file name
            this.m_dirWatcher.NotifyFilter = NotifyFilters.FileName;
            this.m_dirWatcher.Created += new FileSystemEventHandler(M_dirWatcher_Created);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(M_dirWatcher_Created);
            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("Start handle directory: " + dirPath, MessageTypeEnum.INFO);
        }

        /*************************************************************************************/
        /*************************************************************************************/
        /*************************************************************************************/
        /*************************************************************************************/
        /*************************************************************************************/
        /*************************************************************************************/
        /*************************************************************************************/
        private void M_dirWatcher_Created(object sender, FileSystemEventArgs e)
        {
            this.m_logging.Log("Entered M_durWatcher_Created with: " + e.FullPath, MessageTypeEnum.INFO);
            string extension = Path.GetExtension(e.FullPath);

            if (this.relevantFiles.Contains(extension))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandRecievedEventArgs =
                    new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, "");
                this.OnCommandRecieved(this, commandRecievedEventArgs);
            }


        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            try
            {

                this.m_dirWatcher.EnableRaisingEvents = false;

                ((ImageServer)sender).CommandRecieved -= this.OnCommandRecieved;
                this.m_logging.Log("Succsess on closing handler of path " + this.m_path, MessageTypeEnum.INFO);
            }
            catch (Exception exception)
            {
                this.m_logging.Log("Error while trying to close handler of path " + this.m_path + " "
                    + exception.ToString(), MessageTypeEnum.FAIL);
            }
        }
    }
}