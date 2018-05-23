using ImageServiceGUI.Communication;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    class MainWindowModel : IMainWindowModel
    {
        /// <summary>
        /// Constructor.
        /// Get Status of connection
        /// </summary>
        public MainWindowModel()
        {
            Communication = CommunicationSingleton.Instance;
            IsConnected = Communication.IsConnected;
        }

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        public ICommunicationSingleton Communication { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}