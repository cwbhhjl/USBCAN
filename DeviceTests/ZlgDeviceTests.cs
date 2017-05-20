using Microsoft.VisualStudio.TestTools.UnitTesting;
using BtFlash.Device.ZLG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BtFlash.Util;

namespace BtFlash.Device.ZLG.Tests
{
    [TestClass()]
    public class ZlgDeviceTests
    {
        private ZlgDevice zlg = new ZlgDevice(HardwareType.VCI_USBCAN2, 0);

        [TestMethod()]
        public void GetErrorTest()
        {
            Assert.AreEqual(zlg.GetError(1), "CAN控制器内部FIFO溢出");
        }

        [TestMethod()]
        public void OpenDeviceTest()
        {
            if (UsbUtil.HasUsbDevice("Description", "ZLG USBCAN"))
            {
                Assert.IsTrue(zlg.OpenDevice());
                Assert.IsTrue(zlg.CloseDevice());
            }
            else
            {
                Assert.IsFalse(zlg.OpenDevice());
            }
        }

        [TestMethod()]
        public void CloseDeviceTest()
        {
            if (UsbUtil.HasUsbDevice("Description", "ZLG USBCAN"))
            {
                Assert.IsTrue(zlg.OpenDevice());
                Assert.IsTrue(zlg.CloseDevice());
            }
            else
            {
                Assert.IsFalse(zlg.CloseDevice());
            }
        }

        [TestMethod()]
        public void ReadBoardInfoTest()
        {
            VCI_BOARD_INFO info = new VCI_BOARD_INFO();
            if (UsbUtil.HasUsbDevice("Description", "ZLG USBCAN"))
            {
                Assert.IsTrue(zlg.OpenDevice());
                Assert.IsTrue(zlg.ReadBoardInfo(ref info));
                Assert.IsTrue(zlg.CloseDevice());
            }
            else
            {
                Assert.IsFalse(zlg.ReadBoardInfo(ref info));
            }
        }

        [TestMethod()]
        public void ReadErrInfoTest()
        {
            uint code;
            if (UsbUtil.HasUsbDevice("Description", "ZLG USBCAN"))
            {
                Assert.IsTrue(zlg.OpenDevice());
                Assert.IsTrue(zlg.ReadErrInfo(out code));
                Assert.IsTrue(zlg.CloseDevice());
            }
        }
    }
}