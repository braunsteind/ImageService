using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceGUI.Model;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel settingsModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">The model</param>
        public SettingsViewModel(ISettingsModel model)
        {
            this.settingsModel = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            try
            {
                this.RemoveCommand = new DelegateCommand<object>(OnRemove, CanRemove);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
        public string VM_ThumbnailSize
        {
            get { return this.settingsModel.ThumbnailSize; }
        }

        public ObservableCollection<string> VM_LbHandlers
        {
            get { return this.settingsModel.LbHandlers; }
        }

        private string selectedItem;
        public string SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                selectedItem = value;
                var command = this.RemoveCommand as DelegateCommand<object>;
                if (command != null)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand<object> RemoveCommand { get; set; }

        /// <summary>
        /// Check if any item was selected for remove
        /// </summary>
        /// <returns>True if selected, False if not</returns>
        private bool CanRemove(object obj)
        {
            if (this.selectedItem != null)
            {
                return true;
            }
            return false;
        }

        private void OnRemove(object obj)
        {
            try
            {
                string[] arr = { this.selectedItem };
                CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, arr, "");
                this.settingsModel.Communication.Write(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}