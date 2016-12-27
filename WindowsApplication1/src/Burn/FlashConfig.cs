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
    public static class FlashConfig
    {
        private static Regex reg = new Regex("_[pf]$", RegexOptions.IgnoreCase);

        public static ConfigJson ParseConfig(string configPath, out ZLGCANJson usbCanJson)
        {
            string configStr = GetJsonString(configPath);
            ConfigJson config = JsonConvert.DeserializeObject<ConfigJson>(configStr);
            JObject configJObject = JObject.Parse(configStr);
            usbCanJson = JsonConvert.DeserializeObject<ZLGCANJson>(configJObject[config.device].ToString());
            return config;
        }

        public static Car ParseCar(string car, string processPath, ConfigJson config)
        {
            Car carObject;
            CarJson carJson;

            carJson = JsonConvert.DeserializeObject<CarJson>(GetJsonString("json/car/" + car + ".json"));
            carObject = new Car(carJson);
            carObject.sequenceArray = JsonConvert.DeserializeObject<string[]>(config.sequence[carJson.flashSequence].ToString());

            JObject process = JObject.Parse(GetJsonString(processPath));
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


        private static string GetJsonString(string path)
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

    public class ZLGCANJson
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
