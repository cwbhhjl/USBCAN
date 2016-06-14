using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN
{
    // Negative response codes
    public struct NRC
    {
        public const byte RTDNE = 0x37; // requiredTimeDelayNotExpired
        public const byte RCRRP = 0x78; // requestCorrectlyReceived-ResponsePending
    }

    // Service identifier
    public struct SI
    {
        public const byte NRSI = 0x7F; // Negative response service identifier
    }

}
