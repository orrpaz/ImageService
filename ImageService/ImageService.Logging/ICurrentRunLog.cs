using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ICurrentRunLog
    {
        /// <summary>
        /// Add new log to 'current log' list
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="log">log message</param>
        void AddToLog(object sender, MessageRecievedEventArgs log);
        /// <summary>
        /// Property
        /// </summary>
        string GetCurrentRunLog { get; }
        /// <summary>
        /// Clear the current log.
        /// </summary>
        void ClearCurrentLog();
    }

}
