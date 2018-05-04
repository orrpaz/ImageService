using ImageService.Controller;
using sImageService.Server;
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

        public ClientHandler(IImageController controler)
        {
            m_controller = controler;
        }

        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string commandLine = reader.ReadLine();
                    Console.WriteLine("Got command: {0}", commandLine);

                    //לשנות בהתאם לפורמט שכתבנו לקבלת פקודה
                   // string result = m_controller.ExecuteCommand((int)commandLine, )
//                    string result = ExecuteCommand(commandLine, client);
                 //    writer.Write(result);
                }
                client.Close();
            }).Start();
        }
    }
}
