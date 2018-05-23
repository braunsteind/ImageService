using System;

namespace ImageServiceGUI.Communication
{
    class CommandEventArgs : EventArgs
    {
        public int Command { get; set; }
        public string Args { get; set; }

        public CommandEventArgs() { }

        public CommandEventArgs(int command, string args)
        {
            this.Command = command;
            this.Args = args;
        }
    }
}