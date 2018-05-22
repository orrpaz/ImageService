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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="log"></param>
        void AddToLog(object sender, MessageRecievedEventArgs log);
        string GetCurrentRunLog { get; }
        void ClearCurrentLog();
    }

}
