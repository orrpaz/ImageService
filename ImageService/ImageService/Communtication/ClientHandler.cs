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
        private IImageController m_controller;
        private ILoggingService m_logging;
        public ClientHandler(IImageController controler, ILoggingService log)
        {
            m_controller = controler;
            m_logging = log;
        }

        public void HandleClient(TcpClient client)
        {
            m_logging.Log("In client handler", MessageTypeEnum.INFO);
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    m_logging.Log("Check 2", MessageTypeEnum.INFO);
                    string commandLine = reader.ReadLine();
                    Console.WriteLine("Got command: {0}", commandLine);
                    m_logging.Log("Got command", MessageTypeEnum.INFO);
                    m_logging.Log(commandLine, MessageTypeEnum.INFO);
                    //לשנות בהתאם לפורמט שכתבנו לקבלת פקודה
                    //string result = m_controller.ExecuteCommand((int)commandLine, null);
                    //                    string result = ExecuteCommand(commandLine, cl ient);
                    writer.Write("I got");
                }
                client.Close();
            }).Start();
        }
    }

}
