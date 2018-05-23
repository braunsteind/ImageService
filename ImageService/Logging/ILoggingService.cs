
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public delegate void UpdateLogEntry(CommandRecievedEventArgs updateObj);

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

        ObservableCollection<LogItem> LogMessages { get; set; }   //log entries list
        event UpdateLogEntry UpdateLogEntries;  //Invoked everytime a new event log entry is written to the log
        void InvokeUpdateEvent(string message, MessageTypeEnum type); // Invokes UpdateLogEntries event.
    }
}