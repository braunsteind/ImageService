using ImageService.Logging.Modal;
using System;

namespace ImageService.Logging
{
    /// <summary>
    /// Class for storing together log messages and their TypeEnum
    /// </summary>
    public class LogItem
    {
        //members
        private string message;
        private MessageTypeEnum enumType;
        //properties
        public string Type
        {
            get { return Enum.GetName(typeof(MessageTypeEnum), enumType); }

            set { enumType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value); }
        }

        
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}