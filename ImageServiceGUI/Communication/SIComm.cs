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
    class SIComm
    {
        private static SIComm instance;
        private TcpClient client;
        private SIComm()
        {

        }

        public static SIComm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SIComm();
                }
                return instance;
            }
        }
        public void StartComm()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                 client = new TcpClient();
                client.Connect(ep);
                Console.WriteLine("You are connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void SendCommand(string jsonCommand)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {

            }
        }
    }
}
