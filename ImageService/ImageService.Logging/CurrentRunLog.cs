using ImageService.Logging.Modal;
using Infrastructure;
using Infrastructure.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    class CurrentRunLog : ICurrentRunLog
    {
        private List<MessageRecievedEventArgs> currentLog = new List<MessageRecievedEventArgs>();

        public void AddToLog(object sender, MessageRecievedEventArgs log)
        {
            currentLog.Add(log);
        }

        public void ClearCurrentLog()
        {
            foreach (MessageRecievedEventArgs log in currentLog)
            {
                currentLog.Remove(log);
            }
        }

        public string GetCurrentRunLog
        {
            get
            {
                return ToJson();
            }
        }
        private string ToJson()
        {
            List<JObject> temp = new List<JObject>();
            foreach (MessageRecievedEventArgs log in currentLog)
            {
                JObject jsonObject = new JObject();
                jsonObject["logType"] = Enum.GetName(typeof(MessageTypeEnum), log.Status);
                jsonObject["logInfo"] = log.Message;
                temp.Add(jsonObject);
            }
            return JsonConvert.SerializeObject(temp);
        }
    }
}
