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
        /// Constructor
        /// </summary>
        /// <param name="modal">  </param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
                if (args.Length == 0)
                {
                    result = false;
                    return "Not enough arguments";
                }

                //else, arguments are fine, adding the image
                result = true;
                return m_modal.AddFile(args[0], out result);
        }
    }
}