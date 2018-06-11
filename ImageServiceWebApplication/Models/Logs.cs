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

        
        private void InitializeLogsParams()
        {
            LogEntries = new List<Log>();
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            Communication.Write(commandRecievedEventArgs);
        }

        
        private void UpdateResponse(object sender, CommandRecievedEventArgs responseObj)
        {
            if (responseObj != null)
            {
                switch (responseObj.CommandID)
                {
                    case (int)CommandEnum.LogCommand:
                        foreach (LogItem log in JsonConvert.DeserializeObject<ObservableCollection<LogItem>>(responseObj.Args[0]))
                        {
                            LogEntries.Add(new Log { MessageType = log.Type, Message = log.Message });
                        }
                        break;
                    case (int)CommandEnum.AddLogItem:
                        LogItem newLogEntry = new LogItem { Type = responseObj.Args[0], Message = responseObj.Args[1] };
                        this.LogEntries.Insert(0, new Log { MessageType = newLogEntry.Type, Message = newLogEntry.Message });
                        break;
                    default:
                        break;
                }
                Notify?.Invoke();
            }
        }
    }
}