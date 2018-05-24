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

        private ImageServer m_imageServer;

        public CloseHandlerCommand(ImageServer imageServer)
        {
            this.m_imageServer = imageServer;

        }

        /// <summary>
        /// That function will execute the task of the command.
        /// </summary>
        /// <param name="args">arguments</param>
        /// <param name="result"> tells if the command succeded or not.</param>
        /// <returns>command return a string describes the operartion of the command.</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                if (args == null || args.Length == 0)
                {
                    throw new Exception("Invalid args for deleting handler");
                }
                string toBeDeletedHandler = args[0];
                string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
                StringBuilder sbNewHandlers = new StringBuilder();
                for (int i = 0; i < directories.Length; i++)
                {
                    if (directories[i] != toBeDeletedHandler)
                    {
                        sbNewHandlers.Append(directories[i] + ";");
                    }
                }
                string newHandlers = (sbNewHandlers.ToString()).TrimEnd(';');
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // Add an Application Setting.
                config.AppSettings.Settings.Remove("Handler");
                config.AppSettings.Settings.Add("Handler", newHandlers);
                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);
                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
                this.m_imageServer.CloseSpecipicHandler(toBeDeletedHandler);
                string[] array = new string[1];
                array[0] = toBeDeletedHandler;
                CommandRecievedEventArgs notifyParams = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, array, "");
                ImageServer.PerformSomeEvent(notifyParams);
                return string.Empty;
            }
            catch (Exception ex)
            {
                result = false;
                return ex.ToString();
            }

        }
    }
}
