using ImageService.Logging;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private ILoggingService logging;

        public LogCommand(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                ObservableCollection<LogItem> logMessages = logging.LogMessages;
                
                string jsonLogMessages = JsonConvert.SerializeObject(logMessages);
                string[] arr = new string[1];
                arr[0] = jsonLogMessages;
                CommandRecievedEventArgs commandSendArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, arr, "");
                result = true;
                
                return JsonConvert.SerializeObject(commandSendArgs);
            }
            catch (Exception e)
            {
                result = false;
                return "LogCommand.Execute: Failed execute log command";
            }
        }
    }
}
