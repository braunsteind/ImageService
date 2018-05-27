using ImageService.Logging.Modal;
using System;
using System.Collections.ObjectModel;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.Diagnostics;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        private ObservableCollection<LogItem> logs;
        /// <summary>
        /// MessageRecieved event.
        /// in charge of wriiting message to log
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public event UpdateLogEntry UpdateLogItems;

        //properties
        public ObservableCollection<LogItem> LogItemCollection
        {
            get { return this.logs; }
            set { throw new NotImplementedException(); }
        }
        
        /// <summary>
        /// Constructor.
        /// Upon constuction, import logs
        /// </summary>
        /// <param name="eventLog"></param>
        public LoggingService(EventLog eventLog)
        {
            //create an empty collection of LogItems
            this.logs = new ObservableCollection<LogItem>();
            int totalLogs = eventLog.Entries.Count;
            EventLogEntry[] logs = new EventLogEntry[totalLogs];
            eventLog.Entries.CopyTo(logs, 0);
            //add all logs to the list, in LogItem format
            foreach (EventLogEntry logEntry in logs)
            {
                string msg = logEntry.Message;
                this.LogItemCollection.Insert(0, new LogItem
                {
                    Type = Enum.GetName(typeof(MessageTypeEnum), LoggingService.EventTypeToEnum(logEntry.EntryType)),
                    Message = msg
                });
            }
        }


        private static MessageTypeEnum EventTypeToEnum(EventLogEntryType type)
        {
            switch (type)
            {
                case EventLogEntryType.Warning:
                    return MessageTypeEnum.WARNING;
                case EventLogEntryType.Information:
                    return MessageTypeEnum.INFO;
                case EventLogEntryType.Error:
                default:
                    return MessageTypeEnum.FAIL;
            }
        }


        /// <summary>
        /// Logging function. logging a message
        /// </summary>
        /// <param name="message">The message to log </param>
        /// <param name="type">The type of message to log</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(type, message));
            //create new LogItem
            LogItem newLog = new LogItem { Type = Enum.GetName(typeof(MessageTypeEnum), type), Message = message };
            //add the created item to the list
            this.LogItemCollection.Insert(0, newLog);

            EventUpdate(message, type);
        }


        public void EventUpdate(string message, MessageTypeEnum type)
        {
            LogItem logItem = new LogItem { Type = Enum.GetName(typeof(MessageTypeEnum), type), Message = message };
            string[] logParts = {logItem.Type, logItem.Message};
            //add the log with the above information
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.AddLogItem, logParts, null);
            if (this.UpdateLogItems != null)
            {
                UpdateLogItems?.Invoke(command);

            }
        }

    }
}