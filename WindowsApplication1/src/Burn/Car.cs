using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USBCAN.UDS;

namespace USBCAN.Burn
{
    public class Car
    {
        public uint physicalID { get; }
        public uint functionID { get; }
        public uint receiveID { get; }
        public string didSoftwareVersion { get; }
        public Dictionary<string, UDSMessage> process { get; internal set; }
        public string[] sequenceArray { get; internal set; }
        public byte[] securityAccessType { get; }
        public uint securityAccessMask { get; }
        public string securityAccessLibraryPath { get; }
        public bool hasFlashDriver { get; }
        public string flashDriverPath { get; }

        internal Car(CarJson carJson)
        {
            physicalID = Convert.ToUInt32(carJson.physicalID, 16);
            functionID = Convert.ToUInt32(carJson.functionID, 16);
            receiveID = Convert.ToUInt32(carJson.receiveID, 16);
            didSoftwareVersion = carJson.didSoftwareVersion;
            securityAccessType = new byte[2] 
            {
                Convert.ToByte(carJson.SecurityAccessType[0], 16),
                Convert.ToByte(carJson.SecurityAccessType[1], 16)
            };
            securityAccessMask = Convert.ToUInt32(carJson.SecurityAccessMask, 16);
            securityAccessLibraryPath = carJson.SecurityAccessLibraryPath;
            hasFlashDriver = carJson.hasFlashDriver;
            flashDriverPath = carJson.flashDriverPath;
        }
    }
}
