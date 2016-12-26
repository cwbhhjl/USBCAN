using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace USBCAN.Burn
{
    public class FlashConfig
    {
        public string device { get; set; }
        public string[] cars { get; set; }
        public JToken sequence { get; set; }

        private Thread parseThread;

        FlashConfig config;
        //USBCAN usbCan;
        CarJson carJson;

        JObject process;

        private volatile bool threadFlag = false;
        private object o = new object();

        public FlashConfig() { }

        public FlashConfig(string[] configPath)
        {
            parseThread = new Thread(new ParameterizedThreadStart(parseConfig));
            parseThread.IsBackground = true;
            parseThread.Start(configPath);
            while (!threadFlag)
            {
                ;
            }
        }

        private void parseConfig(object path)
        {
            lock (o)
            {
                threadFlag = true;
                string[] p = (string[])path;
                if (p.Length < 2)
                {
                    throw new ArgumentException();
                }
                string configStr = getJsonString(p[0]);
                config = JsonConvert.DeserializeObject<FlashConfig>(configStr);
                JObject configJson = JObject.Parse(configStr);
                //usbCAN = JsonConvert.DeserializeObject<USBCAN>(configJson[config.device].ToString()); 
                //config.sequence = configJson["sequence"];
                process = JObject.Parse(getJsonString(p[1]));
            }
        }

        public void parseCar(string car)
        {
            lock (o)
            {
                carJson = JsonConvert.DeserializeObject<CarJson>(getJsonString("json/car/" + car + ".json"));
            }
        }


        private static string getJsonString(string path)
        {
            string json;
            using (StreamReader sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
            }
            return json;
        }
    }
}
