using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceWebApplication.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWebApplication.Models
{
    public class Config
    {
        public delegate void PropertyChanged();
        public event PropertyChanged propertyChanged;
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
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, "");
            Communication.Write(command);
        }

        public void DeleteHandler(string delete)
        {
            string[] arr = { delete };
            CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, arr, "");
            Communication.Write(args);
        }

        private void UpdateResponse(object sender, CommandRecievedEventArgs args)
        {
            if (args != null)
            {
                if (args.CommandID == (int)CommandEnum.GetConfigCommand)
                {
                    UpdateConfigurations(args);
                }
                else if (args.CommandID == (int)CommandEnum.CloseHandler)
                {
                    CloseHandler(args);
                }
                propertyChanged?.Invoke();
            }
        }

        private void CloseHandler(CommandRecievedEventArgs args)
        {
            if (Handlers != null && Handlers.Count > 0 && args != null && args.Args != null
                                 && Handlers.Contains(args.Args[0]))
            {
                this.Handlers.Remove(args.Args[0]);
            }
        }

        private void UpdateConfigurations(CommandRecievedEventArgs e)
        {
            OutputDirectory = e.Args[0];
            SourceName = e.Args[1];
            LogName = e.Args[2];
            int thumb;
            int.TryParse(e.Args[3], out thumb);
            ThumbnailSize = thumb;
            string[] handlers = e.Args[4].Split(';');
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

        public ObservableCollection<string> Handlers { get; set; }
    }
}