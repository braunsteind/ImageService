using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageService.Logging;
using ImageServiceGUI.Communication;
using ImageService.Infrastructure.Enums;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public ICommunicationSingleton Communication { get; set; }

        public ObservableCollection<LogItem> LogItems
        {
            get { return this.LogItems; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogModel()
        {
            this.Communication = CommunicationSingleton.Instance;
            this.Communication.InMessage += UpdateLogs();
            this.InitializeLogsParams();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateLogs(CommandEventArgs args)
        {
            if (args.Command == (int)CommandEnum.LogCommand)
            {
                IntializeLogEntriesList(responseObj);
            }
            else if (args.Command == (int)CommandEnum.AddLogItem) {
                AddLogEntry(responseObj);
            }
        }
    }
}