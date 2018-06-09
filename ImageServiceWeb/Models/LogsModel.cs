using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Communication;
using Infrastructure.Commands;
using Infrastructure.Enum;
using System.Collections.ObjectModel;
using Infrastructure.Logs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ImageServiceWeb.Models
{
    public class LogsModel
    {
        private ObservableCollection<LogArgs> logs = new ObservableCollection<LogArgs>();
        private ObservableCollection<LogArgs> filterdList = new ObservableCollection<LogArgs>();
        /// <summary>
        /// Constructor
        /// </summary>
        public LogsModel()
        {
            WebCommunicator client = WebCommunicator.Instance;
            client.GotLogCommand += HandleLogMessage;
            getCurrentLog();
        }

        /// <summary>
        /// Handle gotten logs from server
        /// </summary>
        /// <param name="sender">server</param>
        /// <param name="message">logs list</param>
        public void HandleLogMessage(object sender, TCPEventArgs message)
        {
            try
            {
                List<JObject> temp = JsonConvert.DeserializeObject<List<JObject>>(message.Args);
                AllLogs.Clear();
                foreach (JObject log in temp)
                {
                    string logType = (string)log["logType"];
                    string logInfo = (string)log["logInfo"];
                    LogArgs toInsert = new LogArgs(logType, logInfo);
                    AllLogs.Add(toInsert);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// The whole logs
        /// </summary>
        public ObservableCollection<LogArgs> AllLogs
        {
            get{return logs;} set {logs = value;}
        }

        public ObservableCollection<LogArgs> FilterdLogs
        {
            get { return filterdList; }
            set { filterdList = value; }
        }

        /// <summary>
        /// Ask from server the log from the start of the service
        /// </summary>

        public void getCurrentLog()
        {
            WebCommunicator client = WebCommunicator.Instance;
            client.sendToServer(new TCPEventArgs((int)CommandEnum.LogCommand, null));
        }

        public void updateFilterList(string type)
        {
            getCurrentLog();
            ObservableCollection<LogArgs> temp = new ObservableCollection<LogArgs>();
            if (type == "")
            {
                foreach (LogArgs l in AllLogs)
                {
                    temp.Add(l);
                }
            }
            else
            {
                foreach (LogArgs l in AllLogs)
                {
                    if (type.ToLower() == l.logType.ToLower())
                        temp.Add(l);
                }
            }
            FilterdLogs = temp;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Info")]
        public string Info { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Filter By:")]
        public string FilterBy { get; set; }
    }
}