using Microsoft.VisualStudio.TestTools.UnitTesting;
using USBCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.Tests
{
    [TestClass()]
    public class CRC32Tests
    {
        [TestMethod()]
        public void GetCRC32Test()
        {
            string file = "E:\\捕获2.PNG";
            uint re = CRC32.GetCRC32(file);
            Assert.AreEqual(0x89607A8F, re);
        }
    }
}