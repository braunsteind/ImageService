using ImageServiceGUI.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel settingsModel;
        public SettingsViewModel(ISettingsModel model)
        {
            this.settingsModel = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public string VM_LogName
        {
            get { return this.settingsModel.LogName; }
        }
        public string VM_OutputDirectory
        {
            get { return this.settingsModel.OutputDirectory; }
        }
        public string VM_SourceName
        {
            get { return this.settingsModel.SourceName; }
        }
        public string VM_TumbnailSize
        {
            get { return this.settingsModel.TumbnailSize; }
        }

        public ObservableCollection<string> VM_LbHandlers
        {
            get { return this.settingsModel.LbHandlers; }
        }
    }
}