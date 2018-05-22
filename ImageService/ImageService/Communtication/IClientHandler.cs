using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ImageService.Logging.Modal;
using ImageService.Modal;

namespace ImageService.Server
{
    public interface IClientHandler
    {
        /// <summary>
        /// Handle massage got by client
        /// </summary>
        void HandleClient(TcpClient client);
        /// <summary>
        /// Send clients new log
        /// </summary>
        void UpdateClientsNewLog(object sender, MessageRecievedEventArgs e);
        /// <summary>
        /// Send massage to client
        /// </summary>
        void SendToClient(string currentLog, CommandRecievedEventArgs e);
        /// <summary>
        /// Broadcast massage to all clients
        /// </summary>
        void broadcastAllClients(string serialized);
    }
}
