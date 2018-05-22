using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{

    public class GetCurrentRunLogCommand : ICommand
    {
        private ICurrentRunLog m_currentLog;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="cLog">The log from start of the service</param>
        public GetCurrentRunLogCommand(ICurrentRunLog cLog)
        {
            m_currentLog = cLog;
        }
        
        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                return m_currentLog.GetCurrentRunLog;
            } catch(Exception e)
            {
                result = false;
                return "Failed to get current log";
            }
        }
    }
}
