using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageService
{
    class ConfigInfomation
    {

        private static ConfigInfomation instance;
        
        public List<string> handlerPaths { get; private set; }
        public string outputDir { get; }
        public string eventSourceName { get; }
        public string logName { get; }
        public int thumbnailSize { get; }

        /// <summary>
        /// Singleton Constructor
        /// </summary>
        private ConfigInfomation()
        {
            handlerPaths = new List<string>(ConfigurationManager.AppSettings["Handler"].Split(';'));
            outputDir = ConfigurationManager.AppSettings["OutputDir"];
            thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            logName = ConfigurationManager.AppSettings["LogName"];
            eventSourceName = ConfigurationManager.AppSettings["SourceName"];
        }

        /// <summary>
        /// Singelton
        /// </summary>
        public static ConfigInfomation Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigInfomation();
                }
                return instance;
            }
        }
        /// <summary>
        /// Return valus in json format
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            JObject jsonObject = new JObject();

            jsonObject["handlerPaths"] = JsonConvert.SerializeObject(handlerPaths);
            jsonObject["outputDir"] = outputDir;
            jsonObject["thumbnailSize"] = thumbnailSize;
            jsonObject["logName"] = logName;
            jsonObject["eventSourceName"] = eventSourceName;

            return jsonObject.ToString();
        }
        /// <summary>
        /// Remove a handler from current handler's list
        /// </summary>
        /// <param name="toRemove"> the handler</param>
        public void RemoveHandlers(string toRemove)
        {
            foreach(string path in handlerPaths)
            {
                if (path.Equals(toRemove))
                {
                    handlerPaths.Remove(toRemove);
                    break;
                }
            }
        }
    }
   
}