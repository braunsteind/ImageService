using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;        //Storing the Modal Of The System
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
            Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() => {
                bool resultSuccesfulTemp;
                string message = this.commands[commandID].Execute(args, out resultSuccesfulTemp);
                return Tuple.Create(message, resultSuccesfulTemp);
            });
            task.Start();
            task.Wait();
            Tuple<string, bool> result = task.Result;
            resultSuccesful = result.Item2;
            return result.Item1;
        }


        public ImageServer Server
        {
            get
            {
                return m_imageServer;
            }
            set
            {
                this.m_imageServer = value;
                this.commands[((int)CommandEnum.CloseHandler)] = new CloseHandlerCommand(m_imageServer);

            }
        }
    }
}