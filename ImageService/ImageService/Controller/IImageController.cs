using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"> ID of the command</param>
        /// <param name="args"> relevant arguments </param>
        /// <param name="result"> inidication whether or not command succeeded </param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}