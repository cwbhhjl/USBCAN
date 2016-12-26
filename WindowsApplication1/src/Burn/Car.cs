using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.Burn
{
    class Car
    {
        public string physicalID { set; get; }
        public string functionID { set; get; }
        public string receiveID { set; get; }
        public string flashSequence { set; get; }
        public string[] SecurityAccessType { set; get; }
        public string SecurityAccessMask { set; get; }
        public string SecurityAccessLibraryPath { set; get; }
        public bool hasFlashDriver { set; get; }
    }
}
