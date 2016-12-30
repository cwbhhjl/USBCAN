using Microsoft.VisualStudio.TestTools.UnitTesting;
using USBCAN.Burn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.Burn.Tests
{
    [TestClass()]
    public class USBCANTests
    {
        DeviceControl dc;
        [TestMethod()]
        public void ReadErrorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ConnectTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeviceControlTest()
        {
            dc = new DeviceControl(
                new ZLGCANJson()
                {
                    deviceType = 4,
                    deviceIndex = 0,
                    channel = 0,
                    timing0 = "00",
                    timing1 = "1c"
                });
            bool f = dc.Connect();
            Assert.Fail();
        }
    }
}