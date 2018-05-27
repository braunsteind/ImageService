using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Configuration;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                string outputDir = ConfigurationManager.AppSettings.Get("OutputDir");
                string source = ConfigurationManager.AppSettings.Get("SourceName");
                string log = ConfigurationManager.AppSettings.Get("LogName");
                string thumb = ConfigurationManager.AppSettings.Get("ThumbnailSize");
                string handler = ConfigurationManager.AppSettings.Get("Handler");
                string[] appConfigInfo = { outputDir, source, log, thumb, handler };
                CommandRecievedEventArgs toSend = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, appConfigInfo, "");
                result = true;
                return JsonConvert.SerializeObject(toSend);
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
