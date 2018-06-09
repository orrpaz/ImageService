using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ImageServiceGUI.Communication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Infrastructure.Enum;
using Infrastructure.Commands;
using Infrastructure.Logs;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        private ObservableCollection<LogArgs> logs = new ObservableCollection<LogArgs>();
        /// <summary>
        /// Constructor
        /// </summary>
        
        public LogModel()
        {
            Communicator client = Communicator.Instance;
            client.GotTcpMessege += this.HandleMessage;
            getCurrentLog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// on property change
        /// </summary>
        /// <param name="property">name of prop</param>
        
        public void NotifyPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// The whole logs that appear in log tab
        /// </summary>
        
        public ObservableCollection<LogArgs> AllLogs
        {
            get
            {
                return logs;
            }

            set
            {
                logs = value;
                NotifyPropertyChanged("AllLogs");
            }
        }
        /// <summary>
        /// Ask from server the log from the start of the service
        /// </summary>

        public void getCurrentLog()
        {
            Communicator client = Communicator.Instance;
            client.sendToServer(new TCPEventArgs((int)CommandEnum.LogCommand, null));
            System.Threading.Thread.Sleep(500); //In order to show the info smoothly
        }

        /// <summary>
        /// Handle massage from server (only log messebe)
        /// </summary>
        /// <param name="sender">server</param>
        /// <param name="e">params</param>

        public void HandleMessage(object sender, TCPEventArgs message)
        {
            int commandId = message.CommandID;
            try
            {
                
                if (commandId.Equals((int)CommandEnum.LogCommand))
                {
                    List<JObject> temp = JsonConvert.DeserializeObject<List<JObject>>(message.Args);
                    foreach (JObject log in temp)
                    {
                        string logType = (string)log["logType"];
                        string logInfo = (string)log["logInfo"];
                        LogArgs toInsert = new LogArgs(logType, logInfo);
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            AllLogs.Add(toInsert);
                        }));
                        
                    }
                    foreach (var data in AllLogs)
                    {
                        Console.WriteLine(data.logType + " " + data.logInfo);
                    }



                }

                } catch (Exception e) {
                Console.WriteLine("exception: " + e.Message);
            }
        }
    }
}
