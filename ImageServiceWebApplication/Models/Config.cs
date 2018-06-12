using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceWebApplication.Communication;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWebApplication.Models
{
    public class Config
    {
        public delegate void NotifyChange();
        public event NotifyChange Notify;
        private static ICommunicationSingleton Communication { get; set; }


        public Config()
        {
            Communication = CommunicationSingleton.Instance;
            Communication.Read();
            Communication.InMessage += UpdateResponse;
            SourceName = "";
            LogName = "";
            OutputDirectory = "";
            ThumbnailSize = 1;
            Handlers = new ObservableCollection<string>();
            Enabled = false;
            string[] arr = new string[5];
            CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, "");
            Communication.Write(request);
        }

        public void DeleteHandler(string toBeDeleted)
        {
            try
            {
                string[] arr = { toBeDeleted };
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, arr, "");
                Communication.Write(eventArgs);
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateResponse(object sender, CommandRecievedEventArgs responseObj)
        {
            if (responseObj != null)
            {
                switch (responseObj.CommandID)
                {
                    case (int)CommandEnum.GetConfigCommand:
                        UpdateConfigurations(responseObj);
                        break;
                    case (int)CommandEnum.CloseHandler:
                        CloseHandler(responseObj);
                        break;
                }
                
                Notify?.Invoke();
            }
        }

        private void CloseHandler(CommandRecievedEventArgs responseObj)
        {
            if (Handlers != null && Handlers.Count > 0 && responseObj != null && responseObj.Args != null
                                 && Handlers.Contains(responseObj.Args[0]))
            {
                this.Handlers.Remove(responseObj.Args[0]);
            }
        }



        private void UpdateConfigurations(CommandRecievedEventArgs responseObj)
        {
            OutputDirectory = responseObj.Args[0];
            SourceName = responseObj.Args[1];
            LogName = responseObj.Args[2];
            int num;
            int.TryParse(responseObj.Args[3], out num);
            ThumbnailSize = num;
            string[] handlers = responseObj.Args[4].Split(';');
            foreach (string handler in handlers)
            {
                if (!Handlers.Contains(handler))
                {
                    Handlers.Add(handler);
                }
            }
        }

        //members
        [Required]
        [DataType(DataType.Text)]
        public bool Enabled { get; set; }

        [Required]
        [Display(Name = "Output Directory")]
        public string OutputDirectory { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [Display(Name = "Thumbnail Size")]
        public int ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; set; }
    }
}