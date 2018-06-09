using ImageService.Logging.Modal;
using Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Add Log to message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="type">param</param>
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}
