using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class Communicator
    {
        private TcpClient m_client;
        private const int port = 9999;

        public void StartConnection()
        {
            try
            {

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            m_client = new TcpClient();
            m_client.Connect(ep);
            Console.WriteLine("You are connected");
            using (NetworkStream stream = m_client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                Console.Write("Please enter a number: ");
                    Console.WriteLine("Check0");
                    int num = int.Parse(Console.ReadLine());
                    Console.WriteLine("Check 2");
                writer.Write(num);
                    Console.WriteLine("Check3");
                    // Get result from server
                    int result = reader.ReadInt32();
                Console.WriteLine("Result = {0}", result);
            }
            m_client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("blabla");
            }

        }

    }
}
