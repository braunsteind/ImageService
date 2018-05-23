using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    class LogItem
    {
        private MessageTypeEnum enumType;
        private string message;
        public string Type
        {
            get
            {
                return Enum.GetName(typeof(MessageTypeEnum), enumType);
            }

            set
            {
                enumType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value);
            }
        }
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}
