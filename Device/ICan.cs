using BtFlash.Device.ZLG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtFlash.Device
{
    public class CanMessage
    {
        public uint ID { get; set; }

        public bool IsRemote { get; set; }

        public bool IsExtern { get; set; }

        public IList<byte> Data { get; set; }

        public CanMessage(uint id, IList<byte> data, bool remote = false, bool Extern = false)
        {
            ID = id;
            if (data.Count > 8)
            {
                throw new ArgumentOutOfRangeException();
            }
            Data = data.ToList();
            IsRemote = remote;
            IsExtern = Extern;
        }
    }

    interface ICan
    {
        bool Send(CanMessage msg);

        List<CanMessage> Receive();
    }
}
