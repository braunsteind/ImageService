using ImageService.Commands;
using ImageServiceGUI.Model;
using Prism.Commands;
using System;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    class MainWindowViewModel
    {
        private IMainWindowModel mainWindowModel;
        public ICommand CloseCommand { get; set; }

        public MainWindowViewModel(IMainWindowModel model)
        {
            this.mainWindowModel = model;
            this.mainWindowModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            //close command
            this.CloseCommand = new DelegateCommand<object>(this.OnClose, this.CanClose);
        }
        public bool VM_IsConnected
        {
            get { return this.mainWindowModel.IsConnected; }
        }

        private void OnClose(object obj)
        {
            this.mainWindowModel.Communication.Disconnect();
        }

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