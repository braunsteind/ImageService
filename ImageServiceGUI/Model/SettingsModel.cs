using System;
using System.ComponentModel;
using ImageServiceGUI.Communication;
using ImageService.Infrastructure.Enums;
using System.Collections.ObjectModel;
using ImageService.Modal;
using System.Windows.Data;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public ICommunicationSingleton Communication { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsModel()
        {
            //get instance of communication
            this.Communication = CommunicationSingleton.Instance;
            //sign to InMessage
            this.Communication.Read();
            this.Communication.InMessage += UpdateSettings;
            //create list of handelrs
            this.LbHandlers = new ObservableCollection<string>();
            Object settingsLock = new Object();
            BindingOperations.EnableCollectionSynchronization(LbHandlers, settingsLock);
            //write GetConfigCommand to server
            string[] arr = new string[5];
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, "");
            this.Communication.Write(command);
        }

        /// <summary>
        /// Update the settings
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The command event args</param>
        private void UpdateSettings(object sender, CommandRecievedEventArgs e)
        {
            try
            {
                //if the command is GetConfigCommand
                if (e.CommandID == (int)CommandEnum.GetConfigCommand)
                {
                    //set the data
                    this.OutputDirectory = e.Args[0];
                    this.SourceName = e.Args[1];
                    this.LogName = e.Args[2];
                    this.ThumbnailSize = e.Args[3];
                    string[] handlers = e.Args[4].Split(';');
                    foreach (string handler in handlers)
                    {
                        this.LbHandlers.Add(handler);
                    }
                }
                //if the command is CloseHandler
                else if (e.CommandID == (int)CommandEnum.CloseHandler)
                {
                    //check data is ok and close
                    if (LbHandlers != null && LbHandlers.Count > 0 && e != null
                        && e.Args != null && LbHandlers.Contains(e.Args[0]))
                    {
                        this.LbHandlers.Remove(e.Args[0]);
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
        public string ThumbnailSize
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

        public ObservableCollection<string> LbHandlers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}