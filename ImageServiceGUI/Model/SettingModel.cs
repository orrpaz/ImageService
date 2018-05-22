using ImageServiceGUI.Communication;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingModel : ISettingModel
    {
        public SettingModel()
        {
            Communicator client = Communicator.Instance;
            client.getFromServer(); //Make it listen to server massages
            client.GotTcpMessege += HandleMessage;
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(MessageTypeEnum.FAIL);
            Console.WriteLine(MessageTypeEnum.INFO);
            Console.WriteLine(MessageTypeEnum.WARNING);
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            getConfig();
        }
        /// <summary>
        /// Ask for config from server
        /// </summary>

        public void getConfig()
        {
            Communicator client = Communicator.Instance;
            client.sendToServer(new TCPEventArgs((int)CommandEnum.GetConfigCommand, null));
            System.Threading.Thread.Sleep(500); //In order to show the info smoothly
        }
        /// <summary>
        /// Remove the handler
        /// </summary>
        /// <param name="handler">the handler to be removed</param>

        public void RemoveHandler(string handler)
        {
            Console.WriteLine("removing handler " + handler);
            Communicator client = Communicator.Instance;
            client.sendToServer(new TCPEventArgs((int)CommandEnum.CloseCommand, handler));
        }
        /// <summary>
        /// Update properties according to the config from server
        /// </summary>
        /// <param name="config">The config that had gotten from server</param>


        public void UpdateAppConfig(TCPEventArgs config)
        {
            string information = config.Args;
            JObject manager = JObject.Parse(information);
            string str = (string)manager["handlerPaths"];
            List<string> str2 = JsonConvert.DeserializeObject<List<string>>(str);
            ThumbnailSize = (int)manager["thumbnailSize"];
            OutputDirectory = (string)manager["outputDir"];
            LogName = (string)manager["logName"];
            SourceName = (string)manager["eventSourceName"];

            //This type of CollectionView does not support changes to its SourceCollection
            //from a thread different from the Dispatcher thread.
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (string handler in str2)
                {
                    Listhandlers.Add(handler);

                }
               
            }));
        }
        /// <summary>
        /// Handle massage from server (not log message)
        /// </summary>
        /// <param name="sender">server</param>
        /// <param name="e">params</param>

        public void HandleMessage(object sender, TCPEventArgs message)
        {
            int commandId = message.CommandID;
            Console.WriteLine("Id: " + message.CommandID);
            Console.WriteLine("Path: " + message.Args);
            try
            {
                switch (commandId)
                {
                    case (int)CommandEnum.CloseCommand:
                        try {
                            App.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                Listhandlers.Remove(message.Args);
                            }));
                            //Console.WriteLine("Path: " + message.Args);
                            //foreach (string s in Listhandlers)
                            //{
                            //    Console.WriteLine("remaning: " + s);
                            //    if (message.Args.Equals(s))
                            //    {
                            //        Console.WriteLine("Equals!");
                            //    }
                            //}
                            //Console.WriteLine("Handler Removed!!! path: " + message.Args);
                            //Console.WriteLine(Listhandlers.ToString());
                            //foreach (string s in Listhandlers)
                            //{
                            //    Console.WriteLine("remaning: " + s);
                            //    if (message.Args.Equals(s))
                            //    {
                            //        Console.WriteLine("Equals!");
                            //    }
                            //}
                        } catch (Exception e)
                        {
                            Console.WriteLine(e.Data);
                            Console.WriteLine("Couldn't remove handler " + message.Args);
                        }
                        break;

                    case (int)CommandEnum.GetConfigCommand:
                        UpdateAppConfig(message);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string outputDirectory;
        private string sourceName;
        private string logName;
        private int thumbnailSize;
        private ObservableCollection<string> handlers = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On property change
        /// </summary>
        /// <param name="name">"prop name"</param>

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        //Property of outputdirectory
        public string OutputDirectory
        {
            set
            {
                outputDirectory = value;
                OnPropertyChanged("OutputDirectory");
            }
            get { return outputDirectory; }
        }
        //Property of source name
        public string SourceName
        {
            set
            {
                sourceName = value;
                OnPropertyChanged("SourceName");
            }
            get { return sourceName; }
        }
        //Property of log name
        public string LogName
        {
            set
            {
                logName = value;
                OnPropertyChanged("LogName");
            }
            get { return logName; }
        }
        //Property of thumbnailsize
        public int ThumbnailSize
        {
            set
            {
                thumbnailSize = value;
                OnPropertyChanged("ThumbnailSize");
            }
            get {
                return thumbnailSize;
            }
        }
        //List of handlers
        public ObservableCollection<string> Listhandlers
        {
            set
            {
                Listhandlers = value;
                OnPropertyChanged("Listhandlers");
            }
            get {
                return handlers; }
        }

    }
}
