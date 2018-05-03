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
        private static ConfigInfomation configInfomation;

       // public JObject jsonObject { get; }
       // public string json { get; }
        public string[] handlerPaths { get; }
        public string outputDir { get; }
        public string eventSourceName { get; }
        public string logName { get; }
        public int thumbnailSize { get; }

        private ConfigInfomation()
        {
            string[] handlerPaths = ConfigurationManager.AppSettings["Handler"].Split(';');
            string outputDir = ConfigurationManager.AppSettings["OutputDir"];
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            string logName = ConfigurationManager.AppSettings["LogName"];
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];


            // dynamic jsonObject = new JObject();
            // jsonObject.handlerPaths = JsonConvert.SerializeObject(handlerPaths);
            //  jsonObject.outputDir = outputDir;
            //   jsonObject.thumbnailSize = thumbnailSize;
            //   jsonObject.logName = logName;
            //   jsonObject.eventSourceName = eventSourceName;



        }
        public string ToJson()
        { 
            JObject jsonObject = new JObject {
             new JProperty("handlerPaths", JsonConvert.SerializeObject(handlerPaths)),
             new JProperty("outputDir", outputDir),
             new JProperty("thumbnailSize", thumbnailSize),
             new JProperty("logName", logName),
             new JProperty("eventSourceName", eventSourceName)
         };
            return jsonObject.ToString();
        }

        public static ConfigInfomation CreateConfigInfomation()
        {
            if (configInfomation == null)
            {
                configInfomation = new ConfigInfomation();
            }
            return configInfomation;
        }
    }
   
}