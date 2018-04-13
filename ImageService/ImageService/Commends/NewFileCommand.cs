using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {

        private IImageServiceModal m_modal;

        /// <summary>
        /// NewFileCommand Constructor
        /// </summary>
        /// <param name="modal"> the modal </param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// Execute a command
        /// </summary>
        /// <param name="args"> the command's argument </param>
        /// <param name="result"> result of executing the wanted command </param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            //return the result of trying to execute the command          
            return m_modal.AddFile(args[0], out result);
        }
    }
}