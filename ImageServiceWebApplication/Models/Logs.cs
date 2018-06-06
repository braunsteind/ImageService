using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageServiceWebApplication.Communication;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWebApplication.Models
{
    public class Logs
    {
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;
        private static ICommunicationSingleton Communication;
        /// <summary>
        /// LogCollection constructor.
        /// initialize new Log list.
        /// </summary>
        public Logs()
        {
            Communication = CommunicationSingleton.Instance;
            Communication.InMessage += UpdateResponse;
            Communication.Read();
            this.InitializeLogsParams();
        }

        //members
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Enteries")]
        public List<Log> LogEntries { get; set; }

        /// <summary>
        /// retreive event log entries list from the image service.
        /// </summary>
        private void InitializeLogsParams()
        {
            LogEntries = new List<Log>();
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            Communication.Write(commandRecievedEventArgs);
        }

        /// <summary>
        /// get CommandRecievedEventArgs object which was sent from the image service.
        /// reacts only if the commandID is relevant to the log model.
        /// </summary>
        /// <param name="responseObj"></param>
        private void UpdateResponse(object sender, CommandRecievedEventArgs responseObj)
        {
            if (responseObj != null)
            {
                switch (responseObj.CommandID)
                {
                    case (int)CommandEnum.LogCommand:
                        IntializeLogEntriesList(responseObj);
                        break;
                    case (int)CommandEnum.AddLogItem:
                        AddLogEntry(responseObj);
                        break;
                    default:
                        break;
                }
                Notify?.Invoke();
            }
        }

        /// <summary>
        /// Initialize log event entries list.
        /// </summary>
        /// <param name="responseObj">expected json string of ObservableCollection<LogEntry> in responseObj.Args[0]</param>
        private void IntializeLogEntriesList(CommandRecievedEventArgs responseObj)
        {
            foreach (LogItem log in JsonConvert.DeserializeObject<ObservableCollection<LogItem>>(responseObj.Args[0]))
            {
                LogEntries.Add(new Log { MessageType = log.Type, Message = log.Message });
            }
        }

        /// <summary>
        /// adds new log entry to the event log entries list
        /// </summary>
        /// <param name="responseObj">expected responseObj.Args[0] = EntryType,  responseObj.Args[1] = Message </param>
        private void AddLogEntry(CommandRecievedEventArgs responseObj)
        {
            LogItem newLogEntry = new LogItem { Type = responseObj.Args[0], Message = responseObj.Args[1] };
            this.LogEntries.Insert(0, new Log { MessageType = newLogEntry.Type, Message = newLogEntry.Message });
        }
    }
}