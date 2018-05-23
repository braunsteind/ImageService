using System;

namespace ImageServiceGUI.Communication
{
    interface ICommunicationSingleton
    {
        event EventHandler<CommandEventArgs> InMessage;
        bool IsConnected { get; set; }
        void Write(string command);
        void Read();
        void Disconnect();
    }
}