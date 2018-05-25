using ImageService.Commands;
using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    class MainWindowViewModel
    {
        private IMainWindowModel mainWindowModel;

        public MainWindowViewModel(IMainWindowModel model)
        {
            this.mainWindowModel = model;
            this.mainWindowModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            //this.CloseCommand = new DelegateCommand<object>(this.OnClose, this.CanClose);
        }
        public bool VM_IsConnected
        {
            get { return this.mainWindowModel.IsConnected; }
        }

        public ICommand CloseCommand { get; set; }


        private void OnClose(object obj)
        {
            this.mainWindowModel.Communication.Disconnect();
        }
        /// <summary>
        /// CanClose function.
        /// defines if user can close the window.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanClose(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}