﻿using System;
using System.ComponentModel;
using ImageServiceGUI.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public ICommunicationSingleton communication;

        public SettingsModel()
        {
            communication = CommunicationSingleton.Instance;
            communication.InMessage += IncomingMessage;
        }

        private void IncomingMessage(object sender, CommandEventArgs e)
        {
            if (e.Command == CommandEnum.GetConfigCommand)
            {
                try
                {
                    Console.WriteLine(e.Args);
                    SettingsArgs settingsArgs = JsonConvert.DeserializeObject<SettingsArgs>(e.Args);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}