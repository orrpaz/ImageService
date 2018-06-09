using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using Communication;
using Infrastructure.Commands;
using Infrastructure.Enum;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        public bool alreadyGotConfig {get; set;}

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigModel()
        {
            try
            {
                Listhandlers = new ObservableCollection<string>();
                WebCommunicator client = WebCommunicator.Instance;
                client.GotCloseHandlerCommand += HandleRemoveMessage;
                client.GotConfigCommand += HandleConfigMessage;
                alreadyGotConfig = false;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Handle gotten config from server
        /// </summary>
        /// <param name="sender">server</param>
        /// <param name="message">config</param>
        public void HandleConfigMessage(object sender, TCPEventArgs message)
        {
            try
            {
                UpdateAppConfig(message);
            }
            catch (Exception){ }
        }
        /// <summary>
        /// Handle gotten remove-handler from server
        /// </summary>
        /// <param name="sender">server</param>
        /// <param name="message">config</param>
        public void HandleRemoveMessage(object sender, TCPEventArgs message)
        {
            try
            {
                Listhandlers.Remove(message.Args);
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Update properties according to the config from server
        /// </summary>
        /// <param name="config">The config that had gotten from server</param>
        public void UpdateAppConfig(TCPEventArgs config)
        {
            try
            {
                string information = config.Args;
                JObject manager = JObject.Parse(information);
                string str = (string)manager["handlerPaths"];
                List<string> str2 = JsonConvert.DeserializeObject<List<string>>(str);
                ThumbnailSize = (manager["thumbnailSize"]).ToString();
                OutputDirectory = (string)manager["outputDir"];
                LogName = (string)manager["logName"];
                SourceName = (string)manager["eventSourceName"];

                foreach (string handler in str2)
                {
                    Listhandlers.Add(handler);

                }
                alreadyGotConfig = true;
            }
            catch
            {

            }

        }


        /// <summary>
        /// Ask for config from server
        /// </summary>

        public void getConfig()
        {
            WebCommunicator client = WebCommunicator.Instance;
            client.sendToServer(new TCPEventArgs((int)CommandEnum.GetConfigCommand, null));
           // System.Threading.Thread.Sleep(500); //In order to show the info smoothly
        }

        /// <summary>
        /// Remove the handler
        /// </summary>
        /// <param name="handler">the handler to be removed</param>

        public void RemoveHandler(string handler)
        {
            WebCommunicator client = WebCommunicator.Instance;
            client.sendToServer(new TCPEventArgs((int)CommandEnum.CloseCommand, handler));
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory")]
        public string OutputDirectory { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Size")]
        public string ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Listhandlers { get; set; }
    }
}