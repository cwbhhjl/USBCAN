using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.Burn
{
    class Car
    {
        public uint physicalID { set; get; }
        public uint functionID { set; get; }
        public uint receiveID { set; get; }
        public byte[] didSoftwareVersion { get; set; }
        public Queue<byte[]> flashSequence { set; get; }
        public byte[] SecurityAccessType { set; get; }
        public uint SecurityAccessMask { set; get; }
        public string SecurityAccessLibraryPath { set; get; }
        public bool hasFlashDriver { set; get; }

        Car(CarJson carJson)
        {
            physicalID = Convert.ToUInt32(carJson.physicalID, 16);
            functionID = Convert.ToUInt32(carJson.functionID, 16);
            receiveID = Convert.ToUInt32(carJson.receiveID, 16);
            //未完待续
        }
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
    }
}
