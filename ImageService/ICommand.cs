using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// This function executes the command
        /// </summary>
        /// <param name="args"> the arguments </param>
        /// <param name="result"> indicates the result of executing the command </param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);          // The Function That will Execute The 
    }
}
