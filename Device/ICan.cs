using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtFlash.Device
{
    public class CanMessage
    {
        public uint ID;
        public byte SendType;
        public bool RemoteFlag { get; set; }

        public bool ExternFlag { get; set; }

        public IEnumerable<byte> Data;
    }

    interface ICan
    {
        bool Send(ref CanMessage msg);

        IEnumerable<CanMessage> Receive();
    }
}
