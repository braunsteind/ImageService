using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class CommandEventArgs : EventArgs
    {
        public CommandEnum Command { get; set; }
        public string Args { get; set; }

        public CommandEventArgs() { }

        public CommandEventArgs(CommandEnum command, string args)
        {
            this.Command = command;
            this.Args = args;
        }
    }
}