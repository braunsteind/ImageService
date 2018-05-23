using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// Get & Set for the status
        /// </summary>
        public MessageTypeEnum Status { get; set; }

        /// <summary>
        /// Get & Set fot the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type"> type of the message </param>
        /// <param name="message"> the message </param>
        public MessageRecievedEventArgs(MessageTypeEnum type, string message)
        {
            this.Status = type;
            this.Message = message;
        }
    }
}