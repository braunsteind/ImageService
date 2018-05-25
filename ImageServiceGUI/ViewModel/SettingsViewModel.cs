using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceGUI.Communication;
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
        public SettingsViewModel(ISettingsModel model)
        {
            this.settingsModel = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            //this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
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

        public ICommand RemoveCommand { get; set; }
        /// <summary>
        /// OnRemove function.
        /// tells what will happen when we press Remove button.
        /// </summary>
        /// <param name="obj"></param>
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