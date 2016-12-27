using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using USBCAN.UDS;

namespace USBCAN.Burn
{
    public class FlashConfig
    {
        private Thread parseThread;

        public ConfigJson config;
        public USBCANJson usbCanJson;
        public CarJson carJson;

        private JObject process;

        private volatile bool threadFlag = false;
        private object o = new object();
        private Regex reg = new Regex("_[pf]$", RegexOptions.IgnoreCase);

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
                string[] pathArray = (string[])path;
                if (pathArray.Length < 2)
                {
                    throw new ArgumentException();
                }
                string configStr = getJsonString(pathArray[0]);
                config = JsonConvert.DeserializeObject<ConfigJson>(configStr);
                JObject configJObject = JObject.Parse(configStr);
                usbCanJson = JsonConvert.DeserializeObject<USBCANJson>(configJObject[config.device].ToString()); 
                process = JObject.Parse(getJsonString(pathArray[1]));
            }
        }

        public Car parseCar(string car)
        {
            Car carObject;
            lock (o)
            {
                carJson = JsonConvert.DeserializeObject<CarJson>(getJsonString("json/car/" + car + ".json"));
                carObject = new Car(carJson);
                carObject.sequenceArray = JsonConvert.DeserializeObject<string[]>(config.sequence[carJson.flashSequence].ToString());
            }
            carObject.process = new Dictionary<string, UDSMessage>();
            carObject.process.Add(carObject.didSoftwareVersion, 
                JsonConvert.DeserializeObject<UDSMessage>(process[carObject.didSoftwareVersion].ToString()));
            foreach (var item in carObject.sequenceArray)
            {
                string processStr = reg.IsMatch(item) ? reg.Split(item)[0] : item;
                if (!carObject.process.ContainsKey(processStr))
                {
                    UDSMessage mes = JsonConvert.DeserializeObject<UDSMessage>(process[processStr].ToString());
                    carObject.process.Add(processStr, mes);
                }
            }
            return carObject;
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

    public class ConfigJson
    {
        public double configVersion { get; set; }
        public string device { get; set; }
        public string[] cars { get; set; }
        public JToken sequence { get; set; }
    }

    public class USBCANJson
    {
        public int deviceType { get; set; }
        public int deviceIndex { get; set; }
        public int channel { get; set; }
        public string timing0 { get; set; }
        public string timing1 { get; set; }
    }

    public class CarJson
    {
        public string physicalID { set; get; }
        public string functionID { set; get; }
        public string receiveID { set; get; }
        public string didSoftwareVersion { get; set; }
        public string flashSequence { set; get; }
        public string[] SecurityAccessType { set; get; }
        public string SecurityAccessMask { set; get; }
        public string SecurityAccessLibraryPath { set; get; }
        public bool hasFlashDriver { set; get; }
        public string flashDriverPath { set; get; }
    }
}
