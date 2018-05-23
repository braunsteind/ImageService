using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Writing events to log
        /// </summary>
        /// <param name="message"> the message to write </param>
        /// <param name="type"> the type of message to be written </param>
        void Log(string message, MessageTypeEnum type);           // Logging the Message

        ObservableCollection<LogItem> Logs { get; set; }
    }
}