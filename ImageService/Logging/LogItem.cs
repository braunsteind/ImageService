using ImageService.Logging.Modal;
using System;

namespace ImageService.Logging
{
    public class LogItem
    {
        private MessageTypeEnum enumType;
        public string Type
        {
            get { return Enum.GetName(typeof(MessageTypeEnum), enumType); }

            set { enumType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}