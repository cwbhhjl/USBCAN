using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USBCAN.UDS;

namespace USBCAN.Burn
{
    class DataControl
    {
        DeviceControl device;
        Queue<UDSMessage> dataQueue;

        DataControl(DeviceControl device)
        {
            this.device = device;
            dataQueue = new Queue<UDSMessage>();
        }

        void SendUDSMessage(uint id, UDSMessage message)
        {
            Queue<byte[]> rev;
            bool result = device.SendUDSFrame(new CanUDSFrame(id, message.Content));
            if (!result)
            {
                return;
            }

        }

        void DataToUDS(Queue<byte[]> data)
        {
            byte[] mes = data.Dequeue();

        }
    }
}
