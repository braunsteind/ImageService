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
        //members
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Update;
        private static ICommunicationSingleton Communication;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Enteries")]
        public List<Log> LogItems { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Logs()
        {
            Communication = CommunicationSingleton.Instance;
            Communication.InMessage += UpdateAction;
            Communication.Read();
            LogItems = new List<Log>();
            CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            Communication.Write(args);
        }


        /// <summary>
        /// Update the logs list
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The args</param>
        private void UpdateAction(object sender, CommandRecievedEventArgs args)
        {
            if (args != null)
            {
                if (args.CommandID == (int)CommandEnum.LogCommand)
                {
                    foreach (LogItem log in JsonConvert.DeserializeObject<ObservableCollection<LogItem>>(args.Args[0]))
                    {
                        LogItems.Add(new Log { MessageType = log.Type, Message = log.Message });
                    }
                }
                else if (args.CommandID == (int)CommandEnum.AddLogItem)
                {
                    LogItem newLog = new LogItem { Type = args.Args[0], Message = args.Args[1] };
                    this.LogItems.Insert(0, new Log { MessageType = newLog.Type, Message = newLog.Message });
                }
                Update?.Invoke();
            }
        }
    }
}