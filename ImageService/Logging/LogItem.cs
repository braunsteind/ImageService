using ImageService.Logging.Modal;
using System;

namespace ImageService.Logging
{
    public class LogItem
    {
        public int Type { get; set; }

        public string Message { get; set; }

        public LogItem(int type, string message)
        {
            this.Type = type;
            this.Message = message;
        }
    }
}