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
            //ZLGCAN usbCan = new ZLGCAN(ZLGCAN.HardwareType.VCI_USBCAN2, 0);
            //Assert.AreEqual(true, usbCan.OpenDevice());
            //Assert.AreEqual(true, usbCan.isOpen);
            //usbCan.ReadBoardInfo();
            //usbCan.AddCan(0);
            //VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();

            //config.AccCode = 0;
            //config.AccMask = 0xFFFFFFFF;
            //config.Timing0 = 0;
            //config.Timing1 = 28;
            //config.Filter = 1;
            //config.Mode = 0;
            //usbCan.canList[0].InitCan(ref config);
            //Assert.AreEqual(true, usbCan.canList[0].isInit);
            //usbCan.CloseDevice();
            //Assert.AreEqual(false, usbCan.isOpen);
        }
    }
}