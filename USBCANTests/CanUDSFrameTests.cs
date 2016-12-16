using Microsoft.VisualStudio.TestTools.UnitTesting;
using USBCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USBCAN.UDS;

namespace USBCAN.Tests
{
    [TestClass()]
    public class CanUDSFrameTests
    {
        CanUDSFrame test;
        //byte[] tmp;
        [TestMethod()]
        public void nextFrameTest()
        {
            //Queue<byte[]> q = new Queue<byte[]>();
            
            //CanUDSFrameTest();
            byte[] tmp = { 0x03, 0x10, 0x02, 0x33, 0x11, 0x78, 0xfd, 0x66, 0x11, 0x78, 0xfd, 0x66, 0x11, 0x78, 0xfd, 0x66, 0x11, 0x78, 0xfd, 0x66, 0x09 };
            test = new CanUDSFrame(tmp);
            byte[] tmp2 = test.nextFrame();
            byte[] tmp3 = test.nextFrame();
             //CollectionAssert.AreEqual(q, test.nextFrame());
            //Assert.Fail();
        }

        [TestMethod()]
        public void CanUDSFrameTest()
        {
            //tmp = { 0x10, 0x02, 0x33 };
            //test = new CanUDSFrame(tmp);
        }
    }
}