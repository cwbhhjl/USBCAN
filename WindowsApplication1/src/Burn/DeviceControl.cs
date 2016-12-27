using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USBCAN.Device;

namespace USBCAN.Burn
{
    class DeviceControl
    {
        private ZLGCAN zlgCan;
        private byte timing0;
        private byte timing1;
        bool Connect()
        {

            return false;
        }

        public DeviceControl(ZLGCANJson zlgcan)
        {
            zlgCan = new ZLGCAN((ZLGCAN.HardwareType)zlgcan.deviceType, zlgcan.deviceIndex);
            zlgCan.AddCan(zlgcan.channel);
            timing0 = Convert.ToByte(zlgcan.timing0, 16);
            timing1 = Convert.ToByte(zlgcan.timing1, 16);
        }
    }
}
