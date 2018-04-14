using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        private MessageTypeEnum type;

        public MessageRecievedEventArgs(MessageTypeEnum type, string message)
        {
            this.type = type;
            Message = message;
        }

        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
