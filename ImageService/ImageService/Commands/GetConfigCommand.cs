using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Configuration;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        /// <summary>
        /// This function executes the command
        /// </summary>
        /// <param name="args"> the arguments </param>
        /// <param name="result"> indicates the result of executing the command </param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                //extract relevant inforamtion
                string outputDir = ConfigurationManager.AppSettings.Get("OutputDir");
                string source = ConfigurationManager.AppSettings.Get("SourceName");
                string log = ConfigurationManager.AppSettings.Get("LogName");
                string thumb = ConfigurationManager.AppSettings.Get("ThumbnailSize");
                string handler = ConfigurationManager.AppSettings.Get("Handler");
                string[] appConfigInfo = { outputDir, source, log, thumb, handler };
                //serialize the info
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
