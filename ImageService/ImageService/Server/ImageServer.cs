using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ImageService.Server;
using Infrastructure.Enum;
using ImageService.Communtication;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private const int port = 8000;
        private IClientHandler m_ch;
        private IImageController m_controller;
        private ILoggingService m_logging;
        private ICurrentRunLog m_currentLog;
        private ITCPServer m_tcpserver;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="logging">logger</param>
        public ImageServer(IImageController controller, ILoggingService logging, ICurrentRunLog currentLog)
        {
            m_controller = controller;
            m_logging = logging;
            m_currentLog = currentLog;
            m_ch = new ClientHandler(m_controller, m_logging);
            m_tcpserver = new TCPServer(port, m_ch, m_logging);
            // read from App config and put handlers in array of string.
            // string[] directories = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach (string directoryPath in ConfigInfomation.Instance.handlerPaths)
            {
                // create handler for each path. 
                CreateHandler(directoryPath);
            }
            m_controller.SpecialCommanndAppeared += SendCommand;
           // m_controller.SpecialCommanndAppeared += PassLog;
           // m_logging.MessageRecieved += m_ch.UpdateClientsNewLog;
        }

        /// <summary>
        /// this method create handle.
        /// </summary>
        /// <param name="pathDirectory">path to the directory</param>
        public void CreateHandler(string pathDirectory)
        {
            IDirectoryHandler handler = new DirectoyHandler(pathDirectory, m_controller, m_logging);
            CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += RemoveHandler;
            handler.StartHandleDirectory(pathDirectory);
        }

        /// <summary>
        /// this method remove handler.
        /// </summary>
        /// <param name="source">object that send the event</param>
        /// <param name="e">args for the event</param>
        public void RemoveHandler(object source, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler handler = (IDirectoryHandler)source;
            CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= RemoveHandler;
            m_logging.Log(e.Message, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// this method send to handlers that the server was closed.
        /// </summary>
        public void CloseServer()
        {
            //Stop the listening
            Stop();
            SendCommand(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, "$Terminate$"));
        }

        /// <summary>
        ///  this method send command to all handlers by event.
        /// </summary>
        /// <param name="e">args for the event</param>
        public void SendCommand(object sender, CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(sender, e);
        }
        /// <summary>
        ///  Not in use right now. Avaliable for future to pass log by event
        /// </summary>
        /// <param name="e">args for the event</param>
        public void PassLog(object sender, CommandRecievedEventArgs e)
        {
            m_ch.SendToClient(m_currentLog.GetCurrentRunLog, e);
        }

        /// <summary>
        ///  Starts listen to incoming clients
        /// </summary>
        public void Start()
        {

            m_tcpserver.Start();
        }
        /// <summary>
        ///  Stop listen to incoming clients
        /// </summary>
        public void Stop()
        {
            m_tcpserver.Stop();
        }
    }
}