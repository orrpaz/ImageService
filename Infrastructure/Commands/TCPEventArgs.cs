using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class TCPEventArgs : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string Args { get; set; }
        
        public TCPEventArgs(int id, string args)
        {
            CommandID = id;
            Args = args;
        }
    }
}
