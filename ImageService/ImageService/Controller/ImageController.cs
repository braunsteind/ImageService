using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {

        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;              //num-command dictionary
        private ImageServer m_imageServer;
        private ILoggingService m_loggingService;


        /// <summary>
        /// ImangeController constuctor
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal, ILoggingService loggingService)
        {
            m_modal = modal;        //Storing the Modal Of The System
            m_loggingService = loggingService;
            commands = new Dictionary<int, ICommand>();         //creating the dictionary

            //Adding commands to dictionary
            this.commands[((int)CommandEnum.NewFileCommand)] = new NewFileCommand(this.m_modal);
            this.commands[((int)CommandEnum.GetConfigCommand)] = new GetConfigCommand();
            this.commands[((int)CommandEnum.LogCommand)] = new LogCommand(this.m_loggingService);
        }

        /// <summary>
        /// Executing command.
        /// Returns whether or not command succeeded
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="resultSuccesful"></param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            bool temp;
            //run the command
            string message = this.commands[commandID].Execute(args, out temp);
            //set result
            resultSuccesful = temp;
            //return message
            return message;
        }

        public ImageServer Server
        {
            get { return m_imageServer; }
            set
            {
                this.m_imageServer = value;
                this.commands[((int)CommandEnum.CloseHandler)] = new CloseHandlerCommand(m_imageServer);
            }
        }
    }
}