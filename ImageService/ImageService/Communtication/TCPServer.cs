//using ImageService.Logging;
//using ImageService.Logging.Modal;
//using ImageService.Server;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace ImageService.Communtication
//{
//    class TCPServer
//    {
//        private int port;
//        private TcpListener listener;
//        private IClientHandler ch;
//        private ILoggingService m_logging;



//        public TCPServer(int port, IClientHandler ch, ILoggingService log)
//        {
//            this.port = port;
//            this.ch = ch;
//            this.m_logging = log;
//        }
//        public void Start()
//        {
//                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
//                listener = new TcpListener(ep);

//                listener.Start();
//                Console.WriteLine("Waiting for connections...");
//                m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

//                Task task = new Task(() =>
//                {
//                    while (true)
//                    {
//                        try
//                        {
//                            TcpClient client = listener.AcceptTcpClient();
//                            Console.WriteLine("Got new connection");
//                            m_logging.Log("Got new connection!", MessageTypeEnum.INFO);
//                            ch.HandleClient(client);
//                        }
//                        catch (SocketException)
//                        {
//                            m_logging.Log("Fail to establish new conection", MessageTypeEnum.FAIL);
//                            break;
//                        }
//                    }
//                    Console.WriteLine("Server stopped");
//                    m_logging.Log("Server stopped", MessageTypeEnum.INFO);
//                });
//            task.Start();
//        }
//        public void Stop()
//        {
//            listener.Stop();
//        }

//    }


//}
