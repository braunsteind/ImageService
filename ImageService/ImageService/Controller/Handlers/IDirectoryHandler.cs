﻿using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        //The Event That Notifies that the Directory is being closed
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        //The Function Recieves the directory to Handle
        void StartHandleDirectory(string dirPath);
        
        //The Event that will be activated upon new Command
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);

        //Closing an handler
        void StopHandler(object sender, DirectoryCloseEventArgs e);
    }
}