using Microsoft.VisualStudio.TestTools.UnitTesting;
using USBCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USBCAN.Device;

namespace USBCAN.Tests
{
    [TestClass()]
    public class USBCANTests
    {
        [TestMethod()]
        public void USBCANTest()
        {
            ZLGCAN usbCan = new ZLGCAN(ZLGCAN.HardwareType.VCI_USBCAN2, 0);
            bool flag = usbCan.ReadBoardInfo();
            flag = usbCan.OpenDevice();
            flag = usbCan.OpenDevice();
            uint errcode;
            flag = usbCan.ReadErrInfo(out errcode);
            Assert.AreEqual(true, usbCan.IsOpen);
            flag = usbCan.ReadBoardInfo();
            usbCan.AddCan(0);
            //VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
            //config.AccCode = 0;
            //config.AccMask = 0xFFFFFFFF;
            //config.Timing0 = 0;
            //config.Timing1 = 28;
            //config.Filter = 1;
            //config.Mode = 0;
            usbCan.CanList[0].InitCan(0,28);
            Assert.AreEqual(true, usbCan.CanList[0].IsInit);
            usbCan.CloseDevice();
            Assert.AreEqual(false, usbCan.IsOpen);
        }
    }
}