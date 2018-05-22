using Infrastructure.Commands;
using Infrastructure.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class Communicator
    {
        private const int port = 8000;
        private TcpClient m_client;
        private static Mutex m_mutexx = new Mutex();
        private static Communicator m_instance;
       // private bool connected = false;

        public event EventHandler<TCPEventArgs> GotTcpMessege;

        //Constructor
        private Communicator()
        {
            bool result;
            StartConnection(out result);
            connectionIsOn = result;
  //          connected = connectionIsOn;
        }
        /// <summary>
        /// Singelton
        /// </summary>

        public static Communicator Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new Communicator();
                }
                return m_instance;
            }
        }
        /// <summary>
        /// Connect to server
        /// </summary>
        public void StartConnection(out bool success)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                m_client = new TcpClient();
                m_client.Connect(ep);
                Console.WriteLine("You are connected");
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
        }
        /// <summary>
        /// Might not be used right now. Terminate connection
        /// </summary>

        public void EndConnection()
        {
            connectionIsOn = false;
            m_client.Close();
        }
        /// <summary>
        /// Indicator if connection to server is on
        /// </summary>

        public bool connectionIsOn
        {
            get;
            set;
        }

        /// <summary>
        /// Ask for appconfig from server
        /// </summary>

        public string getAppConfig()
        {
            try
            {
                NetworkStream stream = m_client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                TCPEventArgs commandTcp = new TCPEventArgs((int)CommandEnum.GetConfigCommand, null);

                string toSend = JsonConvert.SerializeObject(commandTcp);
                m_mutexx.WaitOne();
                writer.Write(toSend);
                m_mutexx.ReleaseMutex();
                string result2 = reader.ReadString();

                Console.WriteLine(result2);
                return result2;
            }
            catch (Exception)
            {
                return "Coldn't get app config";
            }
        }
        /// <summary>
        /// Send command to server
        /// </summary>
        /// <param name="commandTcp">The command</param>
        public void sendToServer(TCPEventArgs commandTcp)
        {
            new Task(() =>
            {
                try
                {
                    Console.WriteLine("In sendToServer " + commandTcp.CommandID);
                    NetworkStream stream = m_client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    //Serialize and Send To Server
                    string toSend = JsonConvert.SerializeObject(commandTcp);
                    m_mutexx.WaitOne();
                    writer.Write(toSend);
                    writer.Flush();
                    m_mutexx.ReleaseMutex();
                }
                catch (Exception) { }
            }).Start();
        }
        /// <summary>
        /// Get command from server
        /// </summary>

        public void getFromServer()
        {
            new Task(() =>
            {
                try
                {
                    Console.WriteLine("In getFromServer");
                    NetworkStream stream = m_client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    while (connectionIsOn)
                    {
                        try
                        {
                            string incomingMsg = reader.ReadString();
                            //if (string.Compare(incomingMsg, "close") == 0)
                            //{
                            //    EndConnection();
                            //    break;
                            //}
                            TCPEventArgs info = JsonConvert.DeserializeObject<TCPEventArgs>(incomingMsg);
                            Console.WriteLine("Got: " + info.CommandID);
                            Console.WriteLine("Got: " + info.Args);
                            GotTcpMessege?.Invoke(this, info);
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
                catch { }
            }).Start();
        }


    }

}
