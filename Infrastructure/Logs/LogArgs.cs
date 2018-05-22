using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logs
{
    public class LogArgs : EventArgs
    {
        public string logType { get; set; }      // The Command ID
        public string logInfo { get; set; }

        public LogArgs(string lType, string args)
        {
            logType = lType;
            logInfo = args;
        }
    }
}
