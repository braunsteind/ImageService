using System;
using System.ComponentModel;
using ImageServiceGUI.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public ICommunicationSingleton communication;

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            communication = CommunicationSingleton.Instance;
            communication.InMessage += IncomingMessage;
            communication.Read();
        }

        private void IncomingMessage(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.Command == (int)CommandEnum.GetConfigCommand)
                {
                    SettingsDeserialize sd = JsonConvert.DeserializeObject<SettingsDeserialize>(e.Args);
                    this.OutputDirectory = sd.OutputDir;
                    this.SourceName = sd.SourceName;
                    this.LogName = sd.LogName;
                    this.TumbnailSize = sd.ThumbnailSize;
                    foreach (string handler in sd.Handlers)
                    {
                        this.Handlers.Add(handler);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string logName;
        public string LogName
        {
            get { return logName; }
            set
            {
                if (this.logName != value)
                {
                    this.logName = value;
                    this.NotifyPropertyChanged("LogName");
                }
            }
        }
        private string outputDirectory;
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set
            {
                if (this.outputDirectory != value)
                {
                    this.outputDirectory = value;
                    this.NotifyPropertyChanged("OutputDirectory");
                }
            }
        }
        private string sourceName;
        public string SourceName
        {
            get { return sourceName; }
            set
            {
                if (this.sourceName != value)
                {
                    this.sourceName = value;
                    this.NotifyPropertyChanged("SourceName");
                }
            }
        }
        private string thumbnailSize;
        public string TumbnailSize
        {
            get { return thumbnailSize; }
            set
            {
                if (this.thumbnailSize != value)
                {
                    this.thumbnailSize = value;
                    this.NotifyPropertyChanged("ThumbnailSize");
                }
            }
        }

        public ObservableCollection<string> Handlers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}