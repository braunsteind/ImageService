using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commends
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                string[] entries = new string[5];
                //extract needed info from App.config
                entries[0] = ConfigurationManager.AppSettings.Get("OutputDir");
                entries[1] = ConfigurationManager.AppSettings.Get("SourceName");
                entries[2] = ConfigurationManager.AppSettings.Get("LogName");
                entries[3] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
                entries[4] = ConfigurationManager.AppSettings.Get("Handler");
                CommandRecievedEventArgs commandSendArgs = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, entries, "");
                return JsonConvert.SerializeObject(commandSendArgs);
            }
            catch (Exception ex)
            {
                result = false;
                return ex.ToString();
            }
        }
    }
}
