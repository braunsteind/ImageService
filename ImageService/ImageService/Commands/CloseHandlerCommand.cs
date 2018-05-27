using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Configuration;
using System.Text;

namespace ImageService.Commands
{
    class CloseHandlerCommand : ICommand
    {
        //members
        private ImageServer server;

        /// <summary>
        /// Consrtuctor
        /// </summary>
        /// <param name="imageServer"></param>
        public CloseHandlerCommand(ImageServer imageServer)
        {
            this.server = imageServer;
        }

        /// <summary>
        /// That function will execute the task of the command.
        /// </summary>
        /// <param name="args">arguments</param>
        /// <param name="result"> tells if the command succeded or not.</param>
        /// <returns>command return a string describes the operartion of the command.</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            try
            {
                string wantedHandler = args[0];
                string[] sources = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
                StringBuilder restOfHandlers = new StringBuilder();
                for (int i = 0; i < sources.Length; i++)
                {
                    if (sources[i] != wantedHandler)
                    {
                        restOfHandlers.Append(sources[i] + ";");
                    }
                }
                string newHandlers = (restOfHandlers.ToString()).TrimEnd(';');
                ConfigurationManager.AppSettings.Set("Handler", newHandlers);
                this.server.CloseHandler(wantedHandler);
                string[] info = { wantedHandler };
                CommandRecievedEventArgs closeArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, info, "");
                ImageServer.HandlerRemovalExecution(closeArgs);
                return string.Empty;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }

        }
    }
}
