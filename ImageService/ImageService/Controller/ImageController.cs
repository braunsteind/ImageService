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
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            //Storing the Modal Of The System
            m_modal = modal;
            commands = new Dictionary<int, ICommand>();

            //For Now will contain NEW_FILE_COMMAND
            this.commands[((int)CommandEnum.NewFileCommand)] = new NewFileCommand(this.m_modal);

        }

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