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
        /// Taking care of message event
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Logging the message
        /// </summary>
        /// <param name="message"> the message to log </param>
        /// <param name="type"> the type of message </param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
