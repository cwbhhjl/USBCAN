using Microsoft.VisualStudio.TestTools.UnitTesting;
using USBCAN.Burn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.Burn.Tests
{
    [TestClass()]
    public class FlashConfigTests
    {
        [TestMethod()]
        public void FlashConfigTest()
        {
            FlashConfig f = new FlashConfig(new string[2] { "json/config.json", "json/process.json" });
            f.parseCar("N330");
            //do { } while (true);
            Assert.Fail();
        }
    }
}