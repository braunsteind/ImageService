using ImageService.Commands;
using ImageServiceGUI.Model;
using System;
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
        }
        public bool VM_IsConnected
        {
            get { return this.mainWindowModel.IsConnected; }
        }

        public ICommand CloseCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}