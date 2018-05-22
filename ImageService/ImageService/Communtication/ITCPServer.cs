using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communtication
{
    public interface ITCPServer
    {
        /// <summary>
        /// Start the server, to listen to clients
        /// </summary>
        void Start();
        /// <summary>
        /// Stop listen to server
        /// </summary>
        void Stop();
    }
}
