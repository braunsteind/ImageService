using System;

namespace ImageServiceGUI.Communication
{
    interface ICommunicationSingleton
    {
        event EventHandler<CommandEventArgs> InMessage;
        bool IsConnected { get; set; }
        void Connect(string ip, int port);
        void Write(string command);
        void Disconnect();
    }
}