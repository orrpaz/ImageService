using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
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
using sImageService.Server;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private const int port = 8000;
        private TcpListener m_listener;
        private IClientHandler m_ch;
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="logging">logger</param>
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
            m_ch = new ClientHandler(m_controller, m_logging);
            // read from App config and put handlers in array of string.
             string[] directories = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach (string directoryPath in directories)
            {
                // create handler for each path. 
                CreateHandler(directoryPath);
            }
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
            SendCommand(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null));
        }

        /// <summary>
        ///  this method send command to all handlers by event.
        /// </summary>
        /// <param name="e">args for the event</param>
        public void SendCommand(CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

        public void Start()
        {
            Console.WriteLine("IN IMAGE SERVER");

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            m_listener = new TcpListener(ep);

            m_listener.Start();
            Console.WriteLine("Waiting for connections...");
            m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = m_listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        m_logging.Log("Got new connection!", MessageTypeEnum.INFO);
                        m_ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        m_logging.Log("Fail to establish new conection", MessageTypeEnum.FAIL);
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
                m_logging.Log("Server stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }
        public void Stop()
        {
            m_listener.Stop();
        }
    }
}