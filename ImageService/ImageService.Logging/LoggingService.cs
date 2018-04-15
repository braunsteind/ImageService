using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// Event handling
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Logging function. logging a message
        /// </summary>
        /// <param name="message">The message to log </param>
        /// <param name="type">The type of message to log</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}