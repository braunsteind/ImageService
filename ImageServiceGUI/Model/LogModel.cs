using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageService.Logging;
using ImageServiceGUI.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System.Windows.Data;
using ImageService.Modal;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public ICommunicationSingleton Communication { get; set; }

        public ObservableCollection<LogItem> LogItems { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public LogModel()
        {
            //get communication instance
            this.Communication = CommunicationSingleton.Instance;
            //add update logs for InMessage
            Communication.InMessage += UpdateLogs;
            //set logs items list
            this.LogItems = new ObservableCollection<LogItem>();
            Object logLock = new Object();
            BindingOperations.EnableCollectionSynchronization(LogItems, logLock);
            //write log command to server
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            this.Communication.Write(command);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Update logs
        /// </summary>
        /// <param name="sender">Sender of the logs</param>
        /// <param name="args">The command event args</param>
        private void UpdateLogs(object sender, CommandRecievedEventArgs args)
        {
            //if coomand is log command
            if (args.CommandID == (int)CommandEnum.LogCommand)
            {
                try
                {
                    //add log items
                    foreach (LogItem log in JsonConvert.DeserializeObject<ObservableCollection<LogItem>>(args.Args[0]))
                    {
                        this.LogItems.Add(log);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            //if command is add log item
            else if (args.CommandID == (int)CommandEnum.AddLogItem)
            {
                try
                {
                    //add log item
                    LogItem logItem = new LogItem { Type = args.Args[0], Message = args.Args[1] };
                    this.LogItems.Insert(0, logItem);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}