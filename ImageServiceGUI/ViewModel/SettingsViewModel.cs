using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string OutputDirectory
        {
            get { return this.settingsModel.OutputDirectory; }
        }
        public string SourceName
        {
            get { return this.settingsModel.SourceName; }
        }
        public string TumbnailSize
        {
            get { return this.settingsModel.TumbnailSize; }
        }
    }
}