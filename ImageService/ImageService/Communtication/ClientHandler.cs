using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        private ILoggingService m_logging;
        private IImageController m_controller;
        private List<TcpClient> m_activeClients;


        public ClientHandler(IImageController controler, ILoggingService logging)
        {
            m_logging = logging;
            m_controller = controler;
            m_activeClients = new List<TcpClient>();
        }

        public void HandleClient(TcpClient client)
        {
           // bool clientConnect = true;

            new Task(() =>
            {
                m_activeClients.Add(client);
             //   while (clientConnect)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (BinaryReader reader = new BinaryReader(stream))
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        m_logging.Log("waiting to client..", MessageTypeEnum.INFO);
                        ConfigInfomation cInfo = ConfigInfomation.Create();
                        string st = cInfo.ToJson();
                        string commandLine = reader.ReadString();
                        Console.WriteLine("Got command: {0}", commandLine);
                        m_logging.Log("ok " + commandLine, MessageTypeEnum.INFO);

                        //לשנות בהתאם לפורמט שכתבנו לקבלת פקודה
                        //string result = m_controller.ExecuteCommand((int)commandLine, null);
                        //                    string result = ExecuteCommand(commandLine, cl ient);
                        writer.Write("i got " + st);
                    }
                }
                client.Close();
                m_activeClients.Remove(client);
            }).Start();
        }
    }

}
