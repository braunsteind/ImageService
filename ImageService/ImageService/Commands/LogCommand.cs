using ImageService.Logging;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using ImageService.Logging.Modal;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private ILoggingService loggingService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ls"></param>
        public LogCommand(ILoggingService ls)
        {
            loggingService = ls;
            this.loggingService.Log("Successfully created LogCommand", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// This function executes the command
        /// </summary>
        /// <param name="args"> the arguments </param>
        /// <param name="result"> indicates the result of executing the command </param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            try
            {  
                //serializing logs and command
                ObservableCollection<LogItem> logItems = loggingService.LogItemCollection;
                string logsTojson = JsonConvert.SerializeObject(logItems);
                string[] info = { logsTojson };
                CommandRecievedEventArgs send = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, info, "");
                return JsonConvert.SerializeObject(send);
            }
            catch (Exception e)
            {
                result = false;
                string error = "Faiure occured inside LogCommand: " + e.Message;
                return error;
            }
        }
    }
}
