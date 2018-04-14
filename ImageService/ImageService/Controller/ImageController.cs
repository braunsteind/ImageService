using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
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

        /// <summary>
        /// ImangeController constuctor
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;        //Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();         //creating the dictionary

            //For Now will contain NEW_FILE_COMMAND
            this.commands[((int)CommandEnum.NewFileCommand)] = new NewFileCommand(this.m_modal);

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
    }
}