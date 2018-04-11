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

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory

        public DirectoyHandler(IImageController m_controller, ILoggingService m_logging)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
        }
        #endregion

        //The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            if (this.m_path.Equals(e.RequestDirPath) || e.RequestDirPath.Equals("*"))
            {
                bool result;

                //if close command
                if (e.CommandID == (int)CommandEnum.CloseCommand)
                {
                    //this.StopHandleDirectory();
                    return;
                }
                // execute the command
                string message = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                
                //write info to log
                if (result)
                {
                    m_logging.Log(message, MessageTypeEnum.INFO);
                }
                //write fail to log
                else
                {
                    m_logging.Log(message, MessageTypeEnum.FAIL);
                }
            }
        }

        public void StartHandleDirectory(string dirPath)
        {
            //set the path
            this.m_path = dirPath;
            //listen to all files in folder
            this.m_dirWatcher = new FileSystemWatcher(this.m_path, "*");
            this.m_dirWatcher.EnableRaisingEvents = true;

            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            /***********************************************************************************************************/
            //this.m_dirWatcher.Created += new FileSystemEventHandler();
        }

        // Implement Here!
    }
}