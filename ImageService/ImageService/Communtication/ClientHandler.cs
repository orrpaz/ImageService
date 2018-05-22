using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Infrastructure.Commands;
using Infrastructure.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        /// <summary>
        /// Internal class that holds info about client
        /// </summary>
        private class ClientInformation
        {
            public NetworkStream stream { get; set; }
            public BinaryReader reader { get; set; }
            public BinaryWriter writer { get; set; }
            public TcpClient client { get; set; }
            public ClientInformation(NetworkStream s, BinaryReader r, BinaryWriter w, TcpClient c)
            {
                stream = s;
                reader = r;
                writer = w;
                client = c;
            }
        }
        private ILoggingService m_logging;
        private IImageController m_controller;
        private List<ClientInformation> m_allClients;
        private static Mutex m_mutex = new Mutex();
        /// <summary>
        /// Constructor
        /// </summary>
        public ClientHandler(IImageController controler, ILoggingService logging)
        {
            m_logging = logging;
            m_controller = controler;
            m_allClients = new List<ClientInformation>();
        }

        /// <summary>
        /// Not in use right now. Send current log and command
        /// </summary>
        public void SendToClient(string currentLog, CommandRecievedEventArgs e)
        {
            new Task(() =>
            {
                try
            {

            string clientParams = e.Args[0];
            string args = e.Args[1];
            ClientInformation cInfo  = JsonConvert.DeserializeObject<ClientInformation>(e.Args[0]);

            TcpClient client = cInfo.client;
            NetworkStream stream = cInfo.stream;
            BinaryReader reader = cInfo.reader;
            BinaryWriter writer = cInfo.writer;

            m_mutex.WaitOne();
            writer.Write(currentLog);
            m_mutex.ReleaseMutex(); 
            }
            catch
            {
                m_logging.Log("FAIL SPECIAL FUNC", MessageTypeEnum.FAIL);

            }
            }).Start();

        }

        /// <summary>
        /// Handle a massage got from the client, and respone appropriately
        /// </summary>
        /// <param name="client"> the client</param>

        public void HandleClient(TcpClient client)
        {

            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                ClientInformation clientInfo = new ClientInformation(stream, reader, writer, client);
                m_allClients.Add(clientInfo);

                try
                {
                    while (true)
                    {
                        //Read command from client and Derserialize it
                        TCPEventArgs e = JsonConvert.DeserializeObject<TCPEventArgs>(reader.ReadString());
                        TCPEventArgs toSend;
                        string[] args = { e.Args };

                        bool success;
                        string result = m_controller.ExecuteCommand(e.CommandID, args, out success);
                        if (!success)
                        {
                            toSend = new TCPEventArgs((int)CommandEnum.ErrorOccurred, null);
                        }
                        else
                        {
                            toSend = new TCPEventArgs(e.CommandID, result);
                            //   string aww = Enum.GetName(typeof(CommandEnum), e.CommandID);
                            // m_logging.Log("Success in " + aww, MessageTypeEnum.INFO);
                        }
                        //Serialize the command to send, and send it
                        string serialized = JsonConvert.SerializeObject(toSend);
                        if (e.CommandID == (int)CommandEnum.CloseCommand)
                        {
                            if (success)
                            {
                                broadcastAllClients(serialized);
                                continue;
                            }
                        }
                        m_mutex.WaitOne();
                        writer.Write(serialized);
                        m_mutex.ReleaseMutex();
                       // m_logging.Log("Sent to client: " + toSend.Args, MessageTypeEnum.INFO);
                    }
                }

                catch (Exception)
                {
                }
                m_allClients.Remove(clientInfo);
                client.Close();

            }).Start();
        }
        /// <summary>
        /// Called when a new log is written. Send it to all clients
        /// </summary>
        /// <param name="e">The log</param>
        /// <param name="sender">not relevant</param>

        public void UpdateClientsNewLog(object sender, MessageRecievedEventArgs e)
        {
            List<JObject> temp = new List<JObject>();
            JObject jsonObject = new JObject();
            jsonObject["logType"] = Enum.GetName(typeof(MessageTypeEnum), e.Status);
            jsonObject["logInfo"] = e.Message;
            temp.Add(jsonObject);

            string args = JsonConvert.SerializeObject(temp);
            TCPEventArgs toSend = new TCPEventArgs((int)CommandEnum.LogCommand, args);
            string serialized = JsonConvert.SerializeObject(toSend);
            //Send it to AllClients
            broadcastAllClients(serialized);
        }
        /// <summary>
        /// Send the string to all clients
        /// </summary>
        /// <param name="serialized">The message to clients</param>

        public void broadcastAllClients(string serialized)
        {
            foreach (ClientInformation cInfo in m_allClients)
            {
                try
                {
                    m_mutex.WaitOne();
                    cInfo.writer.Write(serialized);
                    m_mutex.ReleaseMutex();
                }
                catch (Exception)
                {
                    cInfo.client.Close();
                    try
                    {
                        m_allClients.Remove(cInfo);
                    }
                    catch (Exception) { }
                }
            }
        }


    }

}
