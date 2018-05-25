using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageService.Logging;
using ImageServiceGUI.Communication;
using ImageService.Infrastructure.Enums;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Data;

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
            this.Communication = CommunicationSingleton.Instance;
            Communication.InMessage += UpdateLogs;
            this.LogItems = new ObservableCollection<LogItem>();
            Object thisLock = new Object();
            BindingOperations.EnableCollectionSynchronization(LogItems, thisLock);
            CommandEventArgs command = new CommandEventArgs((int)CommandEnum.LogCommand, null);
            this.Communication.Write(command);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateLogs(object sender, CommandEventArgs args)
        {
            if (args.Command == (int)CommandEnum.LogCommand)
            {
                try
                {
                    //get logs
                    foreach (LogItem log in JsonConvert.DeserializeObject<ObservableCollection<LogItem>>(args.Args[0]))
                    {
                        this.LogItems.Add(log);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            else if (args.Command == (int)CommandEnum.AddLogItem)
            {
                try
                {
                    LogItem logItem = new LogItem { Type = args.Args[0], Message = args.Args[1] };
                    this.LogItems.Insert(0, logItem);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}