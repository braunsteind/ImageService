using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        /// <summary>
        /// Set & Get of directory path
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Set & Get of message
        /// </summary>
        public string Message { get; set; }             // The Message That goes to the logger

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dirPath"> directory name </param>
        /// <param name="message"> the message </param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}
