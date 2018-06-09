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

namespace Communication
{
    public class WebCommunicator
    {
        private const int port = 8000;
        private TcpClient m_client;
        private static Mutex m_mutexx = new Mutex();
        private static WebCommunicator m_instance;
        private int serviceIsOn;
       

        public event EventHandler<TCPEventArgs> GotTcpMessege;
        public event EventHandler<TCPEventArgs> GotCloseHandlerCommand;
        public event EventHandler<TCPEventArgs> GotConfigCommand;
        public event EventHandler<TCPEventArgs> GotLogCommand;

        private WebCommunicator()
        {
            bool result;
            StartConnection(out result);
            connectionIsOn = result;
            if (result)
                serviceIsOn = 1;
            else
                serviceIsOn = 0;
        }
        /// <summary>
        /// Singelton
        /// </summary>

        public static WebCommunicator Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new WebCommunicator();
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
                serviceIsOn = 1;
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
        /// Indicator if connection to server is on
        /// </summary>

        public int ServiceIsOn
        {
            get { return serviceIsOn; }
        }


        /// <summary>
        /// Send command to server
        /// </summary>
        /// <param name="commandTcp">The command</param>
        public void sendToServer(TCPEventArgs commandTcp)
        {
            if(!connectionIsOn)
            {
                bool result;
                StartConnection(out result);
                connectionIsOn = result;
                if (!connectionIsOn)
                    return;
            }
            try
            {
                NetworkStream stream = m_client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryReader reader = new BinaryReader(stream);
                //Serialize and Send To Server
                string toSend = JsonConvert.SerializeObject(commandTcp);
                m_mutexx.WaitOne();
                writer.Write(toSend);
                writer.Flush();
                m_mutexx.ReleaseMutex();
                //Get from server
                string incomingMsg = reader.ReadString();
                TCPEventArgs info = JsonConvert.DeserializeObject<TCPEventArgs>(incomingMsg);
                switch(info.CommandID)
                {
                    case (int)CommandEnum.CloseCommand:
                        GotCloseHandlerCommand?.Invoke(this, info);
                        break;
                    case (int)CommandEnum.GetConfigCommand:
                        GotConfigCommand?.Invoke(this, info);
                        break;
                    case (int)CommandEnum.LogCommand:
                        GotLogCommand?.Invoke(this, info);
                        break;
                    default:
                        break;
                }

                EndConnection();

            }
                catch (Exception) { serviceIsOn = 0; }
        }
        /// <summary>
        /// Get command from server
        /// </summary>

        public void getFromServer()
        {
                try
                {
                    Console.WriteLine("In getFromServer");
                    NetworkStream stream = m_client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                        try
                        {
                            string incomingMsg = reader.ReadString();
                            TCPEventArgs info = JsonConvert.DeserializeObject<TCPEventArgs>(incomingMsg);
                            Console.WriteLine("Got: " + info.CommandID);
                            Console.WriteLine("Got: " + info.Args);
                            GotTcpMessege?.Invoke(this, info);
                        }
                        catch (Exception)
                        {
                        }
                }
                catch { }
        }

    }


}
