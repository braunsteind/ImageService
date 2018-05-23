using ImageServiceGUI.Communication;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    interface IMainWindowModel : INotifyPropertyChanged
    {
        bool IsConnected { get; set; }
        ICommunicationSingleton Communication { get; set; }
    }
}